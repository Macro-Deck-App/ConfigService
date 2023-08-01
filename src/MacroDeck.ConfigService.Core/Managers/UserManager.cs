using System.Security.Claims;
using MacroDeck.ConfigService.Core.DataAccess.Entities;
using MacroDeck.ConfigService.Core.DataAccess.RepositoryInterfaces;
using MacroDeck.ConfigService.Core.DataTypes;
using MacroDeck.ConfigService.Core.Enums;
using MacroDeck.ConfigService.Core.Exceptions;
using MacroDeck.ConfigService.Core.Extensions;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using MacroDeck.ConfigService.Core.Utils;

namespace MacroDeck.ConfigService.Core.Managers;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenManager _jwtTokenManager;

    public UserManager(IUserRepository userRepository, IJwtTokenManager jwtTokenManager)
    {
        _userRepository = userRepository;
        _jwtTokenManager = jwtTokenManager;
    }

    public async Task CreateDefaultUser()
    {
        var userCount = await _userRepository.CountAllAsync();
        if (userCount > 0)
        {
            return;
        }
        
        var password = PasswordHasher.HashPassword("Admin");
        var userEntity = new UserEntity
        {
            UserName = "Admin",
            PasswordHash = password.Hash,
            PasswordSalt = password.Salt,
            Role = UserRole.Admin
        };

        await _userRepository.InsertAsync(userEntity);
    }

    public async Task ChangePassword(ClaimsPrincipal user, ChangePasswordRequest changePasswordRequest)
    {
        var userName = user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new UnauthorizedException();
        }
        
        var userEntity = await _userRepository.FindAsync(x => x.UserName == userName);
        if (userEntity is null)
        {
            throw new UnauthorizedException();
        }
        
        if (!PasswordHasher.VerifyPassword(changePasswordRequest.CurrentPassword, userEntity.PasswordHash,
                userEntity.PasswordSalt))
        {
            throw new ErrorCodeException(ErrorCode.CurrentPasswordWrong);
        }

        if (string.IsNullOrWhiteSpace(changePasswordRequest.NewPassword) 
            || changePasswordRequest.NewPassword.Length < 8)
        {
            throw new ErrorCodeException(ErrorCode.NewPasswordTooShort);
        }

        if (!changePasswordRequest.NewPassword.EqualsCryptographically(changePasswordRequest.ConfirmNewPassword))
        {
            throw new ErrorCodeException(ErrorCode.PasswordsNotMatch);
        }

        var hashed = PasswordHasher.HashPassword(changePasswordRequest.NewPassword);
        userEntity.PasswordHash = hashed.Hash;
        userEntity.PasswordSalt = hashed.Salt;
        await _userRepository.UpdateAsync(userEntity);
    }

    public async Task<LoginResponse> LoginUser(LoginRequest loginRequest)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.UserName) || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            throw new UnauthorizedException();
        }
        
        var userEntity = await _userRepository.FindAsync(x => x.UserName.ToLower() == loginRequest.UserName.ToLower());
        if (userEntity is null)
        {
            throw new UnauthorizedException();
        }

        if (!PasswordHasher.VerifyPassword(loginRequest.Password, userEntity.PasswordHash, userEntity.PasswordSalt))
        {
            throw new UnauthorizedException();
        }

        await _userRepository.UpdateLastLogin(userEntity.Id);

        return GenerateLoginResponse(userEntity);
    }

    public async Task<LoginResponse> CheckUser(ClaimsPrincipal user)
    {
        var userName = user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new UnauthorizedException();
        }
        
        var userEntity = await _userRepository.FindAsync(x => x.UserName == userName);
        if (userEntity is null)
        {
            throw new UnauthorizedException();
        }

        return GenerateLoginResponse(userEntity);
    }

    private LoginResponse GenerateLoginResponse(UserEntity userEntity)
    {
        return new LoginResponse
        {
            JwtToken = _jwtTokenManager.GenerateToken(userEntity),
            Role = userEntity.Role,
            ValidUntil = DateTime.Now.AddSeconds(_jwtTokenManager.TokenExpirationSeconds),
            UserName = userEntity.UserName
        };
    }
}