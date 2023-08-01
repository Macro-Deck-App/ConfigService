namespace MacroDeck.ConfigService.Core.Attributes;

public class StatusCodeAttribute : Attribute
{
    public int StatusCode { get; }

    public StatusCodeAttribute(int statusCode)
    {
        StatusCode = statusCode;
    }
}