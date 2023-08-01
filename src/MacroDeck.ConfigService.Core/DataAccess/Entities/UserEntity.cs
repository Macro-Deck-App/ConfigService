using MacroDeck.ConfigService.Core.Enums;

namespace MacroDeck.ConfigService.Core.DataAccess.Entities;

public class UserEntity : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public DateTime LastLogin { get; set; }
    public UserRole Role { get; set; } = UserRole.ReadOnly;
}