namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Commands;

public record CreateSecurityAlertCommand(string Type, string Severity, string Description, int SensorId, int CompanyId);