using MacroDeck.ConfigService.Core.Enums;

namespace MacroDeck.ConfigService.Core.DataTypes;

public class LoginResponse
{
    public string UserName { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime ValidUntil { get; set; }
}