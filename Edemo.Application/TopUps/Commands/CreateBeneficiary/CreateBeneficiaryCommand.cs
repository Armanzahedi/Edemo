using Ardalis.GuardClauses;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp;
using MapsterMapper;
using MediatR;

namespace Edemo.Application.TopUps.Commands.CreateBeneficiary;

public record CreateBeneficiaryCommand(string Nickname, string PhoneNumber) : IRequest<BeneficiaryResult>;

public class CreateBeneficiaryCommandHandler(
    ICurrentUser currentUser,
    IMapper mapper,
    TopUpService topUpService)
    : IRequestHandler<CreateBeneficiaryCommand, BeneficiaryResult>
{
    public async Task<BeneficiaryResult> Handle(CreateBeneficiaryCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");

        
        var beneficiary =
            await topUpService.CreateBeneficiary(currentUser.UserId.Value, request.Nickname,
                request.PhoneNumber);
        
        return mapper.Map<BeneficiaryResult>(beneficiary);
    }
}