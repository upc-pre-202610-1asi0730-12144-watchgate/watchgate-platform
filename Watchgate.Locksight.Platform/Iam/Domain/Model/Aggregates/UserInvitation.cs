using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

public partial class UserInvitation : IAuditableEntity
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Role { get; private set; } = "OperationsManager";
    public string Permissions { get; private set; } = string.Empty;
    public int? ZoneId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public string Status { get; private set; } = "PENDING";
    public DateTime ExpiresAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }

    protected UserInvitation() { }

    public UserInvitation(int companyId, string email, string role, string permissions, int? zoneId)
    {
        CompanyId = companyId;
        Email = email;
        Role = string.IsNullOrWhiteSpace(role) ? "OperationsManager" : role;
        Permissions = permissions;
        ZoneId = zoneId;
        Token = Guid.NewGuid().ToString("N");
        ExpiresAt = DateTime.UtcNow.AddDays(7);
    }

    public void Accept()
    {
        Status = "ACCEPTED";
        AcceptedAt = DateTime.UtcNow;
    }

    public void Revoke() => Status = "REVOKED";
}
