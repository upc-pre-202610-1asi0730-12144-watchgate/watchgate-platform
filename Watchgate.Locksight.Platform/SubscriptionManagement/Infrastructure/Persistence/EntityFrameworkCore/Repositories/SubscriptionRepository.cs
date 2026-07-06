using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SubscriptionRepository(AppDbContext context) : BaseRepository<Subscription, SubscriptionId>(context), ISubscriptionRepository
{
    public async Task<IEnumerable<Subscription>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<Subscription>().Include(subscription => subscription.Plan)
            .Where(subscription => subscription.CompanyId == companyId)
            .ToListAsync(cancellationToken);

    public async Task<Subscription?> FindActiveByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<Subscription>().Include(subscription => subscription.Plan)
            .FirstOrDefaultAsync(subscription => subscription.CompanyId == companyId && subscription.Status == "ACTIVE", cancellationToken);

    public async Task<Subscription?> FindByIdWithPlanAsync(SubscriptionId subscriptionId, CancellationToken cancellationToken = default) =>
        await Context.Set<Subscription>().Include(subscription => subscription.Plan)
            .FirstOrDefaultAsync(subscription => subscription.Id == subscriptionId, cancellationToken);
}