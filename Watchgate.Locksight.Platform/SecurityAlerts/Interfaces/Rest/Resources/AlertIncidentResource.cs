namespace Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Resources;

public record AlertIncidentResource(
    int Id,
    string Title,
    string Description,
    string Status,
    string Priority,
    int CompanyId,
    DateTime CreatedAt,
    DateTime? ClosedAt);