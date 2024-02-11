namespace Edemo.Domain.TopUp;

public interface ITopUpOptions
{
    List<decimal> AvailableTopUpAmounts { get; }
    int UnverifiedUserTopUpLimitPerMonthPerBeneficiary { get; }
    int VerifiedUserTopUpLimitPerMonthPerBeneficiary { get; }
    int UserTotalTopUpMonthlyLimit { get; }
    int TopUpTransactionFee { get; }
    int MaxTopUpBeneficiaries { get; }
}