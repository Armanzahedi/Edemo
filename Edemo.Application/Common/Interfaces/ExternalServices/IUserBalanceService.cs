using Refit;

namespace Edemo.Application.Common.Interfaces.ExternalServices;

public interface IUserBalanceService
{
    [Get("/balance/{userId}")]
    Task<UserBalance> GetBalanceAsync(Guid userId);
    
    [Post("/balance/{userId}/block")]
    Task<BlockResponse> BlockAsync(Guid userId,[Body] BlockRequest request);
    
    [Post("/balance/{userId}/unblock")]
    Task<UserBalance> ReleaseBlockAsync(Guid userId,[Body] ReleaseBlockRequest request);
    
    [Post("/balance/{userId}/debit")]
    Task<UserBalance> DebitAsync(Guid userId,[Body] DebitRequest request);
}

public record UserBalance(Guid UserId, decimal Balance);
public record BlockRequest(decimal Amount);
public record BlockResponse(Guid BlockId, decimal Amount);
public record DebitRequest(Guid BlockId);
public record ReleaseBlockRequest(Guid BlockId);    