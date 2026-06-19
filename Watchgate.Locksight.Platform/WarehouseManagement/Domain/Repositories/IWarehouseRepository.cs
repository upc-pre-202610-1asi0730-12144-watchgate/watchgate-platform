using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

public interface IWarehouseRepository : IBaseRepository<Warehouse>
{
    Task<IEnumerable<Warehouse>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Warehouse?> FindByIdWithZonesAsync(int warehouseId, CancellationToken cancellationToken = default);
}
