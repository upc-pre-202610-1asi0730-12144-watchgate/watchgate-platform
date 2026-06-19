namespace Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Resources;

public record CreateSecurityAlertResource(string Type, string Severity, string Description, int SensorId, int CompanyId);