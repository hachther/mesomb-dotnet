using System.Text;

namespace mesomb_dotnet.util;

public class RandomGenerator
{
    private static readonly string Characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly Random Random = new Random();

    public static string Nonce()
    {
        return Nonce(40);
    }


    public static string Nonce(int length)
    {
        StringBuilder s = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            int ch = Random.Next(Characters.Length);
            s.Append(Characters[ch]);
        }
        return s.ToString();
    }
}
