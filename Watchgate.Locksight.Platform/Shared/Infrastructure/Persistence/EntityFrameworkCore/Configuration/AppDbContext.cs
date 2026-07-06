using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.CompanyRegistration.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.SensorIntegration.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Watchgate.Locksight.Platform.WarehouseManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>Application database context for the Watchgate Locksight Platform.</summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyIamConfiguration();
        builder.ApplyCompanyRegistrationConfiguration();
        builder.ApplyWarehouseManagementConfiguration();
        builder.ApplySensorIntegrationConfiguration();
        builder.ApplySubscriptionManagementConfiguration();
        builder.ApplyReportingConfiguration();
        builder.ApplySecurityAlertsConfiguration();
        builder.UseSnakeCaseNamingConvention();
    }
}
