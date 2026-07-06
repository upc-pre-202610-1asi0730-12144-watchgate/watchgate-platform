using Watchgate.Locksight.Platform.Shared.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;

public partial class Invoice : IAuditableEntity
{
    public InvoiceId Id { get; private set; }
    public PaymentId PaymentId { get; private set; }
    public SubscriptionId SubscriptionId { get; private set; }
    public int CompanyId { get; private set; }
    public string Number { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";
    public string Status { get; private set; } = "ISSUED";
    public DateTime IssuedAt { get; private set; }

    public Payment? Payment { get; private set; }
    public Subscription? Subscription { get; private set; }

    protected Invoice() { }

    public Invoice(PaymentId paymentId, SubscriptionId subscriptionId, int companyId, decimal amount, string currency)
    {
        PaymentId = paymentId;
        SubscriptionId = subscriptionId;
        CompanyId = companyId;
        Amount = amount;
        Currency = string.IsNullOrWhiteSpace(currency) ? "USD" : currency.ToUpperInvariant();
        IssuedAt = DateTime.UtcNow;
        Number = $"INV-{IssuedAt:yyyyMMddHHmmss}-{companyId}";
    }

    public string BuildReceiptText() =>
        $"Invoice: {Number}{Environment.NewLine}" +
        $"Company: {CompanyId}{Environment.NewLine}" +
        $"Subscription: {SubscriptionId.Value}{Environment.NewLine}" +
        $"Amount: {Currency} {Amount:0.00}{Environment.NewLine}" +
        $"Status: {Status}{Environment.NewLine}" +
        $"Issued at UTC: {IssuedAt:O}";
}
