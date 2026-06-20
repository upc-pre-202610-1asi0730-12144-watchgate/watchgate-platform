using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

public interface ISecurityAlertRepository : IBaseRepository<SecurityAlert, SecurityAlertId>
{
    Task<IEnumerable<SecurityAlert>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}