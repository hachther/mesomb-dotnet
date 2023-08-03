namespace mesomb_dotnet.testing;

public class Test
{
    private Dictionary<string, string> credentials = new Dictionary<string, string>(){
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

    public void testSignatureWithPost()
    {

    }
}