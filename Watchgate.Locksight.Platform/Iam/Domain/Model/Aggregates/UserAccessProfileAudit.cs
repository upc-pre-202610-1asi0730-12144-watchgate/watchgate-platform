namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

public partial class UserAccessProfile
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;
}
