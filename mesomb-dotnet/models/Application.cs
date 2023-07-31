using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace mesomb_dotnet.models;

/**
 * Represent an instance of application
*/
public class Application
{
    public String key { get; set; }
    public String logo { get; set; }
    public ApplicationBalance[] balances { get; set; }
    public String[] countries { get; set; }
    public String description { get; set; }
    public bool isLive { get; set; }
    public String name { get; set; }
    public dynamic security;
    public String status { get; set; }
    public String url { get; set; }

    public Application(JObject data)
    {
        this.key = (String)data.GetValue("key");
        this.logo = (String)data.GetValue("logo");

        /** create the array of balances*/
        JObject balances = (JObject)data.GetValue("balances");
        this.balances = new ApplicationBalance[balances.Values().Count()];
        for (int i = 0; i < balances.Values().Count(); i++)
        {
            this.balances[i] = new ApplicationBalance((JObject)balances[i]);
        }

        /** create the array of countries*/
        JObject countries = (JObject)data.GetValue("countries");
        this.countries = new String[countries.Values().Count()];
        for (int i = 0; i < countries.Values().Count(); i++)
        {
            this.countries[i] = (String)(countries[i]);
        }

        this.description = (String)data.GetValue("description");
        this.isLive = (bool)data.GetValue("is_live");
        this.name = (String)data.GetValue("name");

        this.security = (JObject)data.GetValue("security");

        this.status = (String)data.GetValue("status");
        this.url = (String)data.GetValue("url");
    }

}