using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Transform;

public static class SensorActionResultAssembler
{
    private static int ToStatusCode(SensorIntegrationError error) => error switch
    {
        SensorIntegrationError.SensorNotFound => StatusCodes.Status404NotFound,
        SensorIntegrationError.WarehouseZoneNotFound => StatusCodes.Status404NotFound,
        SensorIntegrationError.CompanyNotFound => StatusCodes.Status404NotFound,
        SensorIntegrationError.SensorAlreadyExists => StatusCodes.Status409Conflict,
        SensorIntegrationError.InvalidSensorData => StatusCodes.Status400BadRequest,
        SensorIntegrationError.OperationCancelled => StatusCodes.Status409Conflict,
        SensorIntegrationError.DatabaseError => StatusCodes.Status500InternalServerError,
        SensorIntegrationError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory factory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var statusCode = ToStatusCode((SensorIntegrationError)result.Error!);
        return factory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}