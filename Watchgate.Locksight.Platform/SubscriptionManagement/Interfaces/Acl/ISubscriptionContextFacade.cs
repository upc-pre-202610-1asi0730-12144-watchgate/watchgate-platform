namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Acl;

public interface ISubscriptionContextFacade
{
    Task<string> FetchPlanTierByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<bool> IsSubscriptionActiveAsync(int companyId, CancellationToken cancellationToken = default);
}