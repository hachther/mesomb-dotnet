using Newtonsoft.Json.Linq;

namespace mesomb_dotnet.models;

public class Customer
{
    public String email;
    public String phone;
    public String town;
    public String region;
    public String country;
    public String first_name;
    public String last_name;
    public String address;

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