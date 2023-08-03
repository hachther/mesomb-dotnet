using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace mesomb_dotnet;

public class Signature
{
    public static string BytesToHex(byte[] bytes)
    {
        const String hexStr = "0123456789abcdef";
        char[] hexArray = new char[hexStr.Length];
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

        return new string(hexChars);
    }

    /**
     * compute the hash of a string based on the SHA1 algorithm
     */
    public static String Sha1(string input)
    {
        HashAlgorithm ha = HashAlgorithm.Create("SHA-1");
        return BytesToHex(ha.ComputeHash(Encoding.ASCII.GetBytes(input)));
    }

    /**
     * Compute the HMACSHA1 of an input string based on a key
    */
    public static String hmacSha1(String key, String input)
    {
        HMACSHA1 hmacSHA1 = new HMACSHA1();
        hmacSHA1.Key = Encoding.ASCII.GetBytes(key);

        return BytesToHex(hmacSHA1.ComputeHash(Encoding.ASCII.GetBytes(input)));
    }

    public static String signRequest(string service, string method, string url, DateTime date,
        string nonce, Dictionary<string, string> credentials, SortedDictionary<string, string> headers,
        Dictionary<string, object> body)
    {
        string algorithm = MeSomb.algorithm;

        Uri parse = new Uri(url);

        string canonicalQuery = parse.Query != null ? parse.Query : "";
        long timestamp = date.Ticks / TimeSpan.TicksPerSecond;

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

        string payloadHash = Sha1(body != null ? body.ToString() : "{}");

        string signedHeaders = string.Join(";", headersKeys);

        string path;


        path = Uri.UnescapeDataString(parse.AbsolutePath);


        string canonicalRequest = method + "\n" + path + "\n" + canonicalQuery + "\n" + canonicalHeaders + "\n" + signedHeaders + "\n" + payloadHash;

        string dateFormat = "yyyyMMdd";

        string scope = date.ToString(dateFormat) + "/" + service + "/mesomb_request";

        string stringToSign = algorithm + "\n" + timestamp + "\n" + scope + "\n" + Sha1(canonicalRequest);

        string signature = hmacSha1(credentials["secretKey"], stringToSign);

        return algorithm + " Credential=" + credentials["accessKey"] + "/" + scope + ", SignedHeaders=" + signedHeaders + ", Signature=" + signature;
    }
}