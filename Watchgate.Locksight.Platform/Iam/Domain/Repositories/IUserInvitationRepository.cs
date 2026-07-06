using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Domain.Repositories;

public interface IUserInvitationRepository : IBaseRepository<UserInvitation, int>
{
    Task<UserInvitation?> FindByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserInvitation>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
