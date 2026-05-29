namespace ECommerce.Application.Exceptions;

public class ValidationErrorResponse
{
    public Dictionary<string, string[]> Errors { get; set; } = new();
}
