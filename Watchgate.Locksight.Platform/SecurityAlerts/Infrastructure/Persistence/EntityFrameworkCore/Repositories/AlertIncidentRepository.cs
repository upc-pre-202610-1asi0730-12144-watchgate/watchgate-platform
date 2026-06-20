using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertIncidentRepository(AppDbContext context) : BaseRepository<AlertIncident, AlertIncidentId>(context), IAlertIncidentRepository
{
    public async Task<IEnumerable<AlertIncident>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<AlertIncident>().Where(i => i.CompanyId == companyId).ToListAsync(cancellationToken);
}