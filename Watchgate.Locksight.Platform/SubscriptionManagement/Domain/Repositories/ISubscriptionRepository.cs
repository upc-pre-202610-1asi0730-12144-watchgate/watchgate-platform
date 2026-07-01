using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription, SubscriptionId>
{
    Task<Subscription?> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}