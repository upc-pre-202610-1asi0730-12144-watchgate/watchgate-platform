using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>Base repository for all repositories.</summary>
public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
{
    protected readonly AppDbContext Context;

    protected BaseRepository(AppDbContext context) => Context = context;

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public async Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default) =>
        await Context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken);

    public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

    public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

    public async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default) =>
        await Context.Set<TEntity>().ToListAsync(cancellationToken);
}
