using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyReportingConfiguration(this ModelBuilder builder)
    {
        builder.Entity<SecurityReport>().HasKey(report => report.Id);
        builder.Entity<SecurityReport>().Property(report => report.Id)
            .HasConversion(id => id.Value, value => new SecurityReportId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SecurityReport>().Property(report => report.Title).IsRequired().HasMaxLength(200);
        builder.Entity<SecurityReport>().Property(report => report.Format).IsRequired().HasMaxLength(20).HasDefaultValue("PDF");
        builder.Entity<SecurityReport>().Property(report => report.Status).IsRequired().HasMaxLength(20).HasDefaultValue("GENERATED");
        builder.Entity<SecurityReport>().Property(report => report.GeneratedAt).IsRequired();

        builder.Entity<ScheduledReport>().HasKey(report => report.Id);
        builder.Entity<ScheduledReport>().Property(report => report.Id)
            .HasConversion(id => id.Value, value => new ScheduledReportId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ScheduledReport>().Property(report => report.Name).IsRequired().HasMaxLength(150);
        builder.Entity<ScheduledReport>().Property(report => report.Frequency).IsRequired().HasMaxLength(20).HasDefaultValue("WEEKLY");
        builder.Entity<ScheduledReport>().Property(report => report.Format).IsRequired().HasMaxLength(20).HasDefaultValue("PDF");
        builder.Entity<ScheduledReport>().Property(report => report.RecipientEmail).IsRequired().HasMaxLength(200);
        builder.Entity<ScheduledReport>().Property(report => report.IsActive).HasDefaultValue(true);
        builder.Entity<ScheduledReport>().Property(report => report.StartsAt).IsRequired();
    }
}