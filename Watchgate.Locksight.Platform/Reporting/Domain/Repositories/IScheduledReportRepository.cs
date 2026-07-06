using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Reporting.Domain.Repositories;

public interface IScheduledReportRepository : IBaseRepository<ScheduledReport, ScheduledReportId>
{
    Task<IEnumerable<ScheduledReport>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}