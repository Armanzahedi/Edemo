using Edemo.Domain.Common.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edemo.Infrastructure.Persistence;

public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        if (typeof(T).GetInterfaces().Any(i => i == typeof(ISoftDeletableEntity)))
        {
            builder.HasQueryFilter(t => ((ISoftDeletableEntity)t).IsDeleted == false);
        }
    }
}