namespace SharedKernel;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}

public class Result<T> : Result
{
    public T? Data { get; set; }

    public Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Data = value;
    }
}

public class PagedResult<T> : Result<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PagedResult(T data, int pageNumber, int pageSize, Error error) : base(data, true, error)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Data = data;
    }
}