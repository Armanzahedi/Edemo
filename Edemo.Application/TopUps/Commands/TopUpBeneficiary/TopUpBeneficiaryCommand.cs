using Ardalis.GuardClauses;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp;
using MapsterMapper;
using MediatR;

namespace Edemo.Application.TopUps.Commands.TopUpBeneficiary;

public record TopUpBeneficiaryCommand(Guid BeneficiaryId, decimal Amount) : IRequest<TransactionResult>
{
    public Guid BeneficiaryId { get; set; } = BeneficiaryId;
}

public class TopUpBeneficiaryCommandHandler(ICurrentUser currentUser,TopUpService topUpService, IMapper mapper) : IRequestHandler<TopUpBeneficiaryCommand, TransactionResult>
{
    public async Task<TransactionResult> Handle(TopUpBeneficiaryCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");

        var trx = await topUpService.PerformTopUp(currentUser.UserId.Value!, request.BeneficiaryId, request.Amount);

        return mapper.Map<TransactionResult>(trx);
    }
}