using Edemo.Application.Common.Interfaces.ExternalServices;
using Edemo.Application.TopUps.Events;
using MediatR;

namespace Edemo.Application.TopUps.EventHandlers;

public class TopUpTransactionFailedEventHandler(IUserBalanceService userBalanceService) : INotificationHandler<TopUpTransactionFailed>
{
    public async Task Handle(TopUpTransactionFailed notification, CancellationToken cancellationToken)
    {
        await userBalanceService.ReleaseDebitAsync(notification.Transaction.UserId,
            new ReleaseDebitRequest(notification.UserBalanceBlockId));
    }
}