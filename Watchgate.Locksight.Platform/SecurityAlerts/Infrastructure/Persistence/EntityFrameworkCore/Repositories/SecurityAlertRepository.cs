using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SecurityAlertRepository(AppDbContext context) : BaseRepository<SecurityAlert>(context), ISecurityAlertRepository
{
    public async Task<IEnumerable<SecurityAlert>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<SecurityAlert>().Where(a => a.CompanyId == companyId).ToListAsync(cancellationToken);
}