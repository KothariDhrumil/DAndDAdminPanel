namespace SharedKernel;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

}


public static class GenericErrors
{
    public static Error UnAuthorized => Error.Problem(
        "NotAuthorized",
        "You are not Authorized");

    public static Error UserNotFound => Error.Problem(
        "InvalidUser",
        "User not found");

    public static Error NotFound => Error.Problem(
        "Invalid",
        "Item not found");

    public static Error Fobidden => Error.Problem(
        "NotAuthorized",
        "You are not authorized to access this resource");

}