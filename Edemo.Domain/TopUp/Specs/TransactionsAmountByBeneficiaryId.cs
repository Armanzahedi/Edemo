using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public sealed class TransactionsAmountByBeneficiaryId : Specification<TopUpTransaction,decimal> , ISingleResultSpecification<TopUpTransaction,decimal>
{
    public TransactionsAmountByBeneficiaryId(Guid beneficiaryId, DateTime? from = null, DateTime? to = null)
    {
        Query
            .Select(x=>x.Amount)
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.BeneficiaryId == beneficiaryId)
            .Where(x => x.TransactionDate >= from, from != null)
            .Where(x => x.TransactionDate <= to, to != null);
        
    }
    public TransactionsAmountByBeneficiaryId(Guid beneficiaryId, int month)
    {
        Query
            .Select(x=>x.Amount)
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.BeneficiaryId == beneficiaryId)
            .Where(x => x.TransactionDate.Month == month);
    }
}