using Watchgate.Locksight.Platform.Iam.Application.QueryServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Iam.Interfaces.Acl;

namespace Watchgate.Locksight.Platform.Iam.Application.Acl;

/// <summary>
/// Default implementation of <see cref="IIamContextFacade"/>. Wraps the IAM module's internal
/// query services and repositories so that other bounded contexts never depend on them directly.
/// </summary>
public class IamContextFacade(
    IUserQueryService userQueryService,
    ICompanyRepository companyRepository) : IIamContextFacade
{
    public async Task<UserSummary?> FetchUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), cancellationToken);
        return user is null ? null : new UserSummary(user.Id, user.FullName, user.Email, user.Role, user.CompanyId);
    }

    public async Task<UserSummary?> FetchUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByEmailQuery(email), cancellationToken);
        return user is null ? null : new UserSummary(user.Id, user.FullName, user.Email, user.Role, user.CompanyId);
    }

    public async Task<bool> ExistsCompanyAsync(int companyId, CancellationToken cancellationToken = default) =>
        await companyRepository.FindByIdAsync(companyId, cancellationToken) is not null;
}
