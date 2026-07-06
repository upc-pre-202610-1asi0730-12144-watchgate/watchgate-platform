using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.Internal.QueryServices;

public class SubscriptionQueryService(
    ISubscriptionPlanRepository planRepository,
    ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryService
{
    public async Task<Result<IEnumerable<SubscriptionPlan>>> Handle(GetAllSubscriptionPlansQuery query, CancellationToken cancellationToken = default)
    {
        var plans = await planRepository.FindActiveAsync(cancellationToken);
        return Result<IEnumerable<SubscriptionPlan>>.Success(plans);
    }

    public async Task<Result<SubscriptionPlan>> Handle(GetSubscriptionPlanByIdQuery query, CancellationToken cancellationToken = default)
    {
        var plan = await planRepository.FindByIdAsync(query.PlanId, cancellationToken);
        return plan is null
            ? Result<SubscriptionPlan>.Failure(SubscriptionManagementError.PlanNotFound, $"Plan with id {query.PlanId} was not found.")
            : Result<SubscriptionPlan>.Success(plan);
    }

    public async Task<Result<Subscription>> Handle(GetSubscriptionByIdQuery query, CancellationToken cancellationToken = default)
    {
        var subscription = await subscriptionRepository.FindByIdWithPlanAsync(query.SubscriptionId, cancellationToken);
        return subscription is null
            ? Result<Subscription>.Failure(SubscriptionManagementError.SubscriptionNotFound, $"Subscription with id {query.SubscriptionId} was not found.")
            : Result<Subscription>.Success(subscription);
    }

    public async Task<Result<IEnumerable<Subscription>>> Handle(GetSubscriptionsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var subscriptions = await subscriptionRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<Subscription>>.Success(subscriptions);
    }
}