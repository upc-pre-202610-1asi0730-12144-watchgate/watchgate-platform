using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

public partial class UserAccessProfile : IAuditableEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int CompanyId { get; private set; }
    public string Role { get; private set; } = "OperationsManager";
    public string Permissions { get; private set; } = string.Empty;
    public int? RestrictedZoneId { get; private set; }
    public string Status { get; private set; } = "ACTIVE";
    public bool EmailNotificationsEnabled { get; private set; } = true;
    public bool PushNotificationsEnabled { get; private set; } = true;
    public bool CriticalOnlyNotifications { get; private set; }

    protected UserAccessProfile() { }

    public UserAccessProfile(int userId, int companyId, string role, string permissions)
    {
        UserId = userId;
        CompanyId = companyId;
        AssignRole(role, permissions);
    }

    public void AssignRole(string role, string permissions)
    {
        Role = string.IsNullOrWhiteSpace(role) ? "OperationsManager" : role;
        Permissions = permissions;
    }

    public void RestrictToZone(int zoneId) => RestrictedZoneId = zoneId;
    public void Revoke() => Status = "REVOKED";

    public void UpdateNotificationPreferences(bool emailEnabled, bool pushEnabled, bool criticalOnly)
    {
        EmailNotificationsEnabled = emailEnabled;
        PushNotificationsEnabled = pushEnabled;
        CriticalOnlyNotifications = criticalOnly;
    }
}
