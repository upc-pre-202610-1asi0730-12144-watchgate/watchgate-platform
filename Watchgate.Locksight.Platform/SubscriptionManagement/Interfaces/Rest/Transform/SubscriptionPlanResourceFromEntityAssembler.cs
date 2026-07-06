using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

public static class SubscriptionPlanResourceFromEntityAssembler
{
    public static SubscriptionPlanResource ToResourceFromEntity(SubscriptionPlan plan) =>
        new(plan.Id, plan.Name, plan.Description, plan.MonthlyPrice, plan.MaxWarehouses, plan.MaxSensors, plan.IsActive);
}