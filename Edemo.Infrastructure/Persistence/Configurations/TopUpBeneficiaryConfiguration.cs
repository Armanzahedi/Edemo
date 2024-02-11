using Edemo.Domain.TopUp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edemo.Infrastructure.Persistence.Configurations;

public class TopUpBeneficiaryConfiguration : BaseEntityTypeConfiguration<TopUpBeneficiary>
{
    public override void Configure(EntityTypeBuilder<TopUpBeneficiary> builder)
    {
        builder.OwnsOne(x => x.Nickname,
            nickname =>
        {
            nickname.Property(n => n.Value)
                .HasMaxLength(20)
                .IsRequired();
        });
        builder.OwnsOne(x => x.PhoneNumber,
            phoneNumber =>
        {
            phoneNumber
                .Property(n => n.Number)
                .IsRequired();
        });

        builder
            .HasMany(p => p.TopUpTransactions)
            .WithOne()
            .HasForeignKey(i => i.BeneficiaryId)
            .OnDelete(DeleteBehavior.NoAction);
        
        base.Configure(builder);
    }
}