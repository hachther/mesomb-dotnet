using System;
using mesomb_dotnet;

namespace app;

// class for futher testing needs of the final package
public class Program
{
    static void Main(string[] args)
    {
        String url = "http://127.0.0.1:8000/en/api/v1.1/payment/collect/";
        Dictionary<String, String> credentials = new Dictionary<string, string>();
        credentials["accessKey"] = "c6c40b76-8119-4e93-81bf-bfb55417b392";
        credentials["secretKey"] = "fe8c2445-810f-4caa-95c9-778d51580163";
        DateTime date = DateTimeOffset.FromUnixTimeSeconds(1673827200).DateTime;
        // String actual = Signature.signRequest(
        //     "payment", 
        //     "GET", 
        //     url, 
        //     DateTimeOffset.FromUnixTimeSeconds(1673827200).DateTime, 
        //     "fihser", 
        //     credentials, 
        //     null, 
        //     null
        // );
        var data = new Dictionary<string, object>
        {
            { "amount", 100 },
            { "service", "MTN" },
            { "payer", "670000000" },
            { "trxID", "1" },
            { "products", new List<Dictionary<string, string>> {
                new Dictionary<string, string> {
                    { "id", "SKU001" },
                    { "name", "Sac a Dos" },
                    { "category", "Sac" }
                }
            }},
            { "customer", new Dictionary<string, string> {
                { "phone", "+237677550439" },
                { "email", "fisher.bank@gmail.com" },
                { "first_name", "Fisher" },
                { "last_name", "BANK" }
            }},
            { "location", new Dictionary<string, string> {
                { "town", "Douala" },
                { "country", "Cameroun" }
            }}
        };
        String actual = Signature.signRequest(
            "payment", 
            "POST", 
            url, 
            DateTimeOffset.FromUnixTimeSeconds(1673827200).DateTime, 
            "fihser", 
            credentials, 
            null, 
            data
        );
        Console.WriteLine(actual);
    }
}
