using Watchgate.Locksight.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace Watchgate.Locksight.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder) =>
        builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
