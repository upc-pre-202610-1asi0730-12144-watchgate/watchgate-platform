using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Transform;

public static class UserAccessResourceFromEntityAssembler
{
    public static UserInvitationResource ToResourceFromInvitation(UserInvitation invitation) =>
        new(invitation.Id, invitation.CompanyId, invitation.Email, invitation.Role, invitation.Permissions,
            invitation.ZoneId, invitation.Token, invitation.Status, invitation.ExpiresAt, invitation.AcceptedAt);

    public static UserAccessProfileResource ToResourceFromProfile(UserAccessProfile profile) =>
        new(profile.Id, profile.UserId, profile.CompanyId, profile.Role, profile.Permissions,
            profile.RestrictedZoneId, profile.Status, profile.EmailNotificationsEnabled,
            profile.PushNotificationsEnabled, profile.CriticalOnlyNotifications);
}
