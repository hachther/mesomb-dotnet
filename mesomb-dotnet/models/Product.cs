using System.Text.Json;

namespace mesomb_dotnet.models;

public class Product
{
    public String? name;
    public String? category;
    public int? quantity;
    public decimal? amount;

    public Product(JsonElement data)
    {
        this.name = data.GetProperty("name").GetString();
        this.category = data.GetProperty("category").GetString();
        this.quantity = data.GetProperty("quantity").GetInt32();
        this.amount = data.GetProperty("amount").GetDecimal();
    }
}