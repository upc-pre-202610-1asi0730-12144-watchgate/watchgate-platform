namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;

public partial class CompanyAccount
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;
}
