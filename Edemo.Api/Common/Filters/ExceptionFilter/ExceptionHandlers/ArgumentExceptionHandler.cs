using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Common.Filters.ExceptionFilter.ExceptionHandlers;

public class ArgumentExceptionHandler : ExceptionHandler<ArgumentException>
{
    protected override IActionResult HandleException(ArgumentException exception)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "BadRequest",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Detail = exception.Message
        };
        
        return new BadRequestObjectResult(details);
    }
}