namespace Watchgate.Locksight.Platform.Shared.Domain.Model;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }

    void SetCreatedAt(DateTime createdAt);
    void SetUpdatedAt(DateTime updatedAt);
}
