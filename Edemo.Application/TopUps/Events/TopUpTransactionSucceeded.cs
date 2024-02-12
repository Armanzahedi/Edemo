using Edemo.Domain.TopUp;
using MediatR;

namespace Edemo.Application.TopUps.Events;

public record TopUpTransactionSucceeded(Guid DebitId, TopUpTransaction Transaction) : INotification;
