namespace Services.Application.Results;

public enum ErrorType
{
    Failure = 0,    // 400 BadRequest
    Validation = 1, // 400 BadRequest
    NotFound = 2,   // 404 NotFound
    Conflict = 3,   // 409 Conflict
    Forbidden = 4   // 403 Forbidden
}

public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static implicit operator Result(Error error) => Result.Failure(error);
}
