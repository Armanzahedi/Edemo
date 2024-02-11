using Ardalis.GuardClauses;
using Edemo.Application.Common.Extensions.PaginatedList;
using Edemo.Application.Common.Interfaces;
using Edemo.Domain.Common;
using Edemo.Domain.Common.Exceptions;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Specs;
using MediatR;

namespace Edemo.Application.TopUps.Queries.GetUserTopUpTransactions;


public record GetUserTopUpTransactionsQuery(
    DateTime? FromDate,
    DateTime? ToDate,
    int? PageNumber,
    int? PageSize) : IRequest<PaginatedList<TransactionResult>>;

public class GetUserTopUpTransactionsQueryHandler(
    ICurrentUser currentUser,
    IRepository<TopUpTransaction> topUpTransactionRepo) : IRequestHandler<GetUserTopUpTransactionsQuery,
    PaginatedList<TransactionResult>>
{
    public async Task<PaginatedList<TransactionResult>> Handle(
        GetUserTopUpTransactionsQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(currentUser.UserId, "Current User was not found");

        return await topUpTransactionRepo
            .PaginatedListAsync<TopUpTransaction, TransactionResult>(
                new TransactionsByUserId(currentUser.UserId!.Value,
                    request.FromDate, request.ToDate),
                request.PageNumber,
                request.PageSize,
                cancellationToken);
    }
}