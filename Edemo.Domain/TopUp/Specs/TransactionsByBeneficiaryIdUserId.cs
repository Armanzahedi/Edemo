using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public sealed class TransactionsByBeneficiaryIdUserId : Specification<TopUpTransaction>
{
    public TransactionsByBeneficiaryIdUserId(Guid beneficiaryId, Guid userId, DateTime? from = null, DateTime? to = null)
    {
        Query
            .OrderByDescending(x=>x.TransactionDate)
            .Where(x => x.BeneficiaryId == beneficiaryId && x.UserId == userId)
            .Where(x => x.TransactionDate >= from, from != null)
            .Where(x => x.TransactionDate <= to, to != null);
    }
    
    public TransactionsByBeneficiaryIdUserId(Guid beneficiaryId, Guid userId, int month)
    {
        Query
            .OrderByDescending(x=>x.TransactionDate)
            .Where(x => x.BeneficiaryId == beneficiaryId && x.UserId == userId)
            .Where(x => x.TransactionDate.Month == month);
       
    }
}