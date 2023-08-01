using Newtonsoft.Json.Linq;

namespace mesomb_dotnet.models;

public class Transaction
{
    public String pk { get; set; }
    public String status { get; set; }
    public String type { get; set; }
    public Double amount { get; set; }
    public Double fees { get; set; }
    public String b_party { get; set; }
    public String message { get; set; }
    public String service { get; set; }
    public String reference { get; set; }
    public DateTime ts { get; set; }
    public String country { get; set; }
    public String currency { get; set; }
    public String fin_trx_id { get; set; }
    public Double trxamount { get; set; }
    public Customer customer { get; set; }
    public Location location { get; set; }
    public Product[] products { get; set; }

    public Transaction(JObject data)
    {
        this.pk = (String)data.GetValue("pk");
        this.status = (String)data.GetValue("status");
        this.type = (String)data.GetValue("type");
        this.amount = (Double)data.GetValue("amount");
        this.fees = (Double)data.GetValue("fees");
        this.b_party = (String)data.GetValue("b_party");
        this.message = (String)data.GetValue("message");
        this.service = (String)data.GetValue("service");
        this.reference = (String)data.GetValue("reference");
        this.ts = (DateTime)data.GetValue("ts");
        this.country = (String)data.GetValue("country");
        this.currency = (String)data.GetValue("currency");
        this.fin_trx_id = (String)data.GetValue("fin_trx_id");
        this.trxamount = (Double)data.GetValue("trxamount");

        if (data.GetValue("customer") != null)
        {
            this.customer = new Customer((JObject)data.GetValue("customer"));
        }

        if (data.GetValue("location") != null)
        {
            this.location = new Location((JObject)data.GetValue("location"));
        }

        if (data.GetValue("products") != null)
        {
            JObject products = (JObject)data.GetValue("products");
            this.products = new Product[products.Values().Count()];
            for (int i = 0; i < products.Values().Count(); i++)
            {
                this.products[i] = new Product((JObject)products[i]);
            }
        }
    }
}