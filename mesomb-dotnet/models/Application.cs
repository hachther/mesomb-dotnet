using System.Text.Json;
using System.Collections.Generic;

namespace mesomb_dotnet.models;

/**
 * Represent an instance of application
*/
public class Application
{
    public String? key;
    public String? logo;
    public ApplicationBalance[]? balances;
    public String[]? countries;
    public String? description;
    public bool? isLive;
    public String? name;
    public dynamic? security;
    public String? status;
    public String? url;

    public Application(JsonElement data)
    {
        this.key = data.GetProperty("key").GetString();
        this.logo = data.GetProperty("logo").GetString();

        /** create the array of balances*/
        JsonElement balances = (JsonElement)data.GetProperty("balances");

        this.balances = new ApplicationBalance[balances.GetArrayLength()];
        for (int i = 0; i < balances.GetArrayLength(); i++)
        {
            this.balances[i] = new ApplicationBalance((JsonElement)balances[i]);
        }

        /** create the array of countries*/
        JsonElement countries = (JsonElement)data.GetProperty("countries");
        this.countries = new String[countries.GetArrayLength()];
        for (int i = 0; i < countries.GetArrayLength(); i++)
        {
            this.countries[i] = countries[i].GetString();
        }


        this.description = data.GetProperty("description").GetString();
        this.isLive = data.GetProperty("is_live").GetBoolean();
        this.name = data.GetProperty("name").GetString();

        this.security = (JsonElement)data.GetProperty("security");

        this.status = data.GetProperty("status").GetString();
        this.url = data.GetProperty("url").GetString();
    }

}