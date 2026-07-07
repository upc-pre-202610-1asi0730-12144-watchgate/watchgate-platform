using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;

namespace Watchgate.Locksight.Platform.Shared.Interfaces.Rest;

[AllowAnonymous]
[ApiController]
[Route("api/v1/health")]
[SwaggerTag("Available platform health endpoint.")]
public class HealthController(IConfiguration configuration, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get platform health",
        Description = "Returns deployment status, API version and public database configuration metadata.",
        OperationId = "GetPlatformHealth")]
    [SwaggerResponse(StatusCodes.Status200OK, "The platform is available.")]
    public IActionResult GetHealth()
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "3.0.0";

        return Ok(new
        {
            status = "Healthy",
            service = "Watchgate LockSight Platform API",
            version = assemblyVersion,
            environment = environment.EnvironmentName,
            databaseProvider = "Azure Database for MySQL",
            deployedAtUtc = DateTime.UtcNow,
            apiBasePath = "/api/v1",
            swagger = "/swagger",
            databaseSchema = configuration["DATABASE_SCHEMA"] ?? "watchgate_locksight_db"
        });
    }
}
