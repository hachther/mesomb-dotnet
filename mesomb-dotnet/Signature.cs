
using System.Security.Cryptography;
using System.Text;


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
        String nonce, Dictionary<String, String> credentials, SortedDictionary<String, String> headers,
        Dictionary<String, Object> body)
    {
        String algorithm = MeSomb.algorithm;

        Uri parse = new Uri(url);

        String canonicalQuery = parse.Query != null ? parse.Query : "";
        long timestamp = date.Ticks / 1000;

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

        String payloadHash = Sha1(body != null ? body.ToString().Replace("\\/", "/") : "{}");

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