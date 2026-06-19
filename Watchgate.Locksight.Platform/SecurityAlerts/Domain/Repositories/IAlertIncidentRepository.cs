using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

public interface IAlertIncidentRepository : IBaseRepository<AlertIncident>
{
    Task<IEnumerable<AlertIncident>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}