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
        PaymentOperation paymentOperation = new(MeSomb.apiKey,MeSomb.clientKey,MeSomb.secretKey,MeSomb.apiBase,MeSomb.apiVersion);
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