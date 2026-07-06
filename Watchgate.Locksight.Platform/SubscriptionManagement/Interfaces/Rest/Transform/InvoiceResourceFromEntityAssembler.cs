using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Transform;

public static class InvoiceResourceFromEntityAssembler
{
    public static InvoiceResource ToResourceFromEntity(Invoice invoice) =>
        new(invoice.Id, invoice.PaymentId, invoice.SubscriptionId, invoice.CompanyId, invoice.Number,
            invoice.Amount, invoice.Currency, invoice.Status, invoice.IssuedAt);
}
