using Edemo.Domain.Common;

namespace Edemo.Domain.TopUp.Events;

public class NewTopUpTransactionCreated: DomainEventBase
{
    public TopUpTransaction Transaction { get; }

    public NewTopUpTransactionCreated(TopUpTransaction transaction)
    {
        Transaction = transaction;
    }
}