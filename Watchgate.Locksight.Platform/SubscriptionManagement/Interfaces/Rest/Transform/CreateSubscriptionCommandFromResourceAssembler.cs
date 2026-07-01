using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

public static class CreateSubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommandFromResource(CreateSubscriptionResource resource)
    {
        return new CreateSubscriptionCommand(
            resource.CompanyId,
            Enum.Parse<EPlanTier>(resource.Tier),
            resource.CardNumber
        );
    }
}