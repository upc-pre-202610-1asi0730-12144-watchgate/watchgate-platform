namespace Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;

public readonly record struct ScheduledReportId(int Value)
{
    public static implicit operator int(ScheduledReportId id) => id.Value;
    public static implicit operator ScheduledReportId(int value) => new(value);
    public override string ToString() => Value.ToString();
}