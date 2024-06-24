using System.Text.Json;

namespace mesomb_dotnet.models;

public class Transaction
{
    public String? pk;
    public String? status;
    public String? type;
    public Double? amount;
    public Double? fees;
    public String? b_party;
    public String? message;
    public String? service;
    public String? reference;
    public DateTime? ts;
    public String? country;
    public String? currency;
    public String? fin_trx_id;
    public Double? trxamount;
    public Customer customer;
    public Location location;
    public Product[] products;

    public Transaction(JsonElement data)
    {
        this.pk = data.GetProperty("pk").GetString();
        this.status = data.GetProperty("status").GetString();
        this.type = data.GetProperty("type").GetString();
        this.amount = data.GetProperty("amount").GetDouble();
        this.fees = data.GetProperty("fees").GetDouble();
        this.b_party = data.GetProperty("b_party").GetString();
        this.message = data.GetProperty("message").GetString();
        this.service = data.GetProperty("service").GetString();
        this.reference = data.GetProperty("reference").GetString();
        this.ts = data.GetProperty("ts").GetDateTime();
        this.country = data.GetProperty("country").GetString();
        this.currency = data.GetProperty("currency").GetString();
        this.fin_trx_id = data.GetProperty("fin_trx_id").GetString();
        this.trxamount = data.GetProperty("trxamount").GetDouble();

        if (data.GetProperty("customer").ToString() != null)
        {
            this.customer = new Customer((JsonElement)data.GetProperty("customer"));
        }

        if (data.GetProperty("location").ToString() != null)
        {
            this.location = new Location((JsonElement)data.GetProperty("location"));
        }

        if (data.GetProperty("products").ToString() != null)
        {
            JsonElement products = (JsonElement)data.GetProperty("products");
            this.products = new Product[products.GetArrayLength()];
            for (int i = 0; i < products.GetArrayLength(); i++)
            {
                this.products[i] = new Product((JsonElement)products[i]);
            }
        }
    }
}