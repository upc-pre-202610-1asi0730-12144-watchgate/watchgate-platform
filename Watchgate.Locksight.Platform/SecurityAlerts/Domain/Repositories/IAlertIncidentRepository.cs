using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

public interface IAlertIncidentRepository : IBaseRepository<AlertIncident, AlertIncidentId>
{
    Task<IEnumerable<AlertIncident>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds incidents that have at least one related <see cref="Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates.SecurityAlert"/>
    /// raised by one of the given sensor ids.
    /// </summary>
    Task<IEnumerable<AlertIncident>> FindByRelatedAlertSensorIdsAsync(IEnumerable<int> sensorIds, CancellationToken cancellationToken = default);
}