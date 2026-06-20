namespace Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;

/// <summary>Strongly typed identity for the <see cref="Aggregates.Company"/> aggregate root.</summary>
public readonly record struct CompanyId(int Value)
{
    public static implicit operator int(CompanyId id) => id.Value;
    public static implicit operator CompanyId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
