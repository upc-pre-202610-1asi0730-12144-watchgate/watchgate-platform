using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Watchgate.Locksight.Platform.Resources.Errors;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) 
    : ISubscriptionCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var existingSubscription = await subscriptionRepository.FindByCompanyIdAsync(command.CompanyId, cancellationToken);
        if (existingSubscription != null)
            return Result<Subscription>.Failure(SubscriptionError.AlreadyOnThisPlan, 
                _localizer[nameof(SubscriptionError.AlreadyOnThisPlan)]);

        var cardNumber = new CardNumber(command.CardNumber);
        var subscription = new Subscription(command.CompanyId, command.Tier, cardNumber);

        try
        {
            await subscriptionRepository.AddAsync(subscription, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionError.OperationCancelled, 
                _localizer[nameof(SubscriptionError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionError.DatabaseError, 
                _localizer[nameof(SubscriptionError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionError.InternalServerError, 
                _localizer[nameof(SubscriptionError.InternalServerError)]);
        }
    }

    public async Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionError.SubscriptionNotFound, 
                _localizer[nameof(SubscriptionError.SubscriptionNotFound)]);

        try
        {
            subscription.Cancel();
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionError.OperationCancelled, 
                _localizer[nameof(SubscriptionError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionError.DatabaseError, 
                _localizer[nameof(SubscriptionError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(SubscriptionError.InternalServerError, 
                _localizer[nameof(SubscriptionError.InternalServerError)]);
        }
    }

    public async Task<Result<Subscription>> Handle(ChangeSubscriptionPlanCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(SubscriptionError.SubscriptionNotFound, 
                _localizer[nameof(SubscriptionError.SubscriptionNotFound)]);

        try
        {
            subscription.ChangePlan(command.NewTier);
            subscriptionRepository.Update(subscription);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(subscription);
        }
        catch (InvalidOperationException ex)
        {
            return Result<Subscription>.Failure(SubscriptionError.CannotChangeCanceledPlan, ex.Message);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(SubscriptionError.OperationCancelled, 
                _localizer[nameof(SubscriptionError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(SubscriptionError.DatabaseError, 
                _localizer[nameof(SubscriptionError.DatabaseError)]);
        }
        catch (Exception ex)
        {
            return Result<Subscription>.Failure(SubscriptionError.InternalServerError, 
                _localizer[nameof(SubscriptionError.InternalServerError)]);
        }
    }
}