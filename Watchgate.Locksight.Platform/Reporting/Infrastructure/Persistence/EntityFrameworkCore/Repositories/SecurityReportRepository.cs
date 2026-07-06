using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Reporting.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Watchgate.Locksight.Platform.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SecurityReportRepository(AppDbContext context) : BaseRepository<SecurityReport, SecurityReportId>(context), ISecurityReportRepository
{
    public async Task<IEnumerable<SecurityReport>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<SecurityReport>().Where(report => report.CompanyId == companyId).ToListAsync(cancellationToken);
}