namespace Edemo.Domain.Common.Result;

public interface IError
{
    public string Code { get; }
    public string Description { get; }

    public ErrorType Type { get; }

    public int NumericType { get; }

    public Dictionary<string, object>? Metadata { get; }
}

public readonly record struct Error
{
    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public int NumericType { get; }

    public Dictionary<string, object>? Metadata { get; }


    public static Error Failure(
        string code = "General.Failure",
        string description = "A failure has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Failure, metadata);

    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Unexpected, metadata);

    public static Error Validation(
        string code = "General.Validation",
        string description = "A validation error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Validation, metadata);

    public static Error Conflict(
        string code = "General.Conflict",
        string description = "A conflict error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Conflict, metadata);

    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.NotFound, metadata);

    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Unauthorized, metadata);

    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "A 'Forbidden' error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Forbidden, metadata);



    public static Error OfException<T>(ErrorType? type = null) where T : Exception, new()
    {
        var exception = new T();
        return new(exception.Message, exception.Message, type ?? ErrorType.Unexpected, exception.Data as Dictionary<string, object>);
    }

    public static Error OfException(Exception exception, ErrorType? type = null)
    {
        return new(exception.Message, exception.Message, type ?? ErrorType.Unexpected,
            exception.Data as Dictionary<string, object>);
    }
    
    public static Error OfType(IError error)
    {
        return new Error(error.Code, error.Description, error.Type,
            error.Metadata ?? default);
    }
    public static Error OfType<T>(ErrorType? type = null) where T : IError, new()
    {
        var error = new T();
        return new(error.Code, error.Description, type ?? error.Type, error.Metadata);
    }

    public static Error Custom(
        int type,
        string code,
        string description,
        Dictionary<string, object>? metadata = null) =>
        new(code, description, (ErrorType)type, metadata);

    private Error(string code, string description, ErrorType type, Dictionary<string, object>? metadata)
    {
        Code = code;
        Description = description;
        Type = type;
        NumericType = (int)type;
        Metadata = metadata;
    }
}