using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MacroDeck.ConfigService.Core.DataAccess.Entities;
using MacroDeck.ConfigService.Core.Helper;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using Microsoft.IdentityModel.Tokens;

namespace MacroDeck.ConfigService.Core.Managers;

public class JwtTokenManager : IJwtTokenManager
{
    public double TokenExpirationSeconds { get; } = TimeSpan.FromMinutes(15).TotalSeconds;

    public string GenerateToken(UserEntity user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentHelper.JwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: EnvironmentHelper.JwtIssuer,
            audience: EnvironmentHelper.JwtAudience,
            claims: claims,
            expires: DateTime.Now.AddSeconds(TokenExpirationSeconds),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}