using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.Reporting.Domain.Model;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Transform;

public static class ReportingActionResultAssembler
{
    private static int ToStatusCode(ReportingError error) => error switch
    {
        ReportingError.ReportNotFound => StatusCodes.Status404NotFound,
        ReportingError.ScheduledReportNotFound => StatusCodes.Status404NotFound,
        ReportingError.InvalidReportRange => StatusCodes.Status400BadRequest,
        ReportingError.OperationCancelled => StatusCodes.Status409Conflict,
        ReportingError.DatabaseError => StatusCodes.Status500InternalServerError,
        ReportingError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResult<T>(ControllerBase controller, Result<T> result, ProblemDetailsFactory factory, Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var statusCode = ToStatusCode((ReportingError)result.Error!);
        return factory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}