namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;

/// <summary>Strongly typed identity for the <see cref="Aggregates.SecurityAlert"/> aggregate root.</summary>
public readonly record struct SecurityAlertId(int Value)
{
    public static implicit operator int(SecurityAlertId id) => id.Value;
    public static implicit operator SecurityAlertId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
