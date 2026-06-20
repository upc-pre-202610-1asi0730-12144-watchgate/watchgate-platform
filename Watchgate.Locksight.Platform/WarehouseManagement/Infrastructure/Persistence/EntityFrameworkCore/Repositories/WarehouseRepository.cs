using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class WarehouseRepository(AppDbContext context) : BaseRepository<Warehouse, WarehouseId>(context), IWarehouseRepository
{
    public async Task<IEnumerable<Warehouse>> FindByCompanyIdAsync(int companyId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Warehouse>().Include(w => w.Zones)
            .Where(w => w.CompanyId == companyId).ToListAsync(cancellationToken);

    public async Task<Warehouse?> FindByIdWithZonesAsync(WarehouseId warehouseId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Warehouse>().Include(w => w.Zones)
            .FirstOrDefaultAsync(w => w.Id == warehouseId, cancellationToken);
}
