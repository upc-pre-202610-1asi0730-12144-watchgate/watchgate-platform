namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;

public class Sensor
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Status { get; private set; } = "ACTIVE";
    public string? Unit { get; private set; }
    public double? LastReading { get; private set; }
    public DateTime? LastReadingAt { get; private set; }
    public int ZoneId { get; private set; }
    public int CompanyId { get; private set; }

    protected Sensor() { }

    public Sensor(string name, string type, string? unit, int zoneId, int companyId)
    {
        Name = name;
        Type = type;
        Unit = unit;
        ZoneId = zoneId;
        CompanyId = companyId;
    }

    public void UpdateStatus(string status) => Status = status;

    public void RecordReading(double value)
    {
        LastReading = value;
        LastReadingAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string type, string? unit)
    {
        Name = name;
        Type = type;
        Unit = unit;
    }
}