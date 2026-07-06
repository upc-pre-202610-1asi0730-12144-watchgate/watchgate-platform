namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Commands;

public record ClassifyAlertPriorityCommand(int AlertId, string Severity);
