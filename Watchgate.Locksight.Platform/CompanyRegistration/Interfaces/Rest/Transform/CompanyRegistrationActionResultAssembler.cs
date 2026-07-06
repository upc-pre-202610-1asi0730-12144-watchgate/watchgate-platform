using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Transform;

public static class CompanyRegistrationActionResultAssembler
{
    private static int ToStatusCode(CompanyRegistrationError error) => error switch
    {
        CompanyRegistrationError.CompanyAccountNotFound => StatusCodes.Status404NotFound,
        CompanyRegistrationError.CompanyAccountAlreadyExists => StatusCodes.Status409Conflict,
        CompanyRegistrationError.InvalidVerificationCode => StatusCodes.Status400BadRequest,
        CompanyRegistrationError.OperationCancelled => StatusCodes.Status409Conflict,
        CompanyRegistrationError.DatabaseError => StatusCodes.Status500InternalServerError,
        CompanyRegistrationError.InternalServerError => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
    };

    public static IActionResult ToActionResult<T>(ControllerBase controller, Result<T> result, ProblemDetailsFactory factory, Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var statusCode = ToStatusCode((CompanyRegistrationError)result.Error!);
        return factory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}
