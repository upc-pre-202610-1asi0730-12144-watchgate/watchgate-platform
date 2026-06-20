namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.ValueObjects;

/// <summary>Value Object that wraps a validated physical address, avoiding primitive obsession on raw strings.</summary>
public sealed record StreetAddress
{
    public string Value { get; }

    public StreetAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Address cannot be empty.", nameof(value));

        Value = value.Trim();
    }

    public static implicit operator string(StreetAddress address) => address.Value;
    public static implicit operator StreetAddress(string value) => new(value);

    public override string ToString() => Value;
}
