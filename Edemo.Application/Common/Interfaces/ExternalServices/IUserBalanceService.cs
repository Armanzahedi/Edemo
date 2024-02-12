using Refit;

namespace Edemo.Application.Common.Interfaces.ExternalServices;

public interface IUserBalanceService
{
    [Get("/balance/{userId}")]
    Task<UserBalance> GetBalanceAsync(Guid userId);
    
    [Post("/balance/{userId}/debit")]
    Task<DebitResponse> DebitAsync(Guid userId, [Body] DebitRequest request);
    
    [Post("/balance/{userId}/debit/release")]
    Task<UserBalance> ReleaseDebitAsync(Guid userId,[Body] ReleaseDebitRequest request);
    
    [Post("/balance/{userId}/debit/verify")]
    Task<UserBalance> VerifyDebitAsync(Guid userId,[Body] VerifyDebitRequest request);
}

public record UserBalance(Guid UserId, decimal Balance);
public record DebitRequest(decimal Amount);

public record DebitResponse(Guid DebitId, decimal Amount);

public record VerifyDebitRequest(Guid DebitId);

public record ReleaseDebitRequest(Guid DebitId);