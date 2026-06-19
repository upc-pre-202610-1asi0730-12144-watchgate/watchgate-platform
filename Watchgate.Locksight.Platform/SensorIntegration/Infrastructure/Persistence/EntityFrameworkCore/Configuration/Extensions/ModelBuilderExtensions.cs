using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.SensorIntegration.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySensorIntegrationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Sensor>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Type).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Status).IsRequired().HasMaxLength(20).HasDefaultValue("ACTIVE");
            entity.Property(s => s.Unit).HasMaxLength(20);
            entity.Property(s => s.LastReading);
            entity.Property(s => s.LastReadingAt);
        });
    }
}