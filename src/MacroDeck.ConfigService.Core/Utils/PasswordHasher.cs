using System.Security.Cryptography;
using System.Text;

namespace MacroDeck.ConfigService.Core.Utils;

public static class PasswordHasher
{
    private const int SaltSize = 32;

    public static (string Hash, string Salt) HashPassword(string password)
    {
        var saltBytes = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        using var hmac = new HMACSHA512(saltBytes);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        var hashedPassword = Convert.ToBase64String(hashBytes);
        var salt = Convert.ToBase64String(saltBytes);

        return (hashedPassword, salt);
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        
        using var hmac = new HMACSHA512(saltBytes);
        var computedHashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

        var computedHash = Convert.ToBase64String(computedHashBytes);

        return computedHash.Equals(storedHash);
    }
}