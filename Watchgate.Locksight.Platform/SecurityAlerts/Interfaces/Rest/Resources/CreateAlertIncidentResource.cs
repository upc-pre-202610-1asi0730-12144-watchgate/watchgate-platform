namespace Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Resources;

public record CreateAlertIncidentResource(string Title, string Description, string Priority, int CompanyId);