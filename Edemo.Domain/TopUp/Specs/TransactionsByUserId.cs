using Ardalis.Specification;

namespace Edemo.Domain.TopUp.Specs;

public sealed class TransactionsByUserId : Specification<TopUpTransaction>
{
    public TransactionsByUserId(Guid userId, DateTime? from = null, DateTime? to = null)
    {
        Query
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.UserId == userId)
            .Where(x => x.TransactionDate >= from, from != null)
            .Where(x => x.TransactionDate <= to, to != null);
    }
    public TransactionsByUserId(Guid userId, int? month = null)
    {
        Query
            .OrderByDescending(x => x.TransactionDate)
            .Where(x => x.UserId == userId)
            .Where(x => x.TransactionDate.Month == month);

    }
}