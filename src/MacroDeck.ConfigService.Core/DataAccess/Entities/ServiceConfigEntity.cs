namespace MacroDeck.ConfigService.Core.DataAccess.Entities;

public class ServiceConfigEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ConfigValue { get; set; } = "{\n}";
    public string AccessTokenHash { get; set; } = string.Empty;
    public string AccessTokenSalt { get; set; } = string.Empty;
    public int Version { get; set; }
}