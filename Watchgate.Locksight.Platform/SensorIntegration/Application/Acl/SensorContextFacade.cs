using Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Acl;
using Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Acl;

namespace Watchgate.Locksight.Platform.SensorIntegration.Application.Acl;

/// <summary>
/// Default implementation of <see cref="ISensorContextFacade"/>. Resolves the warehouse's zone ids
/// through the WarehouseManagement ACL (<see cref="IWarehouseContextFacade"/>) and then looks up the
/// matching sensors via the SensorIntegration repositories, so that SensorIntegration never depends
/// on WarehouseManagement's internals and callers never depend on SensorIntegration's internals.
/// </summary>
public class SensorContextFacade(
    IWarehouseContextFacade warehouseContextFacade,
    ISensorRepository sensorRepository) : ISensorContextFacade
{
    public async Task<IEnumerable<int>> FetchSensorIdsByWarehouseIdAsync(int warehouseId,
        CancellationToken cancellationToken = default)
    {
        var zoneIds = (await warehouseContextFacade.FetchZoneIdsByWarehouseIdAsync(warehouseId, cancellationToken))
            .ToList();
        if (zoneIds.Count == 0) return [];

        var sensors = await sensorRepository.FindByZoneIdsAsync(zoneIds, cancellationToken);
        return sensors.Select(sensor => (int)sensor.Id);
    }
}
