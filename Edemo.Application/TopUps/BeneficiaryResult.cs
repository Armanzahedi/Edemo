namespace Edemo.Application.TopUps;

public record BeneficiaryResult(Guid Id, string Nickname, decimal TotalMonthlyTopUpAmount, string PhoneNumber);