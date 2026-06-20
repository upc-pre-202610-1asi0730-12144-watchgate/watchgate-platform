using Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

public partial class User : IAuditableEntity
{
    public UserId Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public EmailAddress Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "Visitor";
    public CompanyId CompanyId { get; private set; }

    public Company? Company { get; private set; }

    protected User() { }

    public User(string fullName, EmailAddress email, string passwordHash, CompanyId companyId, string role = "Visitor")
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        CompanyId = companyId;
        Role = role;
    }

    public void UpdatePassword(string passwordHash) => PasswordHash = passwordHash;
}
