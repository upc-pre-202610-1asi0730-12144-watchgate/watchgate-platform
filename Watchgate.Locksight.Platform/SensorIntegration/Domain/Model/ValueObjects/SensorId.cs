namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.ValueObjects;

/// <summary>Strongly typed identity for the <see cref="Aggregates.Sensor"/> aggregate root.</summary>
public readonly record struct SensorId(int Value)
{
    public static implicit operator int(SensorId id) => id.Value;
    public static implicit operator SensorId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
