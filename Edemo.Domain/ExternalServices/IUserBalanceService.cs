using Refit;

namespace Edemo.Domain.ExternalServices;

public interface IUserBalanceService
{
    [Get("/balance/{userId}")]
    Task<UserBalance> GetBalanceAsync(Guid userId);
    
    [Post("/balance/{userId}/debit")]
    Task<UserBalance> DebitBalanceAsync(Guid userId,[Body] DebitRequest request);
    
    [Post("/balance/{userId}/credit")]
    Task<UserBalance> CreditBalanceAsync(Guid userId,[Body] CreditRequest request);
}

public record UserBalance(Guid UserId, decimal Balance);
public record DebitRequest(decimal Amount);
public record CreditRequest(decimal Amount);