using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Events;

public record SubscriptionCreatedEvent(int SubscriptionId, int CompanyId, EPlanTier Tier);