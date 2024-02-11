using Edemo.Api.Common.Filters.ExceptionFilter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Controllers.V1._0;

[ApiController]
[ExceptionFilter]
[Route("api/v1.0/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}