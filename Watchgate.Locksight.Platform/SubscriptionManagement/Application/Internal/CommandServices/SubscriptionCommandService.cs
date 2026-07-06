using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionPlanRepository planRepository,
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork) : ISubscriptionCommandService
{
    public async Task<Result<SubscriptionPlan>> Handle(CreateSubscriptionPlanCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var plan = new SubscriptionPlan(command.Name, command.Description, command.MonthlyPrice, command.MaxWarehouses, command.MaxSensors);
            await planRepository.AddAsync(plan, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SubscriptionPlan>.Success(plan);
        }
        catch (OperationCanceledException)
        {
            return Result<SubscriptionPlan>.Failure(SubscriptionManagementError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<SubscriptionPlan>.Failure(SubscriptionManagementError.DatabaseError, "A database error occurred while creating the subscription plan.");
        }
        catch (Exception)
        {
            return Result<SubscriptionPlan>.Failure(SubscriptionManagementError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var plan = await planRepository.FindByIdAsync(command.PlanId, cancellationToken);
            if (plan is null || !plan.IsActive)
                return Result<Subscription>.Failure(SubscriptionManagementError.PlanNotFound, $"Plan with id {command.PlanId} was not found or is inactive.");

            var currentSubscription = await subscriptionRepository.FindActiveByCompanyIdAsync(command.CompanyId, cancellationToken);
            if (currentSubscription is not null)
                return Result<Subscription>.Failure(SubscriptionManagementError.ActiveSubscriptionAlreadyExists, "The company already has an active subscription.");

            var subscription = new Subscription(command.CompanyId, command.PlanId);
            await subscriptionRepository.AddAsync(subscription, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.DatabaseError, "A database error occurred while creating the subscription.");
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<Subscription>> Handle(ChangeSubscriptionPlanCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
            if (subscription is null)
                return Result<Subscription>.Failure(SubscriptionManagementError.SubscriptionNotFound, $"Subscription with id {command.SubscriptionId} was not found.");

            var plan = await planRepository.FindByIdAsync(command.PlanId, cancellationToken);
            if (plan is null || !plan.IsActive)
                return Result<Subscription>.Failure(SubscriptionManagementError.PlanNotFound, $"Plan with id {command.PlanId} was not found or is inactive.");

            subscription.ChangePlan(command.PlanId);
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.DatabaseError, "A database error occurred while changing the subscription plan.");
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
            if (subscription is null)
                return Result<Subscription>.Failure(SubscriptionManagementError.SubscriptionNotFound, $"Subscription with id {command.SubscriptionId} was not found.");

            subscription.Cancel();
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.DatabaseError, "A database error occurred while cancelling the subscription.");
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionManagementError.InternalServerError, "An unexpected error occurred.");
        }
    }
}