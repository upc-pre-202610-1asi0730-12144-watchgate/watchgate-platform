using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription subscription) =>
        new(subscription.Id, subscription.CompanyId, subscription.PlanId, subscription.Plan?.Name, subscription.Status, subscription.StartedAt, subscription.CancelledAt);
}