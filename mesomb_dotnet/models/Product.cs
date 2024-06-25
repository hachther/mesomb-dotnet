using System.Text.Json;

namespace mesomb_dotnet.models;

public class Product
{
    public String? Name;
    public String? Category;
    public int? Quantity;
    public decimal? Amount;

    public Product(JsonElement data)
    {
        Name = data.GetProperty("name").GetString();
        Category = data.GetProperty("category").GetString();
        Quantity = data.GetProperty("quantity").GetInt32();
        Amount = data.GetProperty("amount").GetDecimal();
    }
}
