using Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;

public partial class ScheduledReport : IAuditableEntity
{
    public ScheduledReportId Id { get; private set; }
    public int CompanyId { get; private set; }
    public int? WarehouseId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Frequency { get; private set; } = "WEEKLY";
    public string Format { get; private set; } = "PDF";
    public string RecipientEmail { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime StartsAt { get; private set; }

    protected ScheduledReport() { }

    public ScheduledReport(int companyId, int? warehouseId, string name, string frequency, string format, string recipientEmail, DateTime startsAt)
    {
        CompanyId = companyId;
        WarehouseId = warehouseId;
        Name = name;
        Frequency = string.IsNullOrWhiteSpace(frequency) ? "WEEKLY" : frequency.ToUpperInvariant();
        Format = string.IsNullOrWhiteSpace(format) ? "PDF" : format.ToUpperInvariant();
        RecipientEmail = recipientEmail;
        StartsAt = startsAt;
    }

    public void Disable() => IsActive = false;
}