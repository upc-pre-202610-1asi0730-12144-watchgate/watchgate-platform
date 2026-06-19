using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class WarehouseZoneRepository(AppDbContext context) : BaseRepository<WarehouseZone>(context), IWarehouseZoneRepository
{
    public async Task<IEnumerable<WarehouseZone>> FindByWarehouseIdAsync(int warehouseId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<WarehouseZone>().Where(z => z.WarehouseId == warehouseId)
            .ToListAsync(cancellationToken);
}
