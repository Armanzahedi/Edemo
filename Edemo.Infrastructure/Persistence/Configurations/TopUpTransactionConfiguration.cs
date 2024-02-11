using Edemo.Domain.TopUp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edemo.Infrastructure.Persistence.Configurations;

public class TopUpTransactionConfiguration : BaseEntityTypeConfiguration<TopUpTransaction>
{
    public override void Configure(EntityTypeBuilder<TopUpTransaction> builder)
    {

        builder
            .HasIndex(t => new { t.BeneficiaryId, t.TransactionDate });

        builder
            .HasIndex(t => new { t.UserId, t.TransactionDate });
        
        builder.Property(x => x.Amount).HasColumnType("decimal(18,4)");
        builder.Property(x => x.Fee).HasColumnType("decimal(18,4)");
        base.Configure(builder);
    }
}