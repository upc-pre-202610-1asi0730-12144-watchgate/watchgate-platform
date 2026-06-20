using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Model.Events;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Events;

/// <summary>Raised right after a new <see cref="Aggregates.SecurityAlert"/> is persisted.</summary>
public sealed class SecurityAlertTriggeredEvent(
    SecurityAlertId alertId, string type, string severity, int sensorId, int companyId) : IEvent
{
    public SecurityAlertId AlertId { get; } = alertId;
    public string Type { get; } = type;
    public string Severity { get; } = severity;
    public int SensorId { get; } = sensorId;
    public int CompanyId { get; } = companyId;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
