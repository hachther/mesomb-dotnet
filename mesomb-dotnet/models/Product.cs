using Newtonsoft.Json.Linq;

namespace mesomb_dotnet.models;

public class Product
{
    public String name { get; set; }
    public String category { get; set; }
    public int quantity { get; set; }
    public float amount { get; set; }

    public Product(JObject data)
    {
        this.name = (String)data.GetValue("name");
        this.category = (String)data.GetValue("category");
        this.quantity = (int)data.GetValue("quantity");
        this.amount = (float)data.GetValue("amount");
    }
}