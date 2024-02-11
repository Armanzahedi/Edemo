using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public sealed class TransactionsAmountByUserId : Specification<TopUpTransaction,decimal> , ISingleResultSpecification<TopUpTransaction,decimal>
{
    public TransactionsAmountByUserId(Guid userId, DateTime? from = null, DateTime? to = null)
    {
        Query
            .Select(x=>x.Amount)
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.UserId == userId)
            .Where(x => x.TransactionDate >= from, from != null)
            .Where(x => x.TransactionDate <= to, to != null);
        
    }
    public TransactionsAmountByUserId(Guid userId, int month)
    {
        Query
            .Select(x=>x.Amount)
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.UserId == userId)
            .Where(x => x.TransactionDate.Month == month);
    }
}