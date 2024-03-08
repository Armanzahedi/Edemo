using Edemo.Api.Common;
using Edemo.Application.Common.Extensions.PaginatedList;
using Edemo.Application.TopUps;
using Edemo.Application.TopUps.Commands.CreateBeneficiary;
using Edemo.Application.TopUps.Commands.DeleteBeneficiary;
using Edemo.Application.TopUps.Commands.TopUpBeneficiary;
using Edemo.Application.TopUps.Queries.GetBeneficiaryTopUpTransactions;
using Edemo.Application.TopUps.Queries.GetUserBeneficiaries;
using Edemo.Application.TopUps.Queries.GetUserBeneficiary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Controllers.V1._0.TopUps;

[Route("api/v1.0/top-ups/beneficiaries")]
[Tags("Top-Up Beneficiaries")]
[Authorize]
public class BeneficiariesController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddBeneficiary([FromBody] CreateBeneficiaryCommand command)
    {
        var result = await Mediator.Send(command);
        
        return result
            .ToActionResult(val => 
             CreatedAtAction("GetUserBeneficiary", new { projectId = val.Id }, val));
    }

    [HttpGet("{BeneficiaryId}")]
    public async Task<ActionResult<BeneficiaryResult>> GetUserBeneficiary([FromRoute] GetUserBeneficiaryQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<BeneficiaryResult>>> GetUserBeneficiaries(
        [FromQuery] GetUserBeneficiariesQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteBeneficiary(Guid beneficiaryId)
    {
        await Mediator.Send(new DeleteBeneficiaryCommand(beneficiaryId));
        return NoContent();
    }

    [HttpGet("{BeneficiaryId}/transactions")]
    public async Task<ActionResult<PaginatedList<TransactionResult>>>
        GetBeneficiaryTopUpTransactions([FromRoute] Guid BeneficiaryId,
            [FromQuery] GetBeneficiaryTopUpTransactionsQuery query)
    {
        query.BeneficiaryId = BeneficiaryId;
        return await Mediator.Send(query);
    }

    [HttpPost("{BeneficiaryId}/transactions")]
    public async Task<ActionResult<TransactionResult>> PerformTopUpTransaction(
        [FromRoute] Guid BeneficiaryId, [FromBody] TopUpBeneficiaryCommand command)
    {
        command.BeneficiaryId = BeneficiaryId;
        return await Mediator.Send(command);
    }
}