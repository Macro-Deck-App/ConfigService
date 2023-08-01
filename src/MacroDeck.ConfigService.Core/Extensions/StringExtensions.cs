using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MacroDeck.ConfigService.Core.Utils;

namespace MacroDeck.ConfigService.Core.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string string1, string stringToCompare)
    {
        return string1.Equals(stringToCompare, StringComparison.OrdinalIgnoreCase);
    }
    
    public static bool EqualsCryptographically(this string? str1, string? str2)
    {
        if (str1 is null || str2 is null)
        {
            return false;
        }
        
        var hash1 = SHA256.HashData(Encoding.UTF8.GetBytes(str1));
        var hash2 = SHA256.HashData(Encoding.UTF8.GetBytes(str2));

        if (hash1.Length != hash2.Length)
        {
            return false;
        }

        return !hash1.Where((t, i) => t != hash2[i]).Any();
    }
    
    public static string EncodeBase64(this string plainText)
    {
        return Base64Utils.Base64Encode(plainText);
    }

    public static string DecodeBase64(this string base64EncodedData)
    {
        return Base64Utils.Base64Decode(base64EncodedData);
    }
    
    public static string TryWriteJsonIndented(this string jsonString)
    {
        try
        {
            return jsonString.WriteJsonIndented();
        }
        catch
        {
            return jsonString;
        }
    }

    public static string TrimJson(this string jsonString)
    {
        var document = JsonDocument.Parse(jsonString);
        return JsonSerializer.Serialize(document, new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = false
        });
    }

    private static string WriteJsonIndented(this string jsonString)
    {
        var document = JsonDocument.Parse(jsonString);
        return JsonSerializer.Serialize(document, new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            WriteIndented = true
        });
    }
}