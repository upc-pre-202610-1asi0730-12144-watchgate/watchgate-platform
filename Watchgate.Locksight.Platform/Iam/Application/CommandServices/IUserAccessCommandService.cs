using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.Iam.Application.CommandServices;

public interface IUserAccessCommandService
{
    Task<Result<UserAccessProfile>> Handle(CreateTeamUserCommand command, CancellationToken cancellationToken = default);
    Task<Result<UserInvitation>> Handle(InviteUserCommand command, CancellationToken cancellationToken = default);
    Task<Result<UserInvitation>> Handle(AcceptInvitationCommand command, CancellationToken cancellationToken = default);
    Task<Result<UserAccessProfile>> Handle(AssignUserAccessCommand command, CancellationToken cancellationToken = default);
    Task<Result<UserAccessProfile>> Handle(RestrictUserZoneAccessCommand command, CancellationToken cancellationToken = default);
    Task<Result<UserAccessProfile>> Handle(RevokeUserAccessCommand command, CancellationToken cancellationToken = default);
    Task<Result<UserAccessProfile>> Handle(UpdateNotificationPreferencesCommand command, CancellationToken cancellationToken = default);
}
