using Edemo.Domain.TopUp;
using MediatR;

namespace Edemo.Application.TopUps.Events;

public record TopUpTransactionFailed(Guid DebitId, TopUpTransaction Transaction) : INotification;