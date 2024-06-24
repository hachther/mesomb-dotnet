namespace mesomb_dotnet.exceptions;

public class InvalidClientRequestException : Exception
{
    private String code { get; set; }

    public InvalidClientRequestException(String message, String code) : base(message)
    {
        this.code = code;
    }
}