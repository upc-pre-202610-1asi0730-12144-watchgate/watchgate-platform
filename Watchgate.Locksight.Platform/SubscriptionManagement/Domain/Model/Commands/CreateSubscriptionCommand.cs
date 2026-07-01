using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;

public record CreateSubscriptionCommand(int CompanyId, EPlanTier Tier, string CardNumber);