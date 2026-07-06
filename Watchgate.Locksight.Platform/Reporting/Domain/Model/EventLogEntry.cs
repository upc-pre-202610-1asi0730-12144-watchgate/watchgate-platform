namespace Watchgate.Locksight.Platform.Reporting.Domain.Model;

public record EventLogEntry(
    int Id,
    string EventType,
    string Severity,
    string Status,
    string Description,
    int SensorId,
    int CompanyId,
    DateTime OccurredAt);