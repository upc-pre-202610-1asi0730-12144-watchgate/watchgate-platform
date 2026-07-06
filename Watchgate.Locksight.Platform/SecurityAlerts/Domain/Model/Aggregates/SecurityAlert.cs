using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;

public partial class SecurityAlert : IAuditableEntity
{
    public SecurityAlertId Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Severity { get; private set; } = "LOW";
    public string Status { get; private set; } = "OPEN";
    public string Description { get; private set; } = string.Empty;
    public int SensorId { get; private set; }
    public int CompanyId { get; private set; }
    public DateTime TriggeredAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }

    protected SecurityAlert() { }

    public SecurityAlert(string type, string severity, string description, int sensorId, int companyId)
    {
        Type = type;
        Severity = severity;
        Description = description;
        SensorId = sensorId;
        CompanyId = companyId;
        TriggeredAt = DateTime.UtcNow;
    }

    public void Resolve()
    {
        Status = "RESOLVED";
        ResolvedAt = DateTime.UtcNow;
    }

    public void Acknowledge() => Status = "ACKNOWLEDGED";

    public void MarkAttended() => Status = "ATTENDED";

    public void Escalate()
    {
        Status = "ESCALATED";
        Severity = "CRITICAL";
    }

    public void FlagAsFalseAlarm() => Status = "FALSE_ALARM";

    public void UpdateSeverity(string severity) => Severity = severity;
}
