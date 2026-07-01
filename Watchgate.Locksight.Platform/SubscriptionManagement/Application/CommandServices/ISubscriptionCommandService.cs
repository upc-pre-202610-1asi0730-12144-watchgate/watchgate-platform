using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;

public interface ISubscriptionCommandService
{
    Task<Result<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken);
    Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken);
    Task<Result<Subscription>> Handle(ChangeSubscriptionPlanCommand command, CancellationToken cancellationToken);
}