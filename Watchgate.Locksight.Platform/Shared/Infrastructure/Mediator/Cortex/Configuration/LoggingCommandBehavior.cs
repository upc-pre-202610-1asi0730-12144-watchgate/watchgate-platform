using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;

namespace Watchgate.Locksight.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;

public class LoggingCommandBehavior<TCommand>(ILogger<LoggingCommandBehavior<TCommand>> logger)
    : ICommandPipelineBehavior<TCommand> where TCommand : ICommand
{
    public async Task Handle(TCommand command, CommandHandlerDelegate next, CancellationToken ct)
    {
        logger.LogInformation("Handling command {CommandName}", typeof(TCommand).Name);
        await next();
        logger.LogInformation("Handled command {CommandName}", typeof(TCommand).Name);
    }
}
