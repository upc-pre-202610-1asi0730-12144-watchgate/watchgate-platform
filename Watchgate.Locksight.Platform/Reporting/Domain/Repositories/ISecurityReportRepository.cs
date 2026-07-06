using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Reporting.Domain.Repositories;

public interface ISecurityReportRepository : IBaseRepository<SecurityReport, SecurityReportId>
{
    Task<IEnumerable<SecurityReport>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}