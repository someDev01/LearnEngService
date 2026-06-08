using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Hash;

public static class HashHelper
{
    public static string ComputeSha256(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);

        return Convert.ToBase64String(hash);
    }

    public static bool VerifySha256(string value, string hash)
    {
        string computedHash = ComputeSha256(value);

        bool result = CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computedHash),
            Convert.FromBase64String(hash));
        return result;
    }
}
