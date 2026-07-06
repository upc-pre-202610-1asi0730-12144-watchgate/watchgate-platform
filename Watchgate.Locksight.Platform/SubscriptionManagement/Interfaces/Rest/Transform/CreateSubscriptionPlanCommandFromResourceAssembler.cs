using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

public static class CreateSubscriptionPlanCommandFromResourceAssembler
{
    public static CreateSubscriptionPlanCommand ToCommandFromResource(CreateSubscriptionPlanResource resource) =>
        new(resource.Name, resource.Description, resource.MonthlyPrice, resource.MaxWarehouses, resource.MaxSensors);
}