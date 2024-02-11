using FluentValidation.Results;

namespace Edemo.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary<IGrouping<string, string>, string, string[]>(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray<string>());
    }

    public IDictionary<string, string[]> Errors { get; }
}