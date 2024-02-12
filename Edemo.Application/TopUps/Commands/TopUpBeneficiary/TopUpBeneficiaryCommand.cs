using Ardalis.GuardClauses;
using Edemo.Application.Common.Interfaces;
using Edemo.Application.Common.Interfaces.ExternalServices;
using Edemo.Application.TopUps.Events;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Exceptions;
using Edemo.Domain.TopUp.Specs;
using Edemo.Domain.User;
using MapsterMapper;
using MediatR;

namespace Edemo.Application.TopUps.Commands.TopUpBeneficiary;

public record TopUpBeneficiaryCommand(Guid BeneficiaryId, decimal Amount) : IRequest<TransactionResult>
{
    public Guid BeneficiaryId { get; set; } = BeneficiaryId;
}

public class TopUpBeneficiaryCommandHandler(
    ICurrentUser currentUser,
    TopUpService topUpService,
    IRepository<User> userRepo,
    IRepository<Domain.TopUp.TopUpBeneficiary> beneficiaryRepo,
    IUserBalanceService userBalanceService,
    IPublisher publisher,
    IMapper mapper) : IRequestHandler<TopUpBeneficiaryCommand, TransactionResult>
{
    public async Task<TransactionResult> Handle(TopUpBeneficiaryCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");


        var user = await userRepo.GetByIdAsync(currentUser.UserId.Value, cancellationToken);
        Guard.Against.NotFound(user, "User was not found.");

        var beneficiary =
            await beneficiaryRepo.FirstOrDefaultAsync(
                new BeneficiaryByUserIdBeneficiaryIdSpec(currentUser.UserId.Value, request.BeneficiaryId),
                cancellationToken);
        
        Guard.Against.NotFound(beneficiary, "Beneficiary was not found.");

        await topUpService.ValidateTopUpRequest(user, beneficiary, request.Amount);

        var userBalance = await userBalanceService.GetBalanceAsync(currentUser.UserId.Value);
        Guard.Against.Expression(x => x < request.Amount, userBalance.Balance, "Insufficient balance.");

        
        var debitResponse = await userBalanceService.DebitAsync(user.Id, new DebitRequest(request.Amount));
        try
        {
            var trx = await topUpService.PerformTopUp(user, beneficiary, request.Amount);
            await publisher.Publish(new TopUpTransactionSucceeded(debitResponse.DebitId,trx), cancellationToken);

            return mapper.Map<TransactionResult>(trx);
        }
        catch (TopUpTransactionFailedException e)
        {
            await publisher.Publish(new TopUpTransactionFailed(debitResponse.DebitId,e.Transaction), cancellationToken);
            throw;
        }

    }
}