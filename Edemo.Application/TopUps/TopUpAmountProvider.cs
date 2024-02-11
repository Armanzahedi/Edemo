using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Specs;

namespace Edemo.Application.TopUps;

public class TopUpAmountProvider(IDateTimeProvider dateTimeProvider, IReadRepository<TopUpTransaction> transactionRepo,ICacheService cacheService)
{
    public Task<decimal?> GetUserTotalMonthlyTopUps(Guid userId)
    {
        return cacheService.GetOrCreateAsync<decimal?>(userId.ToString(), async () => (await
            transactionRepo.ListAsync(new TransactionsAmountByUserId(userId,dateTimeProvider.UtcNow.Month))).Sum());
    }
    public Task<decimal?> GetBeneficiaryTotalMonthlyTopUps(Guid beneficiaryId)
    {
        return cacheService.GetOrCreateAsync<decimal?>(beneficiaryId.ToString(), async () => (await
            transactionRepo.ListAsync(new TransactionsAmountByBeneficiaryId(beneficiaryId,dateTimeProvider.UtcNow.Month))).Sum());
    }

    public async Task Reevaluate(Guid userId, Guid beneficiaryId)
    {
        cacheService.Remove(userId.ToString());
        cacheService.Remove(beneficiaryId.ToString());

        await GetUserTotalMonthlyTopUps(userId);
        await GetUserTotalMonthlyTopUps(beneficiaryId);
    }
}