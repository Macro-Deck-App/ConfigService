using System.ComponentModel;
using MacroDeck.ConfigService.Core.Attributes;

namespace MacroDeck.ConfigService.Core.Enums;

public enum ErrorCode
{
    [Description("Config was not found")]
    [StatusCode(404)]
    ConfigNotFound = -1000,

    [Description("Config already exists")]
    [StatusCode(400)]
    ConfigAlreadyExists = -1100,
    
    [Description("Config is not in valid json")]
    [StatusCode(400)]
    ConfigInvalidJson = -1200,
    
    [Description("Passwords do not match")]
    [StatusCode(400)]
    PasswordsNotMatch = -2000,
    
    [Description("The current password is wrong")]
    [StatusCode(400)]
    CurrentPasswordWrong = -2100,
    
    [Description("The new password is too short")]
    [StatusCode(400)]
    NewPasswordTooShort = -2200,
}