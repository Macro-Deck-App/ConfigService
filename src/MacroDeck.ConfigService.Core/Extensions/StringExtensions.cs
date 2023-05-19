using System.Text.Json;
using MacroDeck.ConfigService.Core.Utils;

namespace MacroDeck.ConfigService.Core.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string string1, string stringToCompare)
    {
        return string1.Equals(stringToCompare, StringComparison.OrdinalIgnoreCase);
    }
    
    public static string EncodeBase64(this string plainText)
    {
        return Base64Utils.Base64Encode(plainText);
    }

    public static string DecodeBase64(this string base64EncodedData)
    {
        return Base64Utils.Base64Decode(base64EncodedData);
    }

    public static string TryTrimJson(this string jsonString)
    {
        try
        {
            return TrimJson(jsonString);
        }
        catch
        {
            return jsonString;
        }
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

    private static string TrimJson(this string jsonString)
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