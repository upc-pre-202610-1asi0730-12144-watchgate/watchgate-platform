namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Commands;

public record CreateAlertIncidentCommand(string Title, string Description, string Priority, int CompanyId, int? RelatedAlertId);
