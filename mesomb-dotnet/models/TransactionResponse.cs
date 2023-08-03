using System.Text.Json;

namespace mesomb_dotnet.models;

public class TransactionResponse
{
    public bool success;
    public String? message;
    public String? redirect;
    public Transaction? transaction;
    public String? reference;
    public String? status;

    public TransactionResponse(JsonElement data)
    {
        this.success = data.GetProperty("success").GetBoolean();
        this.message = data.GetProperty("message").GetString();
        this.redirect = data.GetProperty("redirect").GetString();
        this.transaction = new Transaction((JsonElement)data.GetProperty("transaction"));
        this.reference = data.GetProperty("reference").GetString();
        this.status = data.GetProperty("status").GetString();
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