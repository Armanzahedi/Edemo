using Edemo.Domain.Common.Result;

namespace Edemo.Domain.Common.Errors;

public class NotFoundError : IError
{
    public string Code => "NotFound";
    public string Description => "Required resource was not found.";
    public ErrorType Type => ErrorType.NotFound;
    public int NumericType => (int)ErrorType.NotFound;
    public Dictionary<string, object>? Metadata { get; }
}