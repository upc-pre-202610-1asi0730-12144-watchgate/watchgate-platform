using Watchgate.Locksight.Platform.Reporting.Application.QueryServices;
using Watchgate.Locksight.Platform.Reporting.Domain.Model;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Reporting.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.Reporting.Application.Internal.QueryServices;

public class ReportingQueryService(
    ISecurityReportRepository reportRepository,
    IScheduledReportRepository scheduledReportRepository,
    IEventLogRepository eventLogRepository) : IReportingQueryService
{
    public async Task<Result<IEnumerable<EventLogEntry>>> Handle(GetEventLogQuery query, CancellationToken cancellationToken = default)
    {
        var from = query.From ?? DateTime.UtcNow.AddDays(-30);
        var to = query.To ?? DateTime.UtcNow;
        if (from > to)
            return Result<IEnumerable<EventLogEntry>>.Failure(ReportingError.InvalidReportRange, "The start date must be earlier than or equal to the end date.");

        var events = await eventLogRepository.FindEventsAsync(query.CompanyId, from, to, query.Type, query.ZoneId, query.WarehouseId, cancellationToken);
        return Result<IEnumerable<EventLogEntry>>.Success(events);
    }

    public async Task<Result<ReportingDashboard>> Handle(GetReportingDashboardQuery query, CancellationToken cancellationToken = default)
    {
        var from = query.From ?? DateTime.UtcNow.AddDays(-30);
        var to = query.To ?? DateTime.UtcNow;
        if (from > to)
            return Result<ReportingDashboard>.Failure(ReportingError.InvalidReportRange, "The start date must be earlier than or equal to the end date.");

        var dashboard = await eventLogRepository.GetDashboardAsync(query.CompanyId, from, to, cancellationToken);
        return Result<ReportingDashboard>.Success(dashboard);
    }

    public async Task<Result<SecurityReport>> Handle(GetReportByIdQuery query, CancellationToken cancellationToken = default)
    {
        var report = await reportRepository.FindByIdAsync(query.ReportId, cancellationToken);
        return report is null
            ? Result<SecurityReport>.Failure(ReportingError.ReportNotFound, $"Report with id {query.ReportId} was not found.")
            : Result<SecurityReport>.Success(report);
    }

    public async Task<Result<IEnumerable<SecurityReport>>> Handle(GetReportsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var reports = await reportRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<SecurityReport>>.Success(reports);
    }

    public async Task<Result<IEnumerable<ScheduledReport>>> Handle(GetScheduledReportsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var reports = await scheduledReportRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<ScheduledReport>>.Success(reports);
    }
}