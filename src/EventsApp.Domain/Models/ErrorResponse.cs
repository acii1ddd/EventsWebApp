namespace EventsApp.Domain.Models;

public class ErrorResponse
{
    public string ErrorMessage { get; init; } = string.Empty;

    public string? Trace { get; init; } = string.Empty;
}
