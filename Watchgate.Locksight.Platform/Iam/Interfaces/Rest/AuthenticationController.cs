using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Application.CommandServices;
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
[SwaggerTag("Available Authentication endpoints")]
public class AuthenticationController(
    IUserCommandService userCommandService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Sign in", Description = "Authenticate a user and return a JWT token", OperationId = "SignIn")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was authenticated", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid email or password")]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource,
        CancellationToken cancellationToken)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var result = await userCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResultFromSignInResult(
            this, result, errorLocalizer, problemDetailsFactory,
            userAndToken => Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
                userAndToken.user, userAndToken.token)));
    }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Sign up", Description = "Register a new user account", OperationId = "SignUp")]
    [SwaggerResponse(StatusCodes.Status201Created, "The user was created", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Email already registered")]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource,
        CancellationToken cancellationToken)
    {
        var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        var result = await userCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResultFromSignUpResult(
            this, result, errorLocalizer, problemDetailsFactory,
            userAndToken => Created(string.Empty,
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
                    userAndToken.user, userAndToken.token)));
    }

    [HttpPost("logout")]
    [SwaggerOperation(Summary = "Logout", Description = "Completes the logout flow for JWT clients. The frontend must discard the token.", OperationId = "Logout")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user session was ended on the client side")]
    public IActionResult Logout()
    {
        return Ok(new { message = "User session ended. Remove the JWT token on the client." });
    }
}
