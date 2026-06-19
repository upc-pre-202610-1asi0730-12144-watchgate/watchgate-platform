using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

public interface IWarehouseZoneRepository : IBaseRepository<WarehouseZone>
{
    Task<IEnumerable<WarehouseZone>> FindByWarehouseIdAsync(int warehouseId, CancellationToken cancellationToken = default);
}
