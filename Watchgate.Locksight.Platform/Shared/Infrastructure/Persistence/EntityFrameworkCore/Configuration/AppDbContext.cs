using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.SensorIntegration.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.WarehouseManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>Application database context for the Watchgate Locksight Platform.</summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyIamConfiguration();
        builder.ApplyWarehouseManagementConfiguration();
        builder.ApplySensorIntegrationConfiguration();
        builder.ApplySecurityAlertsConfiguration();
        builder.UseSnakeCaseNamingConvention();
    }
}
