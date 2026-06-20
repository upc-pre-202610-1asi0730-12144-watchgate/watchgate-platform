using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

public interface IWarehouseRepository : IBaseRepository<Warehouse, WarehouseId>
{
    Task<IEnumerable<Warehouse>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Warehouse?> FindByIdWithZonesAsync(WarehouseId warehouseId, CancellationToken cancellationToken = default);
}
