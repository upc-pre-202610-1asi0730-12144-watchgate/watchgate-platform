namespace Watchgate.Locksight.Platform.Shared.Domain.Model.Events;

/// <summary>Dispatches a domain event to every <see cref="IEventHandler{TEvent}"/> registered for its type.</summary>
public interface IEventDispatcher
{
    Task DispatchAsync(IEvent @event, CancellationToken cancellationToken = default);
}
