using System;
using mesomb_dotnet;
using mesomb_dotnet.operations;
using mesomb_dotnet.util;
namespace app;

// class for futher testing needs of the final package
public class Program
{
    static void Main(string[] args)
    { 
        // String applicationKey = "2bb525516ff374bb52545bf22ae4da7d655ba9fd";
        // String accessKey = "c6c40b76-8119-4e93-81bf-bfb55417b392";
        // String secretKey = "fe8c2445-810f-4caa-95c9-778d51580163";
        String applicationKey = "1a88b9d6e534c024493f75e38882a79bbac61c08";
        String accessKey = "55da76d2-e8e2-4841-b0fe-6558eb5d88f1";
        String secretKey = "2af75288-d287-426c-8feb-9369c59d26bb";
        
        PaymentOperation paymentOperation = new(
            applicationKey,
            accessKey,
            secretKey
        );
        // MeSomb.apiBase = "http://192.168.100.10:8000";
        Dictionary<string, object> parameters = new Dictionary<string, object>() 
        {
            {"payer", "677550203" },
            { "amount", "100" },
            { "nonce", RandomGenerator.Nonce() },
            {"service", "MTN" } 
        };

        var response = (paymentOperation.MakeCollectAsync(parameters)).GetAwaiter().GetResult();
        //String url = "http://127.0.0.1:8000/en/api/v1.1/payment/collect/";
        //String actual = Signature.signRequest("payment", "GET", url, new DateTime(1673827200000L), RandomGenerator.Nonce(), credentials, null, null);
        Console.WriteLine(response.Transaction.Pk);
    }
}
