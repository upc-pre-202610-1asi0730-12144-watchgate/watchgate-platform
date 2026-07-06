namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;

public record ProcessPaymentCommand(int SubscriptionId, string Currency, string ProviderReference, bool SimulateFailure);
