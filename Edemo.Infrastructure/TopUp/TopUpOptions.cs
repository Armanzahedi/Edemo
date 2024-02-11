using Edemo.Domain.TopUp;

namespace Edemo.Infrastructure.TopUp;

public class TopUpOptions : ITopUpOptions
{
    public const string TopUp = "TopUp";

    public List<decimal> AvailableTopUpAmounts  { get; set; } = new() { 5, 10, 20, 30, 50, 75, 100 };
    public int UnverifiedUserTopUpLimitPerMonthPerBeneficiary { get; set; } = 1000;

    public int VerifiedUserTopUpLimitPerMonthPerBeneficiary  { get; set; } = 500;
    public int UserTotalTopUpMonthlyLimit  { get; set; } = 3000;
    public int TopUpTransactionFee  { get; set; } = 1;
    public int MaxTopUpBeneficiaries { get; set; } = 5;
}