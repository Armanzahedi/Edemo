using Ardalis.GuardClauses;
using Edemo.Application.Common.Extensions.PaginatedList;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Specs;
using MediatR;

namespace Edemo.Application.TopUps.Queries.GetUserBeneficiaries;

public record GetUserBeneficiariesQuery(int? PageNumber, int? PageSize) : IRequest<PaginatedList<BeneficiaryResult>>;

public class
    GetUserBeneficiariesQueryHandler(
        ICurrentUser currentUser,
        IReadRepository<TopUpBeneficiary> beneficiaryRepository,
        TopUpAmountProvider topUpAmountProvider)
    : IRequestHandler<GetUserBeneficiariesQuery, PaginatedList<BeneficiaryResult>>
{
    public async Task<PaginatedList<BeneficiaryResult>> Handle(GetUserBeneficiariesQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");

        return await beneficiaryRepository
            .PaginatedListAsync<TopUpBeneficiary, BeneficiaryResult>(
                new BeneficiaryByUserIdSpec(currentUser.UserId!.Value),
                async (x) =>
                {
                    var totalMonthTopUpAmount = await topUpAmountProvider.GetBeneficiaryTotalMonthlyTopUps(x.Id);
                    return new BeneficiaryResult(x.Id, x.Nickname, totalMonthTopUpAmount ?? 0, x.PhoneNumber!);
                },
                request.PageNumber,
                request.PageSize,
                cancellationToken);
    }
}