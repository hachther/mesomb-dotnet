using System.Text.Json;

namespace mesomb_dotnet.models;

public class TransactionResponse
{
    public readonly bool Success;
    public String? Message;
    public String? Redirect;
    public Transaction? Transaction;
    public readonly String? Reference;
    public readonly String? Status;

    public TransactionResponse(JsonElement data)
    {
        Success = data.GetProperty("success").GetBoolean();
        Message = data.GetProperty("message").GetString();
        Redirect = data.GetProperty("redirect").GetString();
        Transaction = new Transaction(data.GetProperty("transaction"));
        Reference = data.GetProperty("reference").GetString();
        Status = data.GetProperty("status").GetString();
    }

    public bool IsOperationSuccess()
    {
        return Success;
    }

    public bool IsTransactionSuccess()
    {
        return Success && Status == "SUCCESS";
    }
}
