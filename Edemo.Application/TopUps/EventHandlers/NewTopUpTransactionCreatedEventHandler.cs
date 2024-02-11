using Edemo.Domain.TopUp.Events;
using MediatR;

namespace Edemo.Application.TopUps.EventHandlers;

public class NewTopUpTransactionCreatedEventHandler(TopUpAmountProvider topUpAmountProvider) : INotificationHandler<NewTopUpTransactionCreated>
{
    public async Task Handle(NewTopUpTransactionCreated notification, CancellationToken cancellationToken)
    {
        await topUpAmountProvider.Reevaluate(notification.Transaction.UserId, notification.Transaction.BeneficiaryId);
    }
}