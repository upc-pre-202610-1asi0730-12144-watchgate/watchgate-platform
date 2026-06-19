namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

public class User
{
    public int Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "Visitor";
    public int CompanyId { get; private set; }

    public Company? Company { get; private set; }

    protected User() { }

    public User(string fullName, string email, string passwordHash, int companyId, string role = "Visitor")
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        CompanyId = companyId;
        Role = role;
    }

    public void UpdatePassword(string passwordHash) => PasswordHash = passwordHash;
}
