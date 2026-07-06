using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SubscriptionPlanRepository(AppDbContext context) : BaseRepository<SubscriptionPlan, SubscriptionPlanId>(context), ISubscriptionPlanRepository
{
    public async Task<IEnumerable<SubscriptionPlan>> FindActiveAsync(CancellationToken cancellationToken = default) =>
        await Context.Set<SubscriptionPlan>().Where(plan => plan.IsActive).ToListAsync(cancellationToken);
}