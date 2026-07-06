using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;

public interface ISubscriptionQueryService
{
    Task<Result<IEnumerable<SubscriptionPlan>>> Handle(GetAllSubscriptionPlansQuery query, CancellationToken cancellationToken = default);
    Task<Result<SubscriptionPlan>> Handle(GetSubscriptionPlanByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<Subscription>> Handle(GetSubscriptionByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Subscription>>> Handle(GetSubscriptionsByCompanyIdQuery query, CancellationToken cancellationToken = default);
}