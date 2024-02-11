using System.Reflection;
using Edemo.Domain.TopUp;
using Edemo.Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Edemo.Infrastructure.Common;
using Edemo.Infrastructure.Persistence.Audit;
using Edemo.Infrastructure.Persistence.Audit.Interceptors;
using Edemo.Infrastructure.Persistence.Intreceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Edemo.Infrastructure.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IMediator mediator,
    AuditSaveChangesInterceptor auditSaveChangesInterceptor,
    SoftDeleteSaveChangeInterceptor softDeleteSaveChangeInterceptor)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<AuditEntity> Audits => Set<AuditEntity>();
    public DbSet<TopUpBeneficiary> TopUpBeneficiaries => Set<TopUpBeneficiary>();
    public DbSet<TopUpTransaction> TopUpTransactions => Set<TopUpTransaction>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditSaveChangesInterceptor);
        optionsBuilder.AddInterceptors(softDeleteSaveChangeInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }
}