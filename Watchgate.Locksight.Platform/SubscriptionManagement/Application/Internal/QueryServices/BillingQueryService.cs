using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.Internal.QueryServices;

public class BillingQueryService(IInvoiceRepository invoiceRepository) : IBillingQueryService
{
    public async Task<Result<Invoice>> Handle(GetInvoiceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var invoice = await invoiceRepository.FindByIdAsync(query.InvoiceId, cancellationToken);
        return invoice is null
            ? Result<Invoice>.Failure(SubscriptionManagementError.InvoiceNotFound, $"Invoice with id {query.InvoiceId} was not found.")
            : Result<Invoice>.Success(invoice);
    }

    public async Task<Result<IEnumerable<Invoice>>> Handle(GetInvoicesByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var invoices = await invoiceRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<Invoice>>.Success(invoices);
    }
}
