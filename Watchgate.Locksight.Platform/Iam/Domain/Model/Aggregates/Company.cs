namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

public class Company
{
    public int Id { get; private set; }
    public string TradeName { get; private set; } = string.Empty;
    public string TaxId { get; private set; } = string.Empty;

    protected Company() { }

    public Company(string tradeName, string taxId)
    {
        TradeName = tradeName;
        TaxId = taxId;
    }
}
