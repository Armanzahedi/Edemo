namespace Edemo.Domain.Common.Result;

public class Result<TValue>
{
    public readonly TValue? Value;
    public readonly Error? Error;

    public readonly bool IsSuccess;
    public bool IsError => !IsSuccess;

    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }

    private Result(Exception exception)
    {
        IsSuccess = false;
        Value = default;
        Error = Result.Error.OfException(exception);
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Exception error) => new(error);
    public static implicit operator Result<TValue>(Error error) => new(error);

    public Result<TValue> Match(Func<TValue, Result<TValue>> success, Func<Error, Result<TValue>> failure)
    {
        return IsSuccess ? success(Value!) : failure(Error ?? default);
    }
    public TOut Match<TOut>(Func<TValue, TOut> success, Func<Error, TOut> failure)
    {
        return IsSuccess ? success(Value!) : failure(Error ?? default);
    }
}