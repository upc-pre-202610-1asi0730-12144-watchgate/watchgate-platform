using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Transform;

public static class SecurityAlertActionResultAssembler
{
    private static int ToStatusCode(SecurityAlertsError error) => error switch
    {
        SecurityAlertsError.AlertNotFound => StatusCodes.Status404NotFound,
        SecurityAlertsError.IncidentNotFound => StatusCodes.Status404NotFound,
        SecurityAlertsError.SensorNotFound => StatusCodes.Status404NotFound,
        SecurityAlertsError.CompanyNotFound => StatusCodes.Status404NotFound,
        SecurityAlertsError.InvalidAlertData => StatusCodes.Status400BadRequest,
        SecurityAlertsError.OperationCancelled => StatusCodes.Status409Conflict,
        SecurityAlertsError.DatabaseError => StatusCodes.Status500InternalServerError,
        SecurityAlertsError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory factory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var statusCode = ToStatusCode((SecurityAlertsError)result.Error!);
        return factory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}