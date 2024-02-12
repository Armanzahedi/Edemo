using Edemo.Domain.Common.Entity;
using Edemo.Domain.TopUp.ValueObjects;

namespace Edemo.Domain.TopUp;

[Auditable]
public class TopUpBeneficiary : EntityBase<Guid>, IAggregateRoot, ISoftDeletableEntity
{
    private TopUpBeneficiary()
    {
    }

    public Guid UserId { get; private set; }
    public Nickname Nickname { get; private set; }
    public UAEPhoneNumber? PhoneNumber { get; private set; }

    private readonly List<TopUpTransaction> _topUpTransactions = new();
    public IEnumerable<TopUpTransaction> TopUpTransactions => _topUpTransactions.AsReadOnly();

    public static TopUpBeneficiary Create(Guid userId, string nickName, string phoneNumber)
    {
        return new TopUpBeneficiary()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Nickname = Nickname.Create(nickName),
            PhoneNumber = UAEPhoneNumber.Create(phoneNumber)
        };
    }

    public bool IsDeleted { get; set; }
}