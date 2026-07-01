using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySubscriptionManagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Subscription>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .HasConversion(v => v.Value, v => new SubscriptionId(v))
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(s => s.CompanyId).IsRequired();
            entity.Property(s => s.StartDate).IsRequired();
            entity.Property(s => s.NextBillingDate).IsRequired();
            entity.Property(s => s.IsActive).IsRequired();
            
            entity.Property(s => s.Tier)
                .HasConversion<string>()
                .IsRequired();
            
            entity.Property(s => s.PaymentMethod)
                .HasConversion(v => v.Value, v => new CardNumber(v))
                .HasMaxLength(16)
                .IsRequired();
        });
    }
}