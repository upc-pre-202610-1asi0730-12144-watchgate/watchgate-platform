using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;

public interface ISensorRepository : IBaseRepository<Sensor>
{
    Task<IEnumerable<Sensor>> FindByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sensor>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}