using Edemo.Application.Common.Extensions.PaginatedList;
using Edemo.Application.TopUps;
using Edemo.Application.TopUps.Commands.TopUpBeneficiary;
using Edemo.Application.TopUps.Queries.GetUserTopUpTransactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Controllers.V1._0.TopUps;

[Route("api/v1.0/top-ups/transactions")]
[Tags("Top-Up Transactions")]
[Authorize]
public class TransactionsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<TransactionResult>>>
        GetUserTopUpTransactions([FromQuery] GetUserTopUpTransactionsQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<ActionResult<TransactionResult>> PerformTopUpTransaction([FromBody] TopUpBeneficiaryCommand command)
    {
        return await Mediator.Send(command);
    }
}