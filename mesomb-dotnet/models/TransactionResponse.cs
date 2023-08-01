using Newtonsoft.Json.Linq;

namespace mesomb_dotnet.models;

public class TransactionResponse
{
    public bool success { get; set; }
    public String message { get; set; }
    public String redirect { get; set; }
    public Transaction transaction { get; set; }
    public String reference { get; set; }
    public String status { get; set; }

    public TransactionResponse(JObject data)
    {
        this.success = (bool)data.GetValue("success");
        this.message = (String)data.GetValue("message");
        this.redirect = (String)data.GetValue("redirect");
        this.transaction = new Transaction((JObject)data.GetValue("transaction"));
        this.reference = (String)data.GetValue("reference");
        this.status = (String)data.GetValue("status");
    }

    public bool isOperationSuccess()
    {
        return this.success;
    }

    public bool isTransactionSuccess()
    {
        return this.success && this.status == "SUCCESS";
    }
}