using Edemo.Domain.Common.Entity;
using Edemo.Domain.TopUp.Events;

namespace Edemo.Domain.TopUp;

[Auditable]
public class TopUpTransaction : EntityBase<Guid>, IAggregateRoot
{
    private TopUpTransaction()
    {
    }

    public Guid UserId { get; private set; }
    public Guid BeneficiaryId { get; set; }
    public decimal Amount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public decimal Fee { get; private set; }

    public static TopUpTransaction Create(Guid userId, Guid beneficiaryId, decimal amount, DateTime transactionDate,
        decimal fee)
    {
        var transaction = new TopUpTransaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BeneficiaryId = beneficiaryId,
            Amount = amount,
            TransactionDate = transactionDate,
            Fee = fee
        };
        
        transaction.RegisterDomainEvent(new NewTopUpTransactionCreated(transaction));

        return transaction;
    }
}