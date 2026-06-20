using Watchgate.Locksight.Platform.Shared.Domain.Model.Events;

namespace Watchgate.Locksight.Platform.Shared.Infrastructure.Events;

/// <summary>Resolves and invokes every registered <see cref="IEventHandler{TEvent}"/> for the dispatched event's runtime type.</summary>
public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task DispatchAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var handlers = (IEnumerable<object>)serviceProvider.GetServices(handlerType)!;
        var method = handlerType.GetMethod(nameof(IEventHandler<IEvent>.HandleAsync))!;

        foreach (var handler in handlers)
            await (Task)method.Invoke(handler, [@event, cancellationToken])!;
    }
}
