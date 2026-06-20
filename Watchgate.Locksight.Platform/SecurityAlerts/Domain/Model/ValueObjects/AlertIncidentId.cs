namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.ValueObjects;

/// <summary>Strongly typed identity for the <see cref="Aggregates.AlertIncident"/> aggregate root.</summary>
public readonly record struct AlertIncidentId(int Value)
{
    public static implicit operator int(AlertIncidentId id) => id.Value;
    public static implicit operator AlertIncidentId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
