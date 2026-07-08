using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Transform;

public static class WarehouseActionResultAssembler
{
    private static int ToStatusCode(WarehouseManagementError error) => error switch
    {
        WarehouseManagementError.WarehouseNotFound => StatusCodes.Status404NotFound,
        WarehouseManagementError.ZoneNotFound => StatusCodes.Status404NotFound,
        WarehouseManagementError.ZoneAreaExceedsWarehouseCapacity => StatusCodes.Status400BadRequest,
        WarehouseManagementError.CompanyNotFound => StatusCodes.Status404NotFound,
        WarehouseManagementError.OperationCancelled => StatusCodes.Status409Conflict,
        WarehouseManagementError.DatabaseError => StatusCodes.Status500InternalServerError,
        WarehouseManagementError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory factory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var statusCode = ToStatusCode((WarehouseManagementError)result.Error!);
        return factory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResult(
        ControllerBase controller,
        Result result,
        ProblemDetailsFactory factory,
        Func<IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction();
        var statusCode = ToStatusCode((WarehouseManagementError)result.Error!);
        return factory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}
