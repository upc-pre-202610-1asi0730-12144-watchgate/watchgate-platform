using Watchgate.Locksight.Platform.Shared.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;

public partial class Payment : IAuditableEntity
{
    public PaymentId Id { get; private set; }
    public SubscriptionId SubscriptionId { get; private set; }
    public int CompanyId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";
    public string Provider { get; private set; } = "STRIPE_SIMULATED";
    public string ProviderReference { get; private set; } = string.Empty;
    public string Status { get; private set; } = "PENDING";
    public DateTime RequestedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    public Subscription? Subscription { get; private set; }

    protected Payment() { }

    public Payment(SubscriptionId subscriptionId, int companyId, decimal amount, string currency, string providerReference)
    {
        SubscriptionId = subscriptionId;
        CompanyId = companyId;
        Amount = amount;
        Currency = string.IsNullOrWhiteSpace(currency) ? "USD" : currency.ToUpperInvariant();
        ProviderReference = providerReference;
        RequestedAt = DateTime.UtcNow;
    }

    public void MarkProcessed()
    {
        Status = "PROCESSED";
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkFailed()
    {
        Status = "FAILED";
        ProcessedAt = DateTime.UtcNow;
    }
}
