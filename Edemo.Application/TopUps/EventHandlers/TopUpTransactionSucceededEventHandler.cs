using Edemo.Application.Common.Interfaces.ExternalServices;
using Edemo.Application.TopUps.Events;
using MediatR;

namespace Edemo.Application.TopUps.EventHandlers;

public class TopUpTransactionSucceededEventHandler(IUserBalanceService userBalanceService) : INotificationHandler<TopUpTransactionSucceeded>
{
    public async Task Handle(TopUpTransactionSucceeded notification, CancellationToken cancellationToken)
    {
        await userBalanceService.VerifyDebitAsync(notification.Transaction.UserId,
            new VerifyDebitRequest(notification.DebitId));
    }
}