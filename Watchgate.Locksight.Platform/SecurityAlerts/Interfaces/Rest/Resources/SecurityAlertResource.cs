namespace Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Resources;

public record SecurityAlertResource(
    int Id,
    string Type,
    string Severity,
    string Status,
    string Description,
    int SensorId,
    int CompanyId,
    DateTime TriggeredAt,
    DateTime? ResolvedAt);