using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Application.CommandServices;

public interface IBillingCommandService
{
    Task<Result<Invoice>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken = default);
}
