namespace mesomb_dotnet.exceptions;

public class ServerException : Exception
{
    private String code { get; set; }

    public ServerException(String message, String code) : base(message)
    {
        this.code = code;
    }
}