namespace Edemo.Domain.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}