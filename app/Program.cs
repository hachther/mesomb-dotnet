using System;
using mesomb_dotnet.Signature;
namespace app;

// class for futher testing needs of the final package
public class Program
{
    static void Main(string[] args)
    {
        String url = "http://127.0.0.1:8000/en/api/v1.1/payment/collect/";
        String actual = Signature.signRequest("payment", "GET", url, new DateTime(1673827200000L), "fihser", credentials, null, null);
        Console.WriteLine(url);
    }
}