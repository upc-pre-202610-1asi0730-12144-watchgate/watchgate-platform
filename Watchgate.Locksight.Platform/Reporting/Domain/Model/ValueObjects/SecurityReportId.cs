namespace Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;

public readonly record struct SecurityReportId(int Value)
{
    public static implicit operator int(SecurityReportId id) => id.Value;
    public static implicit operator SecurityReportId(int value) => new(value);
    public override string ToString() => Value.ToString();
}