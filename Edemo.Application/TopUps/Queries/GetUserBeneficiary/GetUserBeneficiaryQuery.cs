using Ardalis.GuardClauses;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Specs;
using MapsterMapper;
using MediatR;

namespace Edemo.Application.TopUps.Queries.GetUserBeneficiary;

public record GetUserBeneficiaryQuery(Guid BeneficiaryId) : IRequest<BeneficiaryResult>;

public class
    GetUserBeneficiaryQueryHandler(
        ICurrentUser currentUser,
        IReadRepository<TopUpBeneficiary> beneficiaryRepository,
        TopUpAmountProvider topUpAmountProvider)
    : IRequestHandler<GetUserBeneficiaryQuery, BeneficiaryResult>
{
    public async Task<BeneficiaryResult> Handle(GetUserBeneficiaryQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");

        var beneficiary = await beneficiaryRepository.FirstOrDefaultAsync(
            new BeneficiaryByUserIdBeneficiaryIdSpec(currentUser.UserId.Value, request.BeneficiaryId),
            cancellationToken);

        Guard.Against.NotFound(beneficiary, "Beneficiary was not found");

        var totalMonthTopUpAmount = await topUpAmountProvider.GetBeneficiaryTotalMonthlyTopUps(beneficiary.Id);
        return new BeneficiaryResult(beneficiary.Id, beneficiary.Nickname, totalMonthTopUpAmount ?? 0,
            beneficiary.PhoneNumber!);
    }
}