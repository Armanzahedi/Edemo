using Edemo.Domain.Common;

namespace Edemo.Infrastructure.Persistence;

public class UnitOfWorkInMemory : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWorkInMemory(AppDbContext context)
    {
        _context = context;
    }

    public void BeginTransaction()
    {
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Commit()
    {
     
    }

    public async Task SaveAndCommitAsync(CancellationToken cancellationToken = new())
    {
        await SaveChangesAsync(cancellationToken);
        Commit();
    }

    public void Rollback()
    {
        
    }

    public void Dispose()
    {
      
    }
}