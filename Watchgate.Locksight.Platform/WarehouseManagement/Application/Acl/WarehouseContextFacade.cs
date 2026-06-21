using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Acl;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Application.Acl;

/// <summary>
/// Default implementation of <see cref="IWarehouseContextFacade"/>. Wraps the WarehouseManagement
/// module's internal repositories so that other bounded contexts never depend on them directly.
/// </summary>
public class WarehouseContextFacade(IWarehouseZoneRepository zoneRepository) : IWarehouseContextFacade
{
    public async Task<IEnumerable<int>> FetchZoneIdsByWarehouseIdAsync(int warehouseId,
        CancellationToken cancellationToken = default)
    {
        var zones = await zoneRepository.FindByWarehouseIdAsync(warehouseId, cancellationToken);
        return zones.Select(zone => zone.Id);
    }
}
