using Edemo.Domain.TopUp;
using MediatR;

namespace Edemo.Application.TopUps.Events;

public record TopUpTransactionFailed(Guid UserBalanceBlockId, TopUpTransaction Transaction) : INotification;