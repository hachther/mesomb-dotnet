using System.Text.Json;

namespace mesomb_dotnet.models;

public class Transaction
{
    public String? Pk;
    public String? Status;
    public String? Type;
    public Double? Amount;
    public Double? Fees;
    public String? BParty;
    public String? Message;
    public String? Service;
    public String? Reference;
    public DateTime? Ts;
    public String? Country;
    public String? Currency;
    public String? FinTrxId;
    public Double? TrxAmount;
    public Customer Customer;
    public Location Location;
    public readonly Product[] Products;

    public Transaction(JsonElement data)
    {
        Pk = data.GetProperty("pk").GetString();
        Status = data.GetProperty("status").GetString();
        Type = data.GetProperty("type").GetString();
        Amount = data.GetProperty("amount").GetDouble();
        Fees = data.GetProperty("fees").GetDouble();
        BParty = data.GetProperty("b_party").GetString();
        Message = data.GetProperty("message").GetString();
        Service = data.GetProperty("service").GetString();
        Reference = data.GetProperty("reference").GetString();
        Ts = data.GetProperty("ts").GetDateTime();
        Country = data.GetProperty("country").GetString();
        Currency = data.GetProperty("currency").GetString();
        FinTrxId = data.GetProperty("fin_trx_id").GetString();
        TrxAmount = data.GetProperty("trxamount").GetDouble();

        try
        {
            Customer = new Customer(data.GetProperty("customer"));
        }
        catch (Exception e)
        {
            // ignored
        }

        try
        {
            Location = new Location(data.GetProperty("location"));
        }
        catch (Exception e)
        {
            // ignored
        }

        try
        {
            JsonElement products = data.GetProperty("products");
            Products = new Product[products.GetArrayLength()];
            for (int i = 0; i < products.GetArrayLength(); i++)
            {
                Products[i] = new Product(products[i]);
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}
