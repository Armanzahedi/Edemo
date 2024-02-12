using Ardalis.GuardClauses;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp.Exceptions;
using Edemo.Domain.TopUp.Specs;

namespace Edemo.Domain.TopUp;

public class TopUpService : IDomainService
{
    public TopUpService()
    {
    }
    private bool _topUpRequestIsValidated;
    private readonly IRepository<TopUpBeneficiary> _beneficiaryRepo;
    private readonly IRepository<TopUpTransaction> _transactionRepo;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITopUpOptions _topUpOptions;

    public TopUpService(IRepository<TopUpBeneficiary> beneficiaryRepo,
        IRepository<TopUpTransaction> transactionRepo,
        IDateTimeProvider dateTimeProvider,
        ITopUpOptions topUpOptions)
    {
        _beneficiaryRepo = beneficiaryRepo;
        _transactionRepo = transactionRepo;
        _dateTimeProvider = dateTimeProvider;
        _topUpOptions = topUpOptions;
    }

    public async Task<TopUpBeneficiary> CreateBeneficiary(Guid userId, string nickName, string phoneNumber)
    {
        var userBeneficiariesCount = await _beneficiaryRepo.CountAsync(new BeneficiaryByUserIdSpec(userId));

        Guard.Against.Expression(x => x >= _topUpOptions.MaxTopUpBeneficiaries, userBeneficiariesCount,
            $"Cannot add more than {_topUpOptions.MaxTopUpBeneficiaries} Beneficiaries.");

        var beneficiary = TopUpBeneficiary.Create(userId, nickName, phoneNumber);

        await _beneficiaryRepo.AddAsync(beneficiary);

        return beneficiary;
    }

    public async Task DeleteBeneficiary(Guid userId, Guid beneficiaryId)
    {
        var beneficiary =
            await _beneficiaryRepo.FirstOrDefaultAsync(new BeneficiaryByUserIdBeneficiaryIdSpec(userId,beneficiaryId));

        Guard.Against.NotFound(beneficiary, "Beneficiary was not found.");

        await _beneficiaryRepo.DeleteAsync(beneficiary);
    }

    public async Task<TopUpTransaction> PerformTopUp(User.User user, TopUpBeneficiary beneficiary, decimal topUpAmount)
    {

        if (_topUpRequestIsValidated == false)
        {
            await ValidateBeneficiaryAllowedTopUp(user, beneficiary, topUpAmount);
            await ValidateUserAllowedTopUp(user, topUpAmount);
        }

        var transaction = TopUpTransaction.Create(user.Id,
            beneficiary.Id,
            topUpAmount,
            _dateTimeProvider.UtcNow,
            _topUpOptions.TopUpTransactionFee);
        
        try
        {
            await _transactionRepo.AddAsync(transaction);
            return transaction;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new TopUpTransactionFailedException(transaction,e);
        }
    }

    public async Task ValidateTopUpRequest(User.User user, TopUpBeneficiary beneficiary, decimal topUpAmount)
    {
        await ValidateBeneficiaryAllowedTopUp(user, beneficiary, topUpAmount);
        await ValidateUserAllowedTopUp(user, topUpAmount);
        _topUpRequestIsValidated = true;
    }
    private async Task ValidateBeneficiaryAllowedTopUp(User.User user, TopUpBeneficiary beneficiary,
        decimal topUpAmount)
    {
        var userAllowedMonthlyTopUpPerBeneficiary = user.IsVerified
            ? _topUpOptions.VerifiedUserTopUpLimitPerMonthPerBeneficiary
            : _topUpOptions.UnverifiedUserTopUpLimitPerMonthPerBeneficiary;


        var beneficiaryTotalTopUpAmount = (await _transactionRepo
            .ListAsync(new TransactionsAmountByBeneficiaryId(beneficiary.Id, _dateTimeProvider.UtcNow.Month))).Sum();

        Guard.Against.Expression(x => x + topUpAmount > userAllowedMonthlyTopUpPerBeneficiary,
            beneficiaryTotalTopUpAmount, "User Monthly allowed top-up per beneficiary exceeded.");
    }

    private async Task ValidateUserAllowedTopUp(User.User user, decimal topUpAmount)
    {
        var userTotalTopUpAmount = (await _transactionRepo
            .ListAsync(new TransactionsAmountByUserId(user.Id, _dateTimeProvider.UtcNow.Month))).Sum();

        Guard.Against.Expression(x => x + topUpAmount > _topUpOptions.UserTotalTopUpMonthlyLimit,
            userTotalTopUpAmount,
            "User Monthly allowed top-up exceeded.");
    }
}