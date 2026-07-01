using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Errors;

public static class SubscriptionErrors
{
    public static readonly Error CategoryCreationFailed =
        new("Subscription.CategoryCreationFailed", "An error occurred while creating the subscription category.");

    public static readonly Error InvalidCardNumber =
        new("Subscription.InvalidCardNumber", "The card number must contain exactly 16 digits.");

    public static readonly Error CompanyIdRequired =
        new("Subscription.CompanyIdRequired", "A valid Company ID is required to process the subscription.");

    public static readonly Error AlreadyCanceled =
        new("Subscription.AlreadyCanceled", "The specified subscription is already canceled.");

    public static readonly Error CannotChangeCanceledPlan =
        new("Subscription.CannotChangeCanceledPlan", "Cannot change the plan of a canceled subscription.");

    public static readonly Error AlreadyOnThisPlan =
        new("Subscription.AlreadyOnThisPlan", "The company is already subscribed to this plan.");
        
    public static readonly Error SubscriptionNotFound =
        new("Subscription.SubscriptionNotFound", "The specified subscription was not found.");
}