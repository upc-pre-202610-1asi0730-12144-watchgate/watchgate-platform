namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;

public class AlertIncident
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Status { get; private set; } = "OPEN";
    public string Priority { get; private set; } = "MEDIUM";
    public int CompanyId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public ICollection<SecurityAlert> RelatedAlerts { get; private set; } = [];

    protected AlertIncident() { }

    public AlertIncident(string title, string description, string priority, int companyId)
    {
        Title = title;
        Description = description;
        Priority = priority;
        CompanyId = companyId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        Status = "CLOSED";
        ClosedAt = DateTime.UtcNow;
    }

    public void UpdatePriority(string priority) => Priority = priority;
}