namespace Watchgate.Locksight.Platform.Shared.Domain.Model.Events;

/// <summary>Handles a specific domain event type.</summary>
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
