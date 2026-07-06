using Watchgate.Locksight.Platform.Reporting.Domain.Model;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.Reporting.Application.QueryServices;

public interface IReportingQueryService
{
    Task<Result<IEnumerable<EventLogEntry>>> Handle(GetEventLogQuery query, CancellationToken cancellationToken = default);
    Task<Result<ReportingDashboard>> Handle(GetReportingDashboardQuery query, CancellationToken cancellationToken = default);
    Task<Result<SecurityReport>> Handle(GetReportByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SecurityReport>>> Handle(GetReportsByCompanyIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ScheduledReport>>> Handle(GetScheduledReportsByCompanyIdQuery query, CancellationToken cancellationToken = default);
}