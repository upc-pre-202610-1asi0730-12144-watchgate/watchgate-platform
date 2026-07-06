using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;

public interface IBillingQueryService
{
    Task<Result<Invoice>> Handle(GetInvoiceByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Invoice>>> Handle(GetInvoicesByCompanyIdQuery query, CancellationToken cancellationToken = default);
}
