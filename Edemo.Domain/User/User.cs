using Edemo.Domain.Common.Entity;
using Edemo.Domain.TopUp;
using Microsoft.AspNetCore.Identity;

namespace Edemo.Domain.User;

[Auditable]
public class User : IdentityUser<Guid>, IAggregateRoot
{
    public bool IsVerified { get; set; }

    private readonly List<TopUpBeneficiary> _topUpBeneficiaries = new(); 
    public IEnumerable<TopUpBeneficiary> TopUpBeneficiaries => _topUpBeneficiaries.AsReadOnly();

    private readonly List<TopUpTransaction> _topUpTransactions = new(); 
    public IEnumerable<TopUpTransaction> TopUpTransactions => _topUpTransactions.AsReadOnly();
}