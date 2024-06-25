using System.Text.Encodings.Web;
using mesomb_dotnet.models;
using System.Text.Json;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace mesomb_dotnet.operations;

public class PaymentOperation
{
    private const string JsonMediaType = "application/json";

    private readonly string _applicationKey;
    private readonly string _accessKey;
    private readonly string _secretKey;

    public PaymentOperation(string applicationKey, string accessKey, string secretKey)
    {
        _applicationKey = applicationKey;
        _accessKey = accessKey;
        _secretKey = secretKey;
    }

    private string BuildUrl(string endpoint) => $"{MeSomb.apiBase}/api/{MeSomb.apiVersion}/{endpoint}";

    private string GetAuthorization(string method, string endpoint, DateTime date, string nonce,
        SortedDictionary<string, string>? headers = null, Dictionary<string, object>? body = null)
    {
        var url = BuildUrl(endpoint);
        var credentials = new Dictionary<string, string> { { "accessKey", _accessKey }, { "secretKey", _secretKey } };
        return Signature.signRequest("payment", method, url, date, nonce, credentials, headers, body);
    }

    private Task ProcessClientExceptionAsync(int statusCode, string response)
    {
        string? code = null;
        string message = response;

        if (message.StartsWith("{"))
        {
            Dictionary<string, object> data = JsonSerializer.Deserialize<Dictionary<string, object>>(message)!;
            message = data["detail"].ToString();
            code = data["code"]?.ToString();
        }

        switch (statusCode)
        {
            case 404: throw new ServiceNotFoundException(message);
            case 403:
            case 401: throw new PermissionDeniedException(message);
            case 400: throw new InvalidClientRequestException(message, code);
            default: throw new ServerException(message, code);
        }
    }
    
    
    private async Task<JsonElement> ExecuteRequestAsync(string method, string endpoint, DateTime date, string nonce,
        Dictionary<string, object>? body = null, string? mode = null)
    {
        var url = BuildUrl(endpoint);

        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        var request = new HttpRequestMessage(new HttpMethod(method), url);
        
        request.Headers.Add("x-mesomb-date", ((DateTimeOffset)date.ToLocalTime()).ToUnixTimeSeconds().ToString());
        request.Headers.Add("x-mesomb-nonce", nonce);
        request.Headers.Add("X-MeSomb-Application", _applicationKey);
        request.Headers.Add("X-MeSomb-OperationMode", mode);
        
        if (body != null && body.ContainsKey("trxID"))
        {
            request.Headers.Add("X-MeSomb-TrxID", body["trxID"].ToString());
            body.Remove("trxID");
        }

        if (body != null)
        {
            body["source"] = $"MeSombCSharp/{MeSomb.version}";
            request.Content = new StringContent(JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }), null, JsonMediaType);
        }
        
        var authorization = body != null && method == HttpMethod.Post.Method
            ? GetAuthorization(method, endpoint, date, nonce,
                new SortedDictionary<string, string> { { "content-type", request.Content!.Headers.ContentType!.ToString() } }, body)
            : GetAuthorization(method, endpoint, date, nonce);

        request.Headers.TryAddWithoutValidation("Authorization", authorization);

        using var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if ((int)response.StatusCode >= 300)
        {
            await ProcessClientExceptionAsync((int)response.StatusCode, responseContent);
        }
        
        return JsonSerializer.Deserialize<JsonElement>(responseContent)!;
    }

    public async Task<TransactionResponse> MakeCollectAsync(Dictionary<string, object> parameters)
    {
        var endpoint = "payment/collect/";
        var date = parameters.ContainsKey("date") ? (DateTime)parameters["date"] : DateTime.Now;

        var body = new Dictionary<string, object>
        {
            { "amount", parameters["amount"] },
            { "service", parameters["service"] },
            { "payer", parameters["payer"] },
            { "country", parameters.GetValueOrDefault("country", "CM") },
            { "currency", parameters.GetValueOrDefault("currency", "XAF") },
            { "fees", parameters.GetValueOrDefault("fees", true) },
            { "conversion", parameters.GetValueOrDefault("conversion", false) }
        };

        if (parameters.TryGetValue("trxID", out var trxID)) body["trxID"] = trxID;
        if (parameters.TryGetValue("location", out var location)) body["location"] = location;
        if (parameters.TryGetValue("customer", out var customer)) body["customer"] = customer;
        if (parameters.TryGetValue("products", out var products)) body["products"] = products;

        if (parameters.GetValueOrDefault("extra") is Dictionary<string, object> extra)
        {
            foreach (var key in extra.Keys)
            {
                body[key] = extra[key];
            }
        }

        try
        {
            var response = await ExecuteRequestAsync("POST", endpoint, date, (string)parameters["nonce"], body,
                parameters.GetValueOrDefault("mode", "synchronous").ToString());
            return new TransactionResponse(response);
        }
        catch (JsonException e)
        {
            throw new ServerException("Issue to parse transaction response", "parsing-issue");
        }
    }

    // Implement other methods (MakeDepositAsync, UpdateSecurityAsync, GetStatusAsync, GetTransactionsAsync, CheckTransactionsAsync) similarly
}

// Define exceptions
public class InvalidClientRequestException : Exception
{
    public InvalidClientRequestException(string message, string code) : base(message)
    {
    }
}

public class PermissionDeniedException : Exception
{
    public PermissionDeniedException(string message) : base(message)
    {
    }
}

public class ServerException : Exception
{
    public ServerException(string message, string code) : base(message)
    {
    }
}

public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(string message) : base(message)
    {
    }
}
