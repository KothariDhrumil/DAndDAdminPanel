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

public class PagedResult<T> : Result
{
    public T? Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }

    public PagedResult(T data, Error error, int pageNumber = 1, int pageSize = 10, int? totalRecords = null)
        : base(true, error)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Data = data;

        if (totalRecords.HasValue)
        {
            TotalRecords = totalRecords.Value;
        }
        else
        {
            // Try to infer count from data
            if (data is System.Collections.ICollection collection)
            {
                TotalRecords = collection.Count;
            }
            else if (data is System.Collections.IEnumerable enumerable)
            {
                int count = 0;
                var enumerator = enumerable.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext()) count++;
                }
                finally
                {
                    (enumerator as IDisposable)?.Dispose();
                }
                TotalRecords = count;
            }
            else
            {
                TotalRecords = data != null ? 1 : 0;
            }
        }
    }

    public static PagedResult<TValue> Success<TValue>(TValue value, int pageNumber = 1, int pageSize = 10, int? totalRecords = null) =>
        new(value, Error.None, pageNumber, pageSize, totalRecords);
}