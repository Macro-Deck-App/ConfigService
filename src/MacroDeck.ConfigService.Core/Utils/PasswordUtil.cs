using System.Security.Cryptography;
using System.Text;

namespace MacroDeck.ConfigService.Core.Utils;

public class PasswordUtil
{
    public static string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var res = new StringBuilder();
        
        while (0 < length--)
        {
            res.Append(valid[RandomNumberGenerator.GetInt32(valid.Length)]);
        }
        return res.ToString();
    }

}