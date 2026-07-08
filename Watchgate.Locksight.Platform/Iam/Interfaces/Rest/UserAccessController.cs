using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Application.CommandServices;
using Watchgate.Locksight.Platform.Iam.Application.QueryServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Transform;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.Extensions;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/user-access")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User & Access Management endpoints.")]
public class UserAccessController(
    IUserAccessCommandService userAccessCommandService,
    IUserAccessQueryService userAccessQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("invitations")]
    [SwaggerOperation(Summary = "Invite new user", Description = "Creates a pending user invitation for a company.", OperationId = "InviteUser")]
    [SwaggerResponse(StatusCodes.Status201Created, "The invitation was created.", typeof(UserInvitationResource))]
    public async Task<IActionResult> InviteUser([FromBody] InviteUserResource resource, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsCurrentUserAdministrator()) return Forbid();
        var companyId = HttpContext.CurrentCompanyId();
        if (companyId is null) return Unauthorized();

        var result = await userAccessCommandService.Handle(
            new InviteUserCommand(companyId.Value, resource.Email, resource.Role,
                NormalizePermissions(resource.Role, resource.Permissions), resource.ZoneId),
            cancellationToken);
        return ToActionResult(result, invitation => CreatedAtAction(nameof(GetInvitationsByCompanyId),
            new { companyId = invitation.CompanyId },
            UserAccessResourceFromEntityAssembler.ToResourceFromInvitation(invitation)));
    }

    [HttpPost("team-users")]
    [SwaggerOperation(Summary = "Create team user", Description = "Creates an operational team user inside the current company and assigns access profile data.", OperationId = "CreateTeamUser")]
    [SwaggerResponse(StatusCodes.Status201Created, "The team user access profile was created.", typeof(UserAccessProfileResource))]
    public async Task<IActionResult> CreateTeamUser([FromBody] CreateTeamUserResource resource, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsCurrentUserAdministrator()) return Forbid();
        var companyId = HttpContext.CurrentCompanyId();
        if (companyId is null) return Unauthorized();

        var result = await userAccessCommandService.Handle(
            new CreateTeamUserCommand(companyId.Value, resource.FullName, resource.Email, resource.Password,
                resource.Role, NormalizePermissions(resource.Role, resource.Permissions), resource.ZoneId),
            cancellationToken);
        return ToActionResult(result, profile => CreatedAtAction(nameof(GetUserAccessProfile),
            new { userId = profile.UserId },
            UserAccessResourceFromEntityAssembler.ToResourceFromProfile(profile)));
    }

    [AllowAnonymous]
    [HttpPatch("invitations/accept")]
    [SwaggerOperation(Summary = "Accept invitation", Description = "Accepts a pending user invitation token.", OperationId = "AcceptInvitation")]
    [SwaggerResponse(StatusCodes.Status200OK, "The invitation was accepted.", typeof(UserInvitationResource))]
    public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationResource resource, CancellationToken cancellationToken)
    {
        var result = await userAccessCommandService.Handle(new AcceptInvitationCommand(resource.Token), cancellationToken);
        return ToActionResult(result, invitation => Ok(UserAccessResourceFromEntityAssembler.ToResourceFromInvitation(invitation)));
    }

    [HttpGet("invitations/company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get company invitations", Description = "Lists user invitations for a company.", OperationId = "GetUserInvitationsByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The invitations were retrieved.", typeof(IEnumerable<UserInvitationResource>))]
    public async Task<IActionResult> GetInvitationsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var currentCompanyId = HttpContext.CurrentCompanyId();
        if (currentCompanyId is null) return Unauthorized();
        if (companyId != currentCompanyId.Value) return Forbid();

        var result = await userAccessQueryService.Handle(new GetInvitationsByCompanyIdQuery(currentCompanyId.Value), cancellationToken);
        return ToActionResult(result, invitations => Ok(invitations.Select(UserAccessResourceFromEntityAssembler.ToResourceFromInvitation)));
    }

    [HttpPost("users/{userId:int}/roles")]
    [SwaggerOperation(Summary = "Assign role and permissions", Description = "Assigns role and permissions to a user.", OperationId = "AssignUserRoleAndPermissions")]
    [SwaggerResponse(StatusCodes.Status200OK, "The access profile was updated.", typeof(UserAccessProfileResource))]
    public async Task<IActionResult> AssignRoleAndPermissions(int userId, [FromBody] AssignUserAccessResource resource, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsCurrentUserAdministrator()) return Forbid();
        var companyId = HttpContext.CurrentCompanyId();
        if (companyId is null) return Unauthorized();

        var result = await userAccessCommandService.Handle(
            new AssignUserAccessCommand(userId, companyId.Value, resource.Role,
                NormalizePermissions(resource.Role, resource.Permissions)),
            cancellationToken);
        return ToActionResult(result, profile => Ok(UserAccessResourceFromEntityAssembler.ToResourceFromProfile(profile)));
    }

    [HttpPatch("users/{userId:int}/zone-restriction")]
    [SwaggerOperation(Summary = "Restrict user zone access", Description = "Restricts user access to a specific warehouse zone.", OperationId = "RestrictUserZoneAccess")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user zone restriction was updated.", typeof(UserAccessProfileResource))]
    public async Task<IActionResult> RestrictZoneAccess(int userId, [FromBody] RestrictUserZoneAccessResource resource, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsCurrentUserAdministrator()) return Forbid();
        if (!await CanAccessUserProfile(userId, cancellationToken)) return Forbid();

        var result = await userAccessCommandService.Handle(new RestrictUserZoneAccessCommand(userId, resource.ZoneId), cancellationToken);
        return ToActionResult(result, profile => Ok(UserAccessResourceFromEntityAssembler.ToResourceFromProfile(profile)));
    }

    [HttpPatch("users/{userId:int}/revoke")]
    [SwaggerOperation(Summary = "Revoke user access", Description = "Revokes platform access for a user profile.", OperationId = "RevokeUserAccess")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user access was revoked.", typeof(UserAccessProfileResource))]
    public async Task<IActionResult> RevokeAccess(int userId, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsCurrentUserAdministrator()) return Forbid();
        if (!await CanAccessUserProfile(userId, cancellationToken)) return Forbid();

        var result = await userAccessCommandService.Handle(new RevokeUserAccessCommand(userId), cancellationToken);
        return ToActionResult(result, profile => Ok(UserAccessResourceFromEntityAssembler.ToResourceFromProfile(profile)));
    }

    [HttpPatch("users/{userId:int}/notification-preferences")]
    [SwaggerOperation(Summary = "Update notification preferences", Description = "Updates email/push and critical-only notification preferences.", OperationId = "UpdateNotificationPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The notification preferences were updated.", typeof(UserAccessProfileResource))]
    public async Task<IActionResult> UpdateNotificationPreferences(int userId, [FromBody] UpdateNotificationPreferencesResource resource, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.CurrentUserId();
        if (!HttpContext.IsCurrentUserAdministrator() && currentUserId != userId) return Forbid();
        if (!await CanAccessUserProfile(userId, cancellationToken)) return Forbid();

        var result = await userAccessCommandService.Handle(
            new UpdateNotificationPreferencesCommand(userId, resource.EmailEnabled, resource.PushEnabled, resource.CriticalOnly),
            cancellationToken);
        return ToActionResult(result, profile => Ok(UserAccessResourceFromEntityAssembler.ToResourceFromProfile(profile)));
    }

    [HttpGet("users/{userId:int}")]
    [SwaggerOperation(Summary = "Get user access profile", Description = "Gets access profile, role, permissions and preferences for a user.", OperationId = "GetUserAccessProfile")]
    [SwaggerResponse(StatusCodes.Status200OK, "The access profile was retrieved.", typeof(UserAccessProfileResource))]
    public async Task<IActionResult> GetUserAccessProfile(int userId, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.CurrentUserId();
        if (!HttpContext.IsCurrentUserAdministrator() && currentUserId != userId) return Forbid();

        var result = await userAccessQueryService.Handle(new GetUserAccessProfileByUserIdQuery(userId), cancellationToken);
        return ToActionResult(result, profile =>
        {
            if (profile.CompanyId != HttpContext.CurrentCompanyId()) return Forbid();
            return Ok(UserAccessResourceFromEntityAssembler.ToResourceFromProfile(profile));
        });
    }

    [HttpGet("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get company access profiles", Description = "Lists access profiles for a company.", OperationId = "GetUserAccessProfilesByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The access profiles were retrieved.", typeof(IEnumerable<UserAccessProfileResource>))]
    public async Task<IActionResult> GetUserAccessProfilesByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var currentCompanyId = HttpContext.CurrentCompanyId();
        if (currentCompanyId is null) return Unauthorized();
        if (companyId != currentCompanyId.Value) return Forbid();

        var result = await userAccessQueryService.Handle(new GetUserAccessProfilesByCompanyIdQuery(currentCompanyId.Value), cancellationToken);
        return ToActionResult(result, profiles => Ok(profiles.Select(UserAccessResourceFromEntityAssembler.ToResourceFromProfile)));
    }

    private async Task<bool> CanAccessUserProfile(int userId, CancellationToken cancellationToken)
    {
        var currentCompanyId = HttpContext.CurrentCompanyId();
        if (currentCompanyId is null) return false;

        var result = await userAccessQueryService.Handle(new GetUserAccessProfileByUserIdQuery(userId), cancellationToken);
        return result.IsSuccess && result.Value!.CompanyId == currentCompanyId.Value;
    }

    private static string NormalizePermissions(string role, string permissions) =>
        string.Equals(role, "Administrator", StringComparison.OrdinalIgnoreCase)
            ? "WAREHOUSES_MANAGE,SENSORS_MANAGE,ALERTS_MANAGE,REPORTS_VIEW,BILLING_MANAGE,TEAM_MANAGE"
            : permissions;

    private IActionResult ToActionResult<T>(Result<T> result, Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        var statusCode = result.Error is IamError.UserNotFound ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;
        return problemDetailsFactory.CreateProblemDetails(this, statusCode, result.Error, result.Message);
    }
}
