namespace Watchgate.Locksight.Platform.Iam.Interfaces.Acl;

/// <summary>
/// Anti-Corruption Layer exposed by the IAM bounded context. Other bounded contexts must depend on this
/// facade instead of reaching into IAM's repositories, commands or queries directly.
/// </summary>
public interface IIamContextFacade
{
    Task<UserSummary?> FetchUserByIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserSummary?> FetchUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsCompanyAsync(int companyId, CancellationToken cancellationToken = default);
}
