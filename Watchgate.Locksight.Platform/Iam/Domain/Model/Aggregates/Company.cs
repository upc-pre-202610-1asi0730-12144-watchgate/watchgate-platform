using Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

public partial class Company : IAuditableEntity
{
    public CompanyId Id { get; private set; }
    public string TradeName { get; private set; } = string.Empty;
    public string TaxId { get; private set; } = string.Empty;

    protected Company() { }

    public Company(string tradeName, string taxId)
    {
        TradeName = tradeName;
        TaxId = taxId;
    }
}
