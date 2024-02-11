using Edemo.Domain.TopUp;
using MediatR;
using Microsoft.Extensions.Options;

namespace Edemo.Application.TopUps.Queries.GetTopUpOptions;

public record GetTopUpOptionsQuery() : IRequest<GetTopUpOptionsQueryResult>;

public class GetTopUpOptionsQueryHandler(ITopUpOptions topUpOptions)
    : IRequestHandler<GetTopUpOptionsQuery, GetTopUpOptionsQueryResult>
{
    public Task<GetTopUpOptionsQueryResult> Handle(GetTopUpOptionsQuery request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetTopUpOptionsQueryResult(topUpOptions.AvailableTopUpAmounts,
            topUpOptions.UnverifiedUserTopUpLimitPerMonthPerBeneficiary,
            topUpOptions.VerifiedUserTopUpLimitPerMonthPerBeneficiary,
            topUpOptions.UserTotalTopUpMonthlyLimit,
            topUpOptions.MaxTopUpBeneficiaries));
    }
}