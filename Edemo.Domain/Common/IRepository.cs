using Ardalis.Specification;
using Edemo.Domain.Common.Entity;

namespace Edemo.Domain.Common;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}