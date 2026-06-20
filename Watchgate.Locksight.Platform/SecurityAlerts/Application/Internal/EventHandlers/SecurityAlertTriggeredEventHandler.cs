using Microsoft.Extensions.Logging;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Events;
using Watchgate.Locksight.Platform.Shared.Domain.Model.Events;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.EventHandlers;

/// <summary>Logs every triggered security alert for observability purposes.</summary>
public class SecurityAlertTriggeredEventHandler(ILogger<SecurityAlertTriggeredEventHandler> logger)
    : IEventHandler<SecurityAlertTriggeredEvent>
{
    public Task HandleAsync(SecurityAlertTriggeredEvent @event, CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Security alert {AlertId} of type {Type} ({Severity}) triggered for sensor {SensorId} in company {CompanyId}.",
            @event.AlertId, @event.Type, @event.Severity, @event.SensorId, @event.CompanyId);
        return Task.CompletedTask;
    }
}
