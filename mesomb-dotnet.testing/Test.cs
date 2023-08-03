namespace mesomb_dotnet.testing;

public class Test
{
    private Dictionary<string, string> credentials = new Dictionary<string, string>(){
        {"accessKey", "c6c40b76-8119-4e93-81bf-bfb55417b392"},
        {"secretKey", "fe8c2445-810f-4caa-95c9-778d51580163"}
    };

    [Fact]
    public void testSignatureWithGet()
    {
        Console.WriteLine("papa");
    }
}