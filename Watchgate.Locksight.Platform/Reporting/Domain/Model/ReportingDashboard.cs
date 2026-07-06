namespace Watchgate.Locksight.Platform.Reporting.Domain.Model;

public record ReportingDashboard(
    int CompanyId,
    DateTime From,
    DateTime To,
    int TotalEvents,
    int OpenEvents,
    int ResolvedEvents,
    int CriticalEvents,
    int HighEvents,
    int MediumEvents,
    int LowEvents);