using Edemo.Domain.TopUp;
using MediatR;

namespace Edemo.Application.TopUps.Events;

public record TopUpTransactionSucceeded(Guid UserBalanceBlockId, TopUpTransaction Transaction) : INotification;
