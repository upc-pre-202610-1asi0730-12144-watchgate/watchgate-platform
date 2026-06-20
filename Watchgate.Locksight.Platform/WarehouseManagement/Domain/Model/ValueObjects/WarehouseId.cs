namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.ValueObjects;

/// <summary>Strongly typed identity for the <see cref="Aggregates.Warehouse"/> aggregate root.</summary>
public readonly record struct WarehouseId(int Value)
{
    public static implicit operator int(WarehouseId id) => id.Value;
    public static implicit operator WarehouseId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
