using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public sealed class TransactionsByBeneficiaryId : Specification<TopUpTransaction>
{
    public TransactionsByBeneficiaryId(Guid beneficiaryId, DateTime? from = null, DateTime? to = null)
    {
        Query
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.BeneficiaryId == beneficiaryId)
            .Where(x => x.TransactionDate >= from, from != null)
            .Where(x => x.TransactionDate <= to, to != null);
        
    }
    public TransactionsByBeneficiaryId(Guid beneficiaryId, int month)
    {
        Query
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.BeneficiaryId == beneficiaryId)
            .Where(x => x.TransactionDate.Month == month);
    }
}