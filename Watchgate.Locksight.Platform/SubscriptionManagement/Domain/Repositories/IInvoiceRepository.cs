using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

public interface IInvoiceRepository : IBaseRepository<Invoice, InvoiceId>
{
    Task<IEnumerable<Invoice>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
