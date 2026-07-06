using Watchgate.Locksight.Platform.Reporting.Domain.Model;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Transform;

public static class ReportingResourceFromEntityAssembler
{
    public static EventLogEntryResource ToResourceFromEventLogEntry(EventLogEntry entry) =>
        new(entry.Id, entry.EventType, entry.Severity, entry.Status, entry.Description, entry.SensorId, entry.CompanyId, entry.OccurredAt);

    public static ReportingDashboardResource ToResourceFromDashboard(ReportingDashboard dashboard) =>
        new(dashboard.CompanyId, dashboard.From, dashboard.To, dashboard.TotalEvents, dashboard.OpenEvents, dashboard.ResolvedEvents, dashboard.CriticalEvents, dashboard.HighEvents, dashboard.MediumEvents, dashboard.LowEvents);

    public static SecurityReportResource ToResourceFromSecurityReport(SecurityReport report) =>
        new(report.Id, report.CompanyId, report.WarehouseId, report.Title, report.From, report.To, report.Format, report.Status, report.TotalEvents, report.CriticalEvents, report.ResolvedEvents, report.GeneratedAt);

    public static ScheduledReportResource ToResourceFromScheduledReport(ScheduledReport report) =>
        new(report.Id, report.CompanyId, report.WarehouseId, report.Name, report.Frequency, report.Format, report.RecipientEmail, report.IsActive, report.StartsAt);
}