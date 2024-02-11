using Ardalis.GuardClauses;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.TopUp;
using MediatR;

namespace Edemo.Application.TopUps.Commands.DeleteBeneficiary;

public record DeleteBeneficiaryCommand(Guid BeneficiaryId)
    : IRequest;

public class DeleteBeneficiaryCommandHandler(
    ICurrentUser currentUser,
    TopUpService topUpService)
    : IRequestHandler<DeleteBeneficiaryCommand>
{
    public async Task Handle(DeleteBeneficiaryCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(currentUser.UserId, "UserId", "Current User was not found");

       await topUpService
                .DeleteBeneficiary(currentUser.UserId.Value, request.BeneficiaryId);
    }
}