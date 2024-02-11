using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public class UserWithBeneficiarySpec : Specification<User.User>
{
    public UserWithBeneficiarySpec(Guid userId, Guid beneficiaryId)
    {
        Query
            .Where(x => x.Id == userId)
            .Include(x => x.TopUpBeneficiaries
                .Where(a => a.Id == beneficiaryId));
    }
}