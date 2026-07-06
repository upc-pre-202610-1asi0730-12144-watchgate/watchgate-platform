namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

public record InvoiceResource(
    int Id,
    int PaymentId,
    int SubscriptionId,
    int CompanyId,
    string Number,
    decimal Amount,
    string Currency,
    string Status,
    DateTime IssuedAt);
