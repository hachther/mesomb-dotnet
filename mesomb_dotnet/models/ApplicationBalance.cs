using System.Text.Json;

namespace mesomb_dotnet.models;


public class ApplicationBalance
{
    public string? Country;
    public string? Currency;
    public string? Provider;
    public double? Value;
    public string? ServiceName;

    public ApplicationBalance(JsonElement data)
    {
        Country = data.GetProperty("country").GetString();
        Currency = data.GetProperty("currency").GetString();
        Provider = data.GetProperty("provider").GetString();
        Value = data.GetProperty("value").GetDouble();
        ServiceName = data.GetProperty("service_name").GetString();
    }
}
