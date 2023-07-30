using MacroDeck.ConfigService.Core.DataTypes;
using MacroDeck.ConfigService.Core.ManagerInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MacroDeck.ConfigService.Controllers;

[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UserController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest loginRequest)
    {
        return await _userManager.LoginUser(loginRequest);
    }

    [HttpGet("check")]
    [Authorize]
    public async Task<ActionResult<LoginResponse>> CheckAuthentication()
    {
        return await _userManager.CheckUser(User);
    }

    [HttpPost("changePassword")]
    [Authorize]
    public async Task ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        await _userManager.ChangePassword(User, changePasswordRequest);
    }
}