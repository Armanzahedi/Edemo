using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public class BeneficiaryByUserIdBeneficiaryIdSpec : Specification<TopUpBeneficiary>
{
    public BeneficiaryByUserIdBeneficiaryIdSpec(Guid userId, Guid beneficiaryId)
    {
        Query.Where(x => x.UserId == userId && x.Id == beneficiaryId);
    }
}