namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.ValueObjects;

public readonly record struct CompanyAccountId(int Value)
{
    public static implicit operator int(CompanyAccountId id) => id.Value;
    public static implicit operator CompanyAccountId(int value) => new(value);
    public override string ToString() => Value.ToString();
}
