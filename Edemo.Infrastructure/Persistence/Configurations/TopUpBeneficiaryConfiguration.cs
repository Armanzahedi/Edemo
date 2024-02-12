using Edemo.Domain.TopUp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edemo.Infrastructure.Persistence.Configurations;

public class TopUpBeneficiaryConfiguration : BaseEntityTypeConfiguration<TopUpBeneficiary>
{
    public override void Configure(EntityTypeBuilder<TopUpBeneficiary> builder)
    {
        builder.ComplexProperty(x => x.Nickname, nickname => nickname.IsRequired());
        
        builder.ComplexProperty(x => x.PhoneNumber,
            phoneNumber => phoneNumber.IsRequired());

        builder
            .HasMany(p => p.TopUpTransactions)
            .WithOne()
            .HasForeignKey(i => i.BeneficiaryId)
            .OnDelete(DeleteBehavior.NoAction);
        
        base.Configure(builder);
    }
}