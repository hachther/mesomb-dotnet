using System.Text.Json;

namespace mesomb_dotnet.models;

public class Location
{
    public String? town;
    public String? region;
    public String? country;

    public Location(JsonElement data)
    {
        this.town = data.GetProperty("town").GetString();
        this.country = data.GetProperty("country").GetString();
        this.region = data.GetProperty("region").GetString();
    }
}