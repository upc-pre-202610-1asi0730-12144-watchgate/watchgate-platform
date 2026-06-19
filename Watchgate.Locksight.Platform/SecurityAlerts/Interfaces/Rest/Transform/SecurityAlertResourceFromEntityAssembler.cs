using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Transform;

public static class SecurityAlertResourceFromEntityAssembler
{
    public static SecurityAlertResource ToResourceFromEntity(SecurityAlert alert) =>
        new(alert.Id, alert.Type, alert.Severity, alert.Status, alert.Description,
            alert.SensorId, alert.CompanyId, alert.TriggeredAt, alert.ResolvedAt);

    public static AlertIncidentResource ToIncidentResourceFromEntity(AlertIncident incident) =>
        new(incident.Id, incident.Title, incident.Description, incident.Status, incident.Priority,
            incident.CompanyId, incident.CreatedAt, incident.ClosedAt);
}