using Watchgate.Locksight.Platform.Reporting.Domain.Model;

namespace Watchgate.Locksight.Platform.Reporting.Domain.Repositories;

public interface IEventLogRepository
{
    Task<IEnumerable<EventLogEntry>> FindEventsAsync(int companyId, DateTime from, DateTime to, string? type, int? zoneId, int? warehouseId, CancellationToken cancellationToken = default);
    Task<ReportingDashboard> GetDashboardAsync(int companyId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
}