using MacroDeck.ConfigService.Core.DataAccess.Entities;

namespace MacroDeck.ConfigService.Core.ManagerInterfaces;

public interface IJwtTokenManager
{
    public double TokenExpirationSeconds { get; }
    public string GenerateToken(UserEntity user);
}