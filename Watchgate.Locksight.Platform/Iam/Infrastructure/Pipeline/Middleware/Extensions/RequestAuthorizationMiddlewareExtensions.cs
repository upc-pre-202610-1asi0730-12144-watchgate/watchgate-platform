using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Components;

namespace Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;

public static class RequestAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder builder) =>
        builder.UseMiddleware<RequestAuthorizationMiddleware>();
}
