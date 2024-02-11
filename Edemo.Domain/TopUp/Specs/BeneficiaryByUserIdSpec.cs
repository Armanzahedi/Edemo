using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public class BeneficiaryByUserIdSpec : Specification<TopUpBeneficiary>
{
    public BeneficiaryByUserIdSpec(Guid userId)
    {
        Query.Where(x => x.UserId == userId);
    }
}