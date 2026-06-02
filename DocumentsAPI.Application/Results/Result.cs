namespace Services.Application.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if(isSuccess && error != Error.None || !isSuccess && error == Error.None)
            throw new ArgumentException("Invalid result state: ", nameof(error));
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    public T Value { get; }
    protected Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        Value = value;
    }
    public static Result<T> Success(T value) => new(value, true, Error.None);
    public static new Result<T> Failure(Error error) => new(default!, false, error);
    public static implicit operator Result<T>(Error error) => Failure(error);
}
