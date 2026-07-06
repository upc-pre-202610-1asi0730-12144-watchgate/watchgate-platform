using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Reporting.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Watchgate.Locksight.Platform.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ScheduledReportRepository(AppDbContext context) : BaseRepository<ScheduledReport, ScheduledReportId>(context), IScheduledReportRepository
{
    public async Task<IEnumerable<ScheduledReport>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<ScheduledReport>().Where(report => report.CompanyId == companyId).ToListAsync(cancellationToken);
}