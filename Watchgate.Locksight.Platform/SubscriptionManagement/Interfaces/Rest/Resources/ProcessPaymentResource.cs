namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

public record ProcessPaymentResource(int SubscriptionId, string Currency, string ProviderReference, bool SimulateFailure);
