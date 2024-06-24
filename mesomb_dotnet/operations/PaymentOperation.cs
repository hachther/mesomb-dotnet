using mesomb_dotnet.exceptions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using mesomb_dotnet.models;

namespace mesomb_dotnet.operations;

public class PaymentOperation
{
    private readonly string ApiBase;
    private readonly string ApiVersion;
    private const string JsonMediaType = "application/json";

    private readonly string applicationKey;
    private readonly string accessKey;
    private readonly string secretKey;

    public PaymentOperation(string applicationKey, string accessKey, string secretKey,string apiBase,string apiVersion)
    {
        this.applicationKey = applicationKey;
        this.accessKey = accessKey;
        this.secretKey = secretKey;
        ApiBase = apiBase;
        ApiVersion = apiVersion;
    }

    private string BuildUrl(string endpoint) => $"{ApiBase}/en/api/{ApiVersion}/{endpoint}";

    private  string GetAuthorization(string method, string endpoint, DateTime date, string nonce, SortedDictionary<string, string> headers = null, Dictionary<string, object> body = null)
    {
        var url = BuildUrl(endpoint);
        var credentials = new Dictionary<string, string> { { "accessKey", accessKey }, { "secretKey", secretKey } };
        return  Signature.signRequest("payment", method, url, date, nonce, credentials, headers, body);
    }

    private async Task ProcessClientExceptionAsync(int statusCode, string response)
    {
        string code = null;
        string message = response;

        if (message.StartsWith("{"))
        {
            var data = JObject.Parse(message);
            message = data["detail"]?.ToString();
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

    private async Task<string> ExecuteRequestAsync(string method, string endpoint, DateTime date, string nonce = "", Dictionary<string, object> body = null, string mode = null)
    {
        var url = BuildUrl(endpoint);
        var authorization = body != null && method == HttpMethod.Post.Method
            ?  GetAuthorization(method, endpoint, date, nonce, new SortedDictionary<string, string> { { "content-type", JsonMediaType } }, body)
            : GetAuthorization(method, endpoint, date, nonce);

        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        var request = new HttpRequestMessage(new HttpMethod(method), url);

        if (body != null)
        {
            body["source"] = "MeSombCSharp/1.0";
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, JsonMediaType);
        }

        request.Headers.Add("x-mesomb-date", ((DateTimeOffset)date).ToUnixTimeSeconds().ToString());
        request.Headers.Add("x-mesomb-nonce", nonce);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
        request.Headers.Add("X-MeSomb-Application", applicationKey);

        if (mode != null) request.Headers.Add("X-MeSomb-OperationMode", mode);

        using var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if ((int)response.StatusCode >= 400)
        {
            await ProcessClientExceptionAsync((int)response.StatusCode, responseContent);
        }

        return responseContent;
    }

    public async Task<TransactionResponse> MakeCollectAsync(Dictionary<string, object> parameters)
    {
        var endpoint = "payment/collect/";
        var date = parameters.ContainsKey("date") ? (DateTime)parameters["date"] : DateTime.UtcNow;

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

        if (parameters.ContainsKey("trxID")) body["trxID"] = parameters["trxID"];
        if (parameters.ContainsKey("location")) body["location"] = parameters["location"];
        if (parameters.ContainsKey("customer")) body["customer"] = parameters["customer"];
        if (parameters.ContainsKey("products")) body["products"] = parameters["products"];

        var extra = parameters.GetValueOrDefault("extra") as Dictionary<string, object>;
        if (extra != null)
        {
            foreach (var key in extra.Keys)
            {
                body[key] = extra[key];
            }
        }

        try
        {
            var response = await ExecuteRequestAsync("POST", endpoint, date, (string)parameters["nonce"], body, parameters.GetValueOrDefault("mode", "synchronous").ToString());
            return JsonConvert.DeserializeObject<TransactionResponse>(response);
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
    public InvalidClientRequestException(string message, string code) : base(message) { }
}

public class PermissionDeniedException : Exception
{
    public PermissionDeniedException(string message) : base(message) { }
}

public class ServerException : Exception
{
    public ServerException(string message, string code) : base(message) { }
}

public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(string message) : base(message) { }
}

