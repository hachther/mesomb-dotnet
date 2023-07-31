using Newtonsoft.Json.Linq;

namespace mesomb_dotnet.models;

public class Location
{
    public String town { set; get; }
    public String region { set; get; }
    public String country { set; get; }

    public Location(JObject data)
    {
        this.town = (String)data.GetValue("town");
        this.country = (String)data.GetValue("country");
        this.region = (String)data.GetValue("region");
    }
}