namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;

public partial class SubscriptionPlan
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;
}