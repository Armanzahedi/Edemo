using Edemo.Domain.TopUp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edemo.Infrastructure.Persistence.Configurations;

public class TopUpBeneficiaryConfiguration : BaseEntityTypeConfiguration<TopUpBeneficiary>
{
    public override void Configure(EntityTypeBuilder<TopUpBeneficiary> builder)
    {
        builder.OwnsOne(x => x.Nickname, nickname =>
        {
            nickname
                .Property(x=>x.Value)
                .IsRequired()
                .HasColumnName("Nickname");
        });

        builder.OwnsOne(x => x.PhoneNumber,
            phoneNumber =>
            {
                phoneNumber
                    .Property(x => x.Number)
                    .IsRequired()
                    .HasColumnName("PhoneNumber");
            });

        builder
            .HasMany(p => p.TopUpTransactions)
            .WithOne()
            .HasForeignKey(i => i.BeneficiaryId)
            .OnDelete(DeleteBehavior.NoAction);

        base.Configure(builder);
    }
}