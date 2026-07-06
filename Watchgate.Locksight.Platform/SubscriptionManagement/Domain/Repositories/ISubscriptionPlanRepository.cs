using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

public interface ISubscriptionPlanRepository : IBaseRepository<SubscriptionPlan, SubscriptionPlanId>
{
    Task<IEnumerable<SubscriptionPlan>> FindActiveAsync(CancellationToken cancellationToken = default);
}