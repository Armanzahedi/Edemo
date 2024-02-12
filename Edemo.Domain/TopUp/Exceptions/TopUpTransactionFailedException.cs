namespace Edemo.Domain.TopUp.Exceptions;

public class TopUpTransactionFailedException : Exception
{
    public readonly TopUpTransaction Transaction;

    public TopUpTransactionFailedException(TopUpTransaction transaction) : base("Top-Up Transaction Failed")
    {
        Transaction = transaction;
    }
    public TopUpTransactionFailedException(TopUpTransaction transaction,Exception innerException)
        : base("Top-Up Transaction Failed", innerException)
    {
        Transaction = transaction;
    }
}