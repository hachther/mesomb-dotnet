namespace mesomb_dotnet.testing;

public class Test
{
    private Dictionary<String, String> credentials = new Dictionary<String, String>(){
        {"accessKey", "c6c40b76-8119-4e93-81bf-bfb55417b392"},
        {"secretKey", "fe8c2445-810f-4caa-95c9-778d51580163"}
    };

    [Fact]
    public void testSignatureWithGet()
    {
        // fixed ticks for tests needs, if we don't set that value to a default one
        // the test will fail
        long fixedTicks = 16565327890000000;
        String url = "http://127.0.0.1:8000/en/api/v1.1/payment/collect/";
        String actual = Signature.signRequest("payment", "GET", url, new DateTime(fixedTicks), "fihser", credentials, null, null);
        String expected = "HMAC-SHA1 Credential=c6c40b76-8119-4e93-81bf-bfb55417b392/00530629/payment/mesomb_request, SignedHeaders=host;x-mesomb-date;x-mesomb-nonce, Signature=b90d088ed3d72b09635a9d9e20ae2557f236231c";
        Assert.Equal(expected, actual);
    }


    [Fact]
    public void testSignatureWithPost()
    {
        long fixedTicks = 16565327890000000;
        String url = "http://127.0.0.1:8000/en/api/v1.1/payment/collect/";
        SortedDictionary<String, String> headers = new SortedDictionary<String, String>(){
            {"content-type","application/json; charset=utf-8"}
        };

        Dictionary<String, Object> products = new Dictionary<String, Object>()
        {
            {"id", "SKU001"},{"name", "Sac a Main"},{"category", "Sac"}
        };


        Dictionary<String, Object> customer = new Dictionary<String, Object>()
        {
            {"phone", "+237677550439"},{"email", "fisher.bank@gmail.com"},{"first_name", "Fisher"},
            {"last_name","BANK"}
        };

        Dictionary<String, Object> location = new Dictionary<String, Object>()
        {
            {"town", "Douala"},{"country", "Cameroun"}
        };


        Dictionary<String, Object> body = new Dictionary<String, Object>()
        {
            {"amount", 100f},
            {"service", "MTN"},
            {"payer", "670000000"},
            {"trxID","1"},
            {"products",products},
            {"customer",customer},
            {"location", location}
        };

        String actual = Signature.signRequest("payment", "POST", url, new DateTime(fixedTicks), "fisher", credentials, headers, body);
        String expected = "HMAC-SHA1 Credential=c6c40b76-8119-4e93-81bf-bfb55417b392/00530629/payment/mesomb_request, SignedHeaders=content-type;host;x-mesomb-date;x-mesomb-nonce, Signature=2620e5167d0d8f92906fb2a3c6c2cbca2deb400c";
        Assert.Equal(expected, actual);
    }
}