namespace Watchgate.Locksight.Platform.Iam.Interfaces.Acl;

/// <summary>Read-only projection of a User exposed to other bounded contexts through the IAM ACL.</summary>
public sealed record UserSummary(int Id, string FullName, string Email, string Role, int CompanyId);
