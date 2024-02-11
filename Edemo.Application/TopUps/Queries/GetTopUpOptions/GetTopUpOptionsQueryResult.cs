namespace Edemo.Application.TopUps.Queries.GetTopUpOptions;

public record GetTopUpOptionsQueryResult(
    List<decimal> AvailableAmounts,
    decimal UnverifiedUserTopUpLimitPerMonthPerBeneficiary,
    decimal VerifiedUserTopUpLimitPerMonthPerBeneficiary,
    decimal UserTotalTopUpMonthlyLimit,
    int MaxTopUpBeneficiaries);