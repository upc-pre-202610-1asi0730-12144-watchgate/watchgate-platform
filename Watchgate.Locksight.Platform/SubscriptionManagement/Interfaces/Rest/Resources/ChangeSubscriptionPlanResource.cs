using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;


public record ChangeSubscriptionPlanResource(string NewTier);