namespace Watchgate.Locksight.Platform.Reporting.Domain.Model.Queries;
public record GetReportingDashboardQuery(int CompanyId, DateTime? From, DateTime? To);