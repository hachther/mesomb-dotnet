using System.Text.Json;

namespace mesomb_dotnet.models;

public class Customer
{
    public String? email;
    public String? phone;
    public String? town;
    public String? region;
    public String? country;
    public String? first_name;
    public String? last_name;
    public String? address;

    public Customer(JsonElement data)
    {
        this.email = data.GetProperty("email").GetString();
        this.phone = data.GetProperty("phone").GetString();
        this.town = data.GetProperty("town").GetString();
        this.region = data.GetProperty("region").GetString();
        this.country = data.GetProperty("country").GetString();
        this.first_name = data.GetProperty("first_name").GetString();
        this.last_name = data.GetProperty("last_name").GetString();
        this.address = data.GetProperty("address").GetString();
    }
}