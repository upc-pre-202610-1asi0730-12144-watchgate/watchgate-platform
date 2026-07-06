namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

public record InviteUserResource(int CompanyId, string Email, string Role, string Permissions, int? ZoneId);
public record CreateTeamUserResource(int CompanyId, string FullName, string Email, string Password, string Role, string Permissions, int? ZoneId);
public record AcceptInvitationResource(string Token);
public record AssignUserAccessResource(int CompanyId, string Role, string Permissions);
public record RestrictUserZoneAccessResource(int ZoneId);
public record UpdateNotificationPreferencesResource(bool EmailEnabled, bool PushEnabled, bool CriticalOnly);

public record UserInvitationResource(
    int Id,
    int CompanyId,
    string Email,
    string Role,
    string Permissions,
    int? ZoneId,
    string Token,
    string Status,
    DateTime ExpiresAt,
    DateTime? AcceptedAt);

public record UserAccessProfileResource(
    int Id,
    int UserId,
    int CompanyId,
    string Role,
    string Permissions,
    int? RestrictedZoneId,
    string Status,
    bool EmailNotificationsEnabled,
    bool PushNotificationsEnabled,
    bool CriticalOnlyNotifications);
