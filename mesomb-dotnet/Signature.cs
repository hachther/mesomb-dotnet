
using System.Security.Cryptography;
using System.Text;


namespace mesomb_dotnet;

public class Signature
{
    public static string BytesToHex(byte[] bytes)
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
     * compute the hash of a string based on the SHA1 algorithm
     */
    public static String Sha1(String? input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString());
            }
            return builder.ToString();
        }
    }

    /**
     * Compute the HMACSHA1 of an input string based on a key
    */
    public static String hmacSHA1SignData(String key, String input)
    {
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(key);

        HMACSHA1 hmacsha1 = new HMACSHA1(secretKeyBytes);
        byte[] dataBytes = Encoding.UTF8.GetBytes(input);
        byte[] calcHash = hmacsha1.ComputeHash(dataBytes);
        return BytesToHex(calcHash);
    }

    public static String signRequest(string service, string method, string url, DateTime date,
        string nonce, Dictionary<string, string> credentials, SortedDictionary<string, string> headers,
        Dictionary<string, object> body)
    {
        string algorithm = MeSomb.algorithm;

        Uri parse = new Uri(url);

        string canonicalQuery = parse.Query != null ? parse.Query : "";
        long timestamp = date.Ticks / 1000;

        if (headers == null)
        {
            headers = new SortedDictionary<string, string>();
        }

        headers.Add("host", parse.Scheme + "://" + parse.Host + (parse.Port > 0 ? ":" + parse.Port : ""));
        headers.Add("x-mesomb-date", timestamp.ToString());
        headers.Add("x-mesomb-nonce", nonce);

        string[] headersTokens = new string[headers.Count];
        string[] headersKeys = new string[headers.Count];
        int i = 0;
        foreach (string key in headers.Keys)
        {
            headersTokens[i] = key + ":" + headers[key];
            headersKeys[i] = key;
            i++;
        }

        string canonicalHeaders = string.Join("\n", headersTokens);

        string payloadHash = Sha1(body != null ? body.ToString().Replace("\\/", "/") : "{}");

        string signedHeaders = string.Join(";", headersKeys);

        string path;

        path = Uri.UnescapeDataString(parse.AbsolutePath);

        string canonicalRequest = method + "\n" + path + "\n" + canonicalQuery + "\n" + canonicalHeaders + "\n" + signedHeaders + "\n" + payloadHash;

        string dateFormat = "yyyyMMdd";

        string scope = date.ToString(dateFormat) + "/" + service + "/mesomb_request";

        string stringToSign = algorithm + "\n" + timestamp + "\n" + scope + "\n" + Sha1(canonicalRequest);

        string signature = hmacSHA1SignData(credentials["secretKey"], stringToSign);
        return algorithm + " Credential=" + credentials["accessKey"] + "/" + scope + ", SignedHeaders=" + signedHeaders + ", Signature=" + signature;
    }
}