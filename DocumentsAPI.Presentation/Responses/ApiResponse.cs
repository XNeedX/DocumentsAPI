namespace Profiles.Presentation.Responses;

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponse Success(string? message = null) =>
        new ApiResponse { IsSuccess = true, Message = message };

    public static ApiResponse Failure(List<string> errors, string? message = null) =>
        new ApiResponse { IsSuccess = false, Errors = errors, Message = message };

    public static ApiResponse Failure(string error, string? message = null) =>
        new ApiResponse { IsSuccess = false, Errors = new List<string> { error }, Message = message };
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string? message = null) =>
        new ApiResponse<T> { IsSuccess = true, Data = data, Message = message };

    public static new ApiResponse<T> Failure(List<string> errors, string? message = null) =>
        new ApiResponse<T> { IsSuccess = false, Errors = errors, Message = message };

    public static new ApiResponse<T> Failure(string error, string? message = null) =>
        new ApiResponse<T> { IsSuccess = false, Errors = new List<string> { error }, Message = message };
}