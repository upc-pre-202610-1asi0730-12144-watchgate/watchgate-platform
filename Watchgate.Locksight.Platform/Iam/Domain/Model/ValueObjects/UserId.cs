namespace Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;

/// <summary>Strongly typed identity for the <see cref="Aggregates.User"/> aggregate root.</summary>
public readonly record struct UserId(int Value)
{
    public static implicit operator int(UserId id) => id.Value;
    public static implicit operator UserId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
