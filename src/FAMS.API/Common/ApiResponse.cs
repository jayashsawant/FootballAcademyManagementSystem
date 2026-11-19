namespace FAMS.API.Common;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Message { get; init; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null) =>
        new()
        {
            Success = true,
            Data = data,
            Message = message
        };

    public static ApiResponse<T> FailureResponse(string message) =>
        new()
        {
            Success = false,
            Data = default,
            Message = message
        };
}