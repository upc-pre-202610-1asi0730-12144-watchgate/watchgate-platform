using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

public static class SubscriptionActionResultAssembler
{
    private static int ToStatusCode(SubscriptionManagementError error) => error switch
    {
        SubscriptionManagementError.PlanNotFound => StatusCodes.Status404NotFound,
        SubscriptionManagementError.SubscriptionNotFound => StatusCodes.Status404NotFound,
        SubscriptionManagementError.InvoiceNotFound => StatusCodes.Status404NotFound,
        SubscriptionManagementError.ActiveSubscriptionAlreadyExists => StatusCodes.Status409Conflict,
        SubscriptionManagementError.PaymentFailed => StatusCodes.Status402PaymentRequired,
        SubscriptionManagementError.OperationCancelled => StatusCodes.Status409Conflict,
        SubscriptionManagementError.DatabaseError => StatusCodes.Status500InternalServerError,
        SubscriptionManagementError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResult<T>(ControllerBase controller, Result<T> result, ProblemDetailsFactory factory, Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var statusCode = ToStatusCode((SubscriptionManagementError)result.Error!);
        return factory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}
