namespace SharedKernel;

public class Response
{
    public Response(bool isSuccess, Error error)
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

    public static Response Success() => new(true, Error.None);

    public static Response<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Response Failure(Error error) => new(false, error);

    public static Response<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}

public class Response<T> : Response
{
    public T? Data { get; set; }

    public Response(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Data = value;
    }
} 