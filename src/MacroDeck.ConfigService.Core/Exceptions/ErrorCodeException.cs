using MacroDeck.ConfigService.Core.Enums;

namespace MacroDeck.ConfigService.Core.Exceptions;

public class ErrorCodeException : Exception
{
    public ErrorCode ErrorCode { get; }

    public ErrorCodeException(ErrorCode errorCode)
    {
        ErrorCode = errorCode;
    }
}