namespace MacroDeck.ConfigService.Core.DataTypes;

public class EncodedConfig
{
    public int Version { get; set; }
    public string? ConfigBase64 { get; set; }
}