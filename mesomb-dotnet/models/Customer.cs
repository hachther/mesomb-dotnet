using Newtonsoft.Json.Linq;

namespace mesomb_dotnet.models;

public class Customer
{
    public String email { set; get; }
    public String phone { set; get; }
    public String town { set; get; }
    public String region { set; get; }
    public String country { set; get; }
    public String first_name { set; get; }
    public String last_name { set; get; }
    public String address { set; get; }

    public Customer(JObject data)
    {
        this.email = (String)data.GetValue("email");
        this.phone = (String)data.GetValue("phone");
        this.town = (String)data.GetValue("town");
        this.region = (String)data.GetValue("region");
        this.country = (String)data.GetValue("country");
        this.first_name = (String)data.GetValue("first_name");
        this.last_name = (String)data.GetValue("last_name");
        this.address = (String)data.GetValue("address");
    }
}