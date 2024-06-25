using System.Text.Json;

namespace mesomb_dotnet.models;

public class Customer
{
    public String? Email;
    public String? Phone;
    public String? Town;
    public String? Region;
    public String? Country;
    public String? FirstName;
    public String? LastName;
    public String? Address;

    public Customer(JsonElement data)
    {
        Email = data.GetProperty("email").GetString();
        Phone = data.GetProperty("phone").GetString();
        Town = data.GetProperty("town").GetString();
        Region = data.GetProperty("region").GetString();
        Country = data.GetProperty("country").GetString();
        FirstName = data.GetProperty("first_name").GetString();
        LastName = data.GetProperty("last_name").GetString();
        Address = data.GetProperty("address").GetString();
    }
}
