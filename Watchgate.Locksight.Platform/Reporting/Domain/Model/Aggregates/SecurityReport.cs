using Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;

public partial class SecurityReport : IAuditableEntity
{
    public SecurityReportId Id { get; private set; }
    public int CompanyId { get; private set; }
    public int? WarehouseId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    public string Format { get; private set; } = "PDF";
    public string Status { get; private set; } = "GENERATED";
    public int TotalEvents { get; private set; }
    public int CriticalEvents { get; private set; }
    public int ResolvedEvents { get; private set; }
    public DateTime GeneratedAt { get; private set; }

    protected SecurityReport() { }

    public SecurityReport(int companyId, int? warehouseId, string title, DateTime from, DateTime to, string format, int totalEvents, int criticalEvents, int resolvedEvents)
    {
        CompanyId = companyId;
        WarehouseId = warehouseId;
        Title = title;
        From = from;
        To = to;
        Format = string.IsNullOrWhiteSpace(format) ? "PDF" : format.ToUpperInvariant();
        TotalEvents = totalEvents;
        CriticalEvents = criticalEvents;
        ResolvedEvents = resolvedEvents;
        GeneratedAt = DateTime.UtcNow;
    }
}