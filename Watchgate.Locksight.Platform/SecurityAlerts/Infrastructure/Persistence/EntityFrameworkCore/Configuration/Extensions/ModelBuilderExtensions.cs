using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySecurityAlertsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<SecurityAlert>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Type).IsRequired().HasMaxLength(50);
            entity.Property(a => a.Severity).IsRequired().HasMaxLength(20).HasDefaultValue("LOW");
            entity.Property(a => a.Status).IsRequired().HasMaxLength(20).HasDefaultValue("OPEN");
            entity.Property(a => a.Description).IsRequired().HasMaxLength(500);
            entity.Property(a => a.TriggeredAt).IsRequired();
            entity.Property(a => a.ResolvedAt);
        });

        builder.Entity<AlertIncident>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Title).IsRequired().HasMaxLength(200);
            entity.Property(i => i.Description).IsRequired().HasMaxLength(1000);
            entity.Property(i => i.Status).IsRequired().HasMaxLength(20).HasDefaultValue("OPEN");
            entity.Property(i => i.Priority).IsRequired().HasMaxLength(20).HasDefaultValue("MEDIUM");
            entity.Property(i => i.CreatedAt).IsRequired();
            entity.Property(i => i.ClosedAt);
            entity.HasMany(i => i.RelatedAlerts).WithMany()
                .UsingEntity("alert_incident_security_alerts");
        });
    }
}