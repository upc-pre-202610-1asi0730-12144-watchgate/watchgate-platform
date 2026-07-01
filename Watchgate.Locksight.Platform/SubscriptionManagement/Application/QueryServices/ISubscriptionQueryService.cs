using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;

public interface ISubscriptionQueryService
{
    Task<Subscription?> Handle(GetSubscriptionByCompanyIdQuery query, CancellationToken cancellationToken);
}