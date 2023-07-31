using Newtonsoft.Json.Linq;
namespace mesomb_dotnet.models;


public class ApplicationBalance
{
    public string country;
    public string currency;
    public string provider;
    public double value;
    public string service_name;

    public ApplicationBalance(JObject data)
    {
        this.country = (string)data.GetValue("country");
        this.currency = (string)data.GetValue("currency");
        this.provider = (string)data.GetValue("provider");
        this.value = (double)data.GetValue("value");
        this.service_name = (string)data.GetValue("service_name");
    }
}