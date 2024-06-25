using System.Text.Json;

namespace mesomb_dotnet.models;

/**
 * Represent an instance of application
*/
public class Application
{
    public String? Key;
    public String? Logo;
    public readonly ApplicationBalance[]? balances;
    public readonly String[]? countries;
    public String? Description;
    public bool? IsLive;
    public String? Name;
    public dynamic? Security;
    public String? Status;
    public String? Url;

    public Application(JsonElement data)
    {
        Key = data.GetProperty("key").GetString();
        Logo = data.GetProperty("logo").GetString();

        /** create the array of balances*/
        JsonElement balances = data.GetProperty("balances");

        this.balances = new ApplicationBalance[balances.GetArrayLength()];
        for (int i = 0; i < balances.GetArrayLength(); i++)
        {
            this.balances[i] = new ApplicationBalance(balances[i]);
        }

        /** create the array of countries*/
        JsonElement countries = data.GetProperty("countries");
        this.countries = new String[countries.GetArrayLength()];
        for (int i = 0; i < countries.GetArrayLength(); i++)
        {
            this.countries[i] = countries[i].GetString();
        }


        Description = data.GetProperty("description").GetString();
        IsLive = data.GetProperty("is_live").GetBoolean();
        Name = data.GetProperty("name").GetString();

        Security = data.GetProperty("security");

        Status = data.GetProperty("status").GetString();
        Url = data.GetProperty("url").GetString();
    }

}
