using Ardalis.GuardClauses;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.ExternalServices;
using Edemo.Domain.TopUp.Specs;

namespace Edemo.Domain.TopUp;

public class TopUpService(
    IRepository<User.User> userRepo,
    IRepository<TopUpBeneficiary> beneficiaryRepo,
    IRepository<TopUpTransaction> transactionRepo,
    IDateTimeProvider dateTimeProvider,
    IUserBalanceService userBalanceService,
    ITopUpOptions topUpOptions) : IDomainService
{
    public async Task<TopUpBeneficiary> CreateBeneficiary(Guid userId, string nickName, string phoneNumber)
    {
        var userBeneficiariesCount = await beneficiaryRepo.CountAsync(new BeneficiaryByUserIdSpec(userId));

        Guard.Against.Expression(x => x >= topUpOptions.MaxTopUpBeneficiaries, userBeneficiariesCount,
            $"Cannot add more than {topUpOptions.MaxTopUpBeneficiaries} Beneficiaries.");

        var beneficiary = TopUpBeneficiary.Create(userId, nickName, phoneNumber);

        await beneficiaryRepo.AddAsync(beneficiary);

        return beneficiary;
    }

    public async Task DeleteBeneficiary(Guid userId, Guid beneficiaryId)
    {
        var beneficiary =
            await beneficiaryRepo.FirstOrDefaultAsync(new BeneficiaryByUserIdBeneficiaryIdSpec(userId,beneficiaryId));

        Guard.Against.NotFound(beneficiary, "Beneficiary was not found.");

        await beneficiaryRepo.DeleteAsync(beneficiary);
    }

    public async Task<TopUpTransaction> PerformTopUp(Guid userId, Guid beneficiaryId, decimal topUpAmount)
    {
        Guard.Against.Expression(x => topUpOptions.AvailableTopUpAmounts.Contains(x) == false, topUpAmount,
            "TopUp amount is not in the range of valid TopUp amounts.");

        var user = await userRepo.GetByIdAsync(userId);
        Guard.Against.NotFound(user, "User was not found.");

        var beneficiary =
            await beneficiaryRepo.FirstOrDefaultAsync(new BeneficiaryByUserIdBeneficiaryIdSpec(userId, beneficiaryId));
        Guard.Against.NotFound(beneficiary, "Beneficiary was not found.");


        await ValidateBeneficiaryAllowedTopUp(user, beneficiary, topUpAmount);
        await ValidateUserAllowedTopUp(user, topUpAmount);


        var userBalance = await userBalanceService.GetBalanceAsync(userId);
        Guard.Against.Expression(x => x < topUpAmount, userBalance.Balance, "Insufficient balance.");

        await userBalanceService.DebitBalanceAsync(userId, new DebitRequest(topUpAmount));

        try
        {
            var transaction = TopUpTransaction.Create(userId,
                beneficiaryId,
                topUpAmount,
                dateTimeProvider.UtcNow,
                topUpOptions.TopUpTransactionFee);

            await transactionRepo.AddAsync(transaction);
            return transaction;
        }
        catch (Exception e)
        {
            await userBalanceService.CreditBalanceAsync(userId, new CreditRequest(topUpAmount));
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task ValidateBeneficiaryAllowedTopUp(User.User user, TopUpBeneficiary beneficiary,
        decimal topUpAmount)
    {
        var userAllowedMonthlyTopUpPerBeneficiary = user.IsVerified
            ? topUpOptions.VerifiedUserTopUpLimitPerMonthPerBeneficiary
            : topUpOptions.UnverifiedUserTopUpLimitPerMonthPerBeneficiary;


        var beneficiaryTotalTopUpAmount = (await transactionRepo
            .ListAsync(new TransactionsAmountByBeneficiaryId(beneficiary.Id, dateTimeProvider.UtcNow.Month))).Sum();

        Guard.Against.Expression(x => x + topUpAmount > userAllowedMonthlyTopUpPerBeneficiary,
            beneficiaryTotalTopUpAmount, "User Monthly allowed top-up per beneficiary exceeded.");
    }

    private async Task ValidateUserAllowedTopUp(User.User user, decimal topUpAmount)
    {
        var userTotalTopUpAmount = (await transactionRepo
            .ListAsync(new TransactionsAmountByUserId(user.Id, dateTimeProvider.UtcNow.Month))).Sum();

        Guard.Against.Expression(x => x + topUpAmount > topUpOptions.UserTotalTopUpMonthlyLimit,
            userTotalTopUpAmount,
            "User Monthly allowed top-up exceeded.");
    }
}