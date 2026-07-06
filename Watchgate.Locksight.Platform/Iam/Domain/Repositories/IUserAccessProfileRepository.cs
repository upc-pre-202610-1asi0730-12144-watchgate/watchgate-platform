using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Domain.Repositories;

public interface IUserAccessProfileRepository : IBaseRepository<UserAccessProfile, int>
{
    Task<UserAccessProfile?> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserAccessProfile>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
