namespace Watchgate.Locksight.Platform.Shared.Domain.Repositories;

/// <summary>Base repository interface for all repositories.</summary>
public interface IBaseRepository<TEntity, in TId>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
}
