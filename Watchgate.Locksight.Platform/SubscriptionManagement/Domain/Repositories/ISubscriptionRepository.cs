using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription, SubscriptionId>
{
    Task<IEnumerable<Subscription>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Subscription?> FindActiveByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Subscription?> FindByIdWithPlanAsync(SubscriptionId subscriptionId, CancellationToken cancellationToken = default);
}