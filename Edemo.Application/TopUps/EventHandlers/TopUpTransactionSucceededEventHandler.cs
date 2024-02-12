using Edemo.Application.Common.Interfaces.ExternalServices;
using Edemo.Application.TopUps.Events;
using MediatR;

namespace Edemo.Application.TopUps.EventHandlers;

public class TopUpTransactionSucceededEventHandler(IUserBalanceService userBalanceService) : INotificationHandler<TopUpTransactionSucceeded>
{
    public async Task Handle(TopUpTransactionSucceeded notification, CancellationToken cancellationToken)
    {
        await userBalanceService.DebitAsync(notification.Transaction.UserId,
            new DebitRequest(notification.UserBalanceBlockId));
    }
}