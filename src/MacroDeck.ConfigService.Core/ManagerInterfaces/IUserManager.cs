using System.Security.Claims;
using MacroDeck.ConfigService.Core.DataTypes;

namespace MacroDeck.ConfigService.Core.ManagerInterfaces;

public interface IUserManager
{
    public Task CreateDefaultUser();
    public Task ChangePassword(ClaimsPrincipal user, ChangePasswordRequest changePasswordRequest);
    public Task<LoginResponse> LoginUser(LoginRequest loginRequest);
    public Task<LoginResponse> CheckUser(ClaimsPrincipal user);
}