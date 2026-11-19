namespace FAMS.API.Middleware;

public sealed class ErrorResponse
{
    public int StatusCode { get; init; }
    public string Message { get; init; } = string.Empty;
    public string TraceId { get; init; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; init; }
}