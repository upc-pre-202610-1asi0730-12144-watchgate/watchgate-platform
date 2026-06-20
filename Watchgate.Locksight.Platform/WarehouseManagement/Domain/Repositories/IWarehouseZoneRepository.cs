using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

public interface IWarehouseZoneRepository : IBaseRepository<WarehouseZone, int>
{
    Task<IEnumerable<WarehouseZone>> FindByWarehouseIdAsync(WarehouseId warehouseId, CancellationToken cancellationToken = default);
}
