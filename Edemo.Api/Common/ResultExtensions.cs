using Edemo.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Edemo.Api.Common;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<TValue>(this Result<TValue> result, Func<TValue, ObjectResult> resolveFunc)
    {
        return result.Match<ActionResult>(
            resolveFunc,
            e => e.ToObjectResult());
    }
    
    private static ActionResult ToObjectResult(this Error error)
    {
        var problemDetails = error.ToProblemDetails();
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails?.Status
        };
    }

    private static ProblemDetails ToProblemDetails(this Error error)
    {
        return new ProblemDetails
        {
            Type = error.Type.ToString(),
            Title = error.Code,
            Status = error.Type.ToStatusCode(),
            Detail = error.Description
        };
    }

    private static int ToStatusCode(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unexpected => StatusCodes.Status400BadRequest,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}