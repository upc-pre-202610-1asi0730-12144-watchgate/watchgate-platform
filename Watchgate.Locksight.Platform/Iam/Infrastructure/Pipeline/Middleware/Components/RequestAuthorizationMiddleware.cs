using Watchgate.Locksight.Platform.Iam.Application.Internal.OutboundServices;
using Watchgate.Locksight.Platform.Iam.Application.QueryServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;

namespace Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService
        //,CancellationToken cancellationToken
        )
    {
        
        // Permitir acceso libre a Swagger sin pasar por autenticación
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next(context);
            return;
        }


        var endpoint = context.Request.HttpContext.GetEndpoint();
        if (endpoint == null)
        {
            await next(context);
            return;
        }

        var allowAnonymous = endpoint.Metadata
            .Any(m => m.GetType() == typeof(AllowAnonymousAttribute));

        if (allowAnonymous)
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null) throw new Exception($"Null or invalid token. Path: {context.Request.Path}");
        
        var userId = await tokenService.ValidateToken(token);
        if (userId == null) throw new Exception("Invalid token");

        var user = await userQueryService.Handle(new GetUserByIdQuery(userId.Value), context.RequestAborted);
        context.Items["User"] = user;

        await next(context);
    }
}
