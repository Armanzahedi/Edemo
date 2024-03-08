using Ardalis.GuardClauses;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Errors;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.Common.Result;
using Edemo.Domain.TopUp;
using MapsterMapper;
using MediatR;
using NotFoundException = Edemo.Domain.Common.Exceptions.NotFoundException;

namespace Edemo.Application.TopUps.Commands.CreateBeneficiary;

public record CreateBeneficiaryCommand(string Nickname, string PhoneNumber) : IRequest<Result<BeneficiaryResult>>;

public class CreateBeneficiaryCommandHandler(
    ICurrentUser currentUser,
    IMapper mapper,
    TopUpService topUpService)
    : IRequestHandler<CreateBeneficiaryCommand, Result<BeneficiaryResult>>
{
    public async Task<Result<BeneficiaryResult>> Handle(CreateBeneficiaryCommand request,
        CancellationToken cancellationToken)
    {
        // return Error.OfType<NotFoundError>();

        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");


        var beneficiary =
            await topUpService.CreateBeneficiary(currentUser.UserId.Value, request.Nickname,
                request.PhoneNumber);

        return mapper.Map<BeneficiaryResult>(beneficiary);
    }
}