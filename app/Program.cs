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
        String applicationKey = "2bb525516ff374bb52545bf22ae4da7d655ba9fd";
        String accessKey = "c6c40b76-8119-4e93-81bf-bfb55417b392";
        String secretKey = "fe8c2445-810f-4caa-95c9-778d51580163";
        
        PaymentOperation paymentOperation = new(
            applicationKey,
            accessKey,
            secretKey
        );
        MeSomb.apiBase = "http://192.168.100.10:8000";
        Dictionary<string, object> parameters = new Dictionary<string, object>() 
        {
            {"payer", "670000000" },
            { "amount", "100" },
            { "nonce", RandomGenerator.Nonce() },
            {"service", "MTN" } 
        };


        var response = (paymentOperation.MakeCollectAsync(parameters)).GetAwaiter().GetResult();
        //String url = "http://127.0.0.1:8000/en/api/v1.1/payment/collect/";
        //String actual = Signature.signRequest("payment", "GET", url, new DateTime(1673827200000L), RandomGenerator.Nonce(), credentials, null, null);
        Console.WriteLine(response.reference);
    }
}
