using Edemo.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Common.Filters.ExceptionFilter.ExceptionHandlers;

public class ValidationExceptionHandler : ExceptionHandler<ValidationException>
{
    protected override IActionResult HandleException(ValidationException exception)
    {
        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };
        
        return new BadRequestObjectResult(details);
    }
}