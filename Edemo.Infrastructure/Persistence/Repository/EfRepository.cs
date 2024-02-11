using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Entity;


namespace Edemo.Infrastructure.Persistence.Repository;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    private readonly AppDbContext _dbContext;
    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

}