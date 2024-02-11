using Ardalis.GuardClauses;
using Edemo.Application.Common.Extensions.PaginatedList;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Specs;
using MediatR;

namespace Edemo.Application.TopUps.Queries.GetBeneficiaryTopUpTransactions;

public record GetBeneficiaryTopUpTransactionsQuery(
    Guid BeneficiaryId,
    DateTime? FromDate,
    DateTime? ToDate,
    int? PageNumber,
    int? PageSize) : IRequest<PaginatedList<TransactionResult>>
{
    public Guid BeneficiaryId { get; set; } = BeneficiaryId;
}

public class GetBeneficiaryTopUpTransactionsQueryHandler(
    ICurrentUser currentUser,
    IRepository<TopUpTransaction> topUpTransactionRepo) : IRequestHandler<GetBeneficiaryTopUpTransactionsQuery,
    PaginatedList<TransactionResult>>
{
    public async Task<PaginatedList<TransactionResult>> Handle(
        GetBeneficiaryTopUpTransactionsQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");

        return await topUpTransactionRepo
            .PaginatedListAsync<TopUpTransaction, TransactionResult>(
                new TransactionsByBeneficiaryIdUserId(request.BeneficiaryId, currentUser.UserId!.Value,
                    request.FromDate, request.ToDate),
                request.PageNumber,
                request.PageSize,
                cancellationToken);
    }
}