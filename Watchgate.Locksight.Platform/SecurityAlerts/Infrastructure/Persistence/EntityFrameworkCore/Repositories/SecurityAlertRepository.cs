using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SecurityAlertRepository(AppDbContext context) : BaseRepository<SecurityAlert, SecurityAlertId>(context), ISecurityAlertRepository
{
    public async Task<IEnumerable<SecurityAlert>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<SecurityAlert>().Where(a => a.CompanyId == companyId).ToListAsync(cancellationToken);

    public async Task<IEnumerable<SecurityAlert>> FindBySensorIdsAsync(IEnumerable<int> sensorIds, CancellationToken cancellationToken = default) =>
        await Context.Set<SecurityAlert>().Where(a => sensorIds.Contains(a.SensorId)).ToListAsync(cancellationToken);
}