using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;

public record ChangeSubscriptionPlanCommand(int SubscriptionId, EPlanTier NewTier);