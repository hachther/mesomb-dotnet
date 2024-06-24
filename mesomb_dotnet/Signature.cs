
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace mesomb_dotnet;

public class Signature
{
    public static String BytesToHex(byte[] bytes)
    {
        const String hexStr = "0123456789abcdef";
        char[] hexArray = new char[hexStr.Length];
        // transform the hexStr into a hexArray
        for (int i = 0; i < hexStr.Length; i++)
        {
            hexArray[i] = hexStr[i];
        }

        char[] hexChars = new char[bytes.Length * 2];
        for (int j = 0, v; j < bytes.Length; j++)
        {
            v = bytes[j] & 0xFF;
            hexChars[j * 2] = hexArray[v >>> 4];
            hexChars[j * 2 + 1] = hexArray[v & 0x0F];
        }

        return new String(hexChars);
    }

    /**
     * compute the hash of a String based on the SHA1 algorithm
     */
    public static String Sha1(String? input)
    {
        
        using (SHA1 sha1Hash = SHA1.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    /**
     * Compute the HMACSHA1 of an input String based on a key
    */
    public static String hmacSHA1SignData(String key, String input)
    {
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(key);

        HMACSHA1 hmacsha1 = new HMACSHA1(secretKeyBytes);
        byte[] dataBytes = Encoding.UTF8.GetBytes(input);
        byte[] calcHash = hmacsha1.ComputeHash(dataBytes);
        return BytesToHex(calcHash);
    }

    public static String signRequest(String service, String method, String url, DateTime date,
        String nonce, Dictionary<String, String> credentials, SortedDictionary<string, string>? headers,
        Dictionary<string, object>? body)
    {
        String algorithm = MeSomb.algorithm;

        Uri parse = new Uri(url);

        String canonicalQuery = parse.Query != null ? parse.Query : "";
        long timestamp = ((DateTimeOffset)date.ToLocalTime()).ToUnixTimeSeconds();

        if (headers == null)
        {
            headers = new SortedDictionary<String, String>();
        }

        headers.Add("host", parse.Scheme + "://" + parse.Host + (parse.Port > 0 ? ":" + parse.Port : ""));
        headers.Add("x-mesomb-date", timestamp.ToString());
        headers.Add("x-mesomb-nonce", nonce);

        String[] headersTokens = new String[headers.Count];
        String[] headersKeys = new String[headers.Count];
        int i = 0;
        foreach (String key in headers.Keys)
        {
            headersTokens[i] = key + ":" + headers[key];
            headersKeys[i] = key;
            i++;
        }

        String canonicalHeaders = String.Join("\n", headersTokens);

        Console.WriteLine(JsonSerializer.Serialize(body, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }));
        String payloadHash = Sha1(body == null ? "{}" : JsonSerializer.Serialize(body, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }));

        String signedHeaders = String.Join(";", headersKeys);

        String path;

        path = Uri.UnescapeDataString(parse.AbsolutePath);

        String canonicalRequest = method + "\n" + path + "\n" + canonicalQuery + "\n" + canonicalHeaders + "\n" + signedHeaders + "\n" + payloadHash;

        String dateFormat = "yyyyMMdd";

        String scope = date.ToString(dateFormat) + "/" + service + "/mesomb_request";

        String StringToSign = algorithm + "\n" + timestamp + "\n" + scope + "\n" + Sha1(canonicalRequest);

        String signature = hmacSHA1SignData(credentials["secretKey"], StringToSign);
        return algorithm + " Credential=" + credentials["accessKey"] + "/" + scope + ", SignedHeaders=" + signedHeaders + ", Signature=" + signature;
    }
}
