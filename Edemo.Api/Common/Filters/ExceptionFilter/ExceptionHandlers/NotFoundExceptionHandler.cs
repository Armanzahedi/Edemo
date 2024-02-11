using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Common.Filters.ExceptionFilter.ExceptionHandlers;

public class NotFoundExceptionHandler : ExceptionHandler<Domain.Common.Exceptions.NotFoundException>
{
    protected override IActionResult HandleException(Domain.Common.Exceptions.NotFoundException exception)
    {
        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        return new NotFoundObjectResult(details);
    }
}