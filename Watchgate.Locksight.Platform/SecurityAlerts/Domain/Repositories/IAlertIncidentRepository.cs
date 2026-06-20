using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

public interface IAlertIncidentRepository : IBaseRepository<AlertIncident, AlertIncidentId>
{
    Task<IEnumerable<AlertIncident>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}