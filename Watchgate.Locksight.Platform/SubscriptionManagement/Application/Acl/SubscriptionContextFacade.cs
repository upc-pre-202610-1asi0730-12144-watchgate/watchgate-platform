using Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Acl;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.Acl;

public class SubscriptionContextFacade(ISubscriptionQueryService subscriptionQueryService) : ISubscriptionContextFacade
{
    public async Task<string> FetchPlanTierByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        var query = new GetSubscriptionByCompanyIdQuery(companyId);
        var subscription = await subscriptionQueryService.Handle(query, cancellationToken);
        
        return subscription?.Tier.ToString() ?? "None";
    }

    public async Task<bool> IsSubscriptionActiveAsync(int companyId, CancellationToken cancellationToken = default)
    {
        var query = new GetSubscriptionByCompanyIdQuery(companyId);
        var subscription = await subscriptionQueryService.Handle(query, cancellationToken);
        
        return subscription?.IsActive ?? false;
    }
}