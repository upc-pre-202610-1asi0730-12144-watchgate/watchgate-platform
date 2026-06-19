using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

public interface ISecurityAlertRepository : IBaseRepository<SecurityAlert>
{
    Task<IEnumerable<SecurityAlert>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}