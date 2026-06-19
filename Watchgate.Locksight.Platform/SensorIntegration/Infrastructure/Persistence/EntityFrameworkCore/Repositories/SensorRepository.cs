using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SensorIntegration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SensorRepository(AppDbContext context) : BaseRepository<Sensor>(context), ISensorRepository
{
    public async Task<IEnumerable<Sensor>> FindByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default) =>
        await Context.Set<Sensor>().Where(s => s.ZoneId == zoneId).ToListAsync(cancellationToken);

    public async Task<IEnumerable<Sensor>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<Sensor>().Where(s => s.CompanyId == companyId).ToListAsync(cancellationToken);
}