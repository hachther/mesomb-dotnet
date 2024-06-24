using System.Text.Json;
namespace mesomb_dotnet.models;


public class ApplicationBalance
{
    public string? country;
    public string? currency;
    public string? provider;
    public double? value;
    public string? service_name;

    public ApplicationBalance(JsonElement data)
    {
        this.country = data.GetProperty("country").GetString();
        this.currency = data.GetProperty("currency").GetString();
        this.provider = data.GetProperty("provider").GetString();
        this.value = data.GetProperty("value").GetDouble();
        this.service_name = data.GetProperty("service_name").GetString();
    }
}