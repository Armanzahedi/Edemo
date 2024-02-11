using Edemo.Application.TopUps.Queries.GetTopUpOptions;
using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Controllers.V1._0.TopUps;

[Route("api/v1.0/top-ups/options")]
[Tags("Top-Up Options")]
public class OptionsController() : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetTopUpOptionsQueryResult>> GetTopUpOptions()
    {
        return await Mediator.Send(new GetTopUpOptionsQuery());
    }
}