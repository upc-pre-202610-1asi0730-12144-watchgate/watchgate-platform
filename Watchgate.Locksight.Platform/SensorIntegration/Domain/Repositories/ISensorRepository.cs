using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;

public interface ISensorRepository : IBaseRepository<Sensor, SensorId>
{
    Task<IEnumerable<Sensor>> FindByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sensor>> FindByZoneIdsAsync(IEnumerable<int> zoneIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sensor>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}