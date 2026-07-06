using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Application.QueryServices;
using Watchgate.Locksight.Platform.Iam.Application.CommandServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Transform;
using Watchgate.Locksight.Platform.Resources.Errors;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;


namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Users endpoints")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all users", OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "Users retrieved", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await userQueryService.Handle(new GetAllUsersQuery(), cancellationToken);
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get user by ID", OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "User retrieved", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(id), cancellationToken);
        return IamActionResultAssembler.ToActionResultFromGetUserByIdResult(
            this, user, errorLocalizer, problemDetailsFactory,
            foundUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(foundUser)));
    }

    [HttpPatch("{id:int}/password")]
    [SwaggerOperation(Summary = "Reset password", Description = "Updates a user password using the configured hashing service.", OperationId = "ResetPassword")]
    [SwaggerResponse(StatusCodes.Status200OK, "Password reset completed", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordResource resource, CancellationToken cancellationToken)
    {
        var result = await userCommandService.Handle(new ResetPasswordCommand(id, resource.NewPassword), cancellationToken);
        if (result.IsSuccess)
            return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));

        var statusCode = result.Error is IamError.UserNotFound ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;
        return problemDetailsFactory.CreateProblemDetails(this, statusCode, result.Error, result.Message);
    }
}
