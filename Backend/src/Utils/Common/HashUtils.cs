using System.Text;
using System.Security.Cryptography;

namespace Backend.Utils.Common;

public static class HashUtils
{
    public static string Hash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        using var algorithm = MD5.Create();
        var hashedBytes = algorithm.ComputeHash(inputBytes);
        return Convert.ToHexString(hashedBytes);
    }

    public static string RandomHash() => Hash(Guid.NewGuid().ToString());
}
