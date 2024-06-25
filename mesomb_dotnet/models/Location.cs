using System.Text.Json;

namespace mesomb_dotnet.models;

public class Location
{
    public String? Town;
    public String? Region;
    public String? Country;

    public Location(JsonElement data)
    {
        Town = data.GetProperty("town").GetString();
        Region = data.GetProperty("country").GetString();
        Country = data.GetProperty("region").GetString();
    }
}
