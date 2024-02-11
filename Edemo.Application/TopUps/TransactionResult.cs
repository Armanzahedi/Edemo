namespace Edemo.Application.TopUps;

public record TransactionResult(Guid Id, decimal Amount, DateTime TransactionDate,Guid BeneficiaryId, decimal Fee);