using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySubscriptionManagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<SubscriptionPlan>().HasKey(plan => plan.Id);
        builder.Entity<SubscriptionPlan>().Property(plan => plan.Id)
            .HasConversion(id => id.Value, value => new SubscriptionPlanId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SubscriptionPlan>().Property(plan => plan.Name).IsRequired().HasMaxLength(100);
        builder.Entity<SubscriptionPlan>().Property(plan => plan.Description).IsRequired().HasMaxLength(500);
        builder.Entity<SubscriptionPlan>().Property(plan => plan.MonthlyPrice).HasPrecision(10, 2).IsRequired();
        builder.Entity<SubscriptionPlan>().Property(plan => plan.MaxWarehouses).IsRequired();
        builder.Entity<SubscriptionPlan>().Property(plan => plan.MaxSensors).IsRequired();
        builder.Entity<SubscriptionPlan>().Property(plan => plan.IsActive).HasDefaultValue(true);
        builder.Entity<SubscriptionPlan>().HasData(
            new
            {
                Id = new SubscriptionPlanId(1),
                Name = "Starter",
                Description = "Basic monitoring for small warehouses.",
                MonthlyPrice = 49.00m,
                MaxWarehouses = 1,
                MaxSensors = 10,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = new SubscriptionPlanId(2),
                Name = "Business",
                Description = "Advanced monitoring for growing warehouse operations.",
                MonthlyPrice = 99.00m,
                MaxWarehouses = 5,
                MaxSensors = 50,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = new SubscriptionPlanId(3),
                Name = "Enterprise",
                Description = "Multi-site monitoring for enterprise security teams.",
                MonthlyPrice = 199.00m,
                MaxWarehouses = 20,
                MaxSensors = 250,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });

        builder.Entity<Subscription>().HasKey(subscription => subscription.Id);
        builder.Entity<Subscription>().Property(subscription => subscription.Id)
            .HasConversion(id => id.Value, value => new SubscriptionId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Subscription>().Property(subscription => subscription.CompanyId).IsRequired();
        builder.Entity<Subscription>().Property(subscription => subscription.PlanId)
            .HasConversion(id => id.Value, value => new SubscriptionPlanId(value))
            .IsRequired();
        builder.Entity<Subscription>().Property(subscription => subscription.Status).IsRequired().HasMaxLength(20).HasDefaultValue("ACTIVE");
        builder.Entity<Subscription>().Property(subscription => subscription.StartedAt).IsRequired();
        builder.Entity<Subscription>().HasOne(subscription => subscription.Plan)
            .WithMany()
            .HasForeignKey(subscription => subscription.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Payment>().HasKey(payment => payment.Id);
        builder.Entity<Payment>().Property(payment => payment.Id)
            .HasConversion(id => id.Value, value => new PaymentId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Payment>().Property(payment => payment.SubscriptionId)
            .HasConversion(id => id.Value, value => new SubscriptionId(value))
            .IsRequired();
        builder.Entity<Payment>().Property(payment => payment.CompanyId).IsRequired();
        builder.Entity<Payment>().Property(payment => payment.Amount).HasPrecision(10, 2).IsRequired();
        builder.Entity<Payment>().Property(payment => payment.Currency).IsRequired().HasMaxLength(3);
        builder.Entity<Payment>().Property(payment => payment.Provider).IsRequired().HasMaxLength(40);
        builder.Entity<Payment>().Property(payment => payment.ProviderReference).IsRequired().HasMaxLength(120);
        builder.Entity<Payment>().Property(payment => payment.Status).IsRequired().HasMaxLength(20);
        builder.Entity<Payment>().Property(payment => payment.RequestedAt).IsRequired();
        builder.Entity<Payment>().HasOne(payment => payment.Subscription)
            .WithMany()
            .HasForeignKey(payment => payment.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Invoice>().HasKey(invoice => invoice.Id);
        builder.Entity<Invoice>().Property(invoice => invoice.Id)
            .HasConversion(id => id.Value, value => new InvoiceId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Invoice>().Property(invoice => invoice.PaymentId)
            .HasConversion(id => id.Value, value => new PaymentId(value))
            .IsRequired();
        builder.Entity<Invoice>().Property(invoice => invoice.SubscriptionId)
            .HasConversion(id => id.Value, value => new SubscriptionId(value))
            .IsRequired();
        builder.Entity<Invoice>().Property(invoice => invoice.CompanyId).IsRequired();
        builder.Entity<Invoice>().Property(invoice => invoice.Number).IsRequired().HasMaxLength(40);
        builder.Entity<Invoice>().Property(invoice => invoice.Amount).HasPrecision(10, 2).IsRequired();
        builder.Entity<Invoice>().Property(invoice => invoice.Currency).IsRequired().HasMaxLength(3);
        builder.Entity<Invoice>().Property(invoice => invoice.Status).IsRequired().HasMaxLength(20);
        builder.Entity<Invoice>().Property(invoice => invoice.IssuedAt).IsRequired();
        builder.Entity<Invoice>().HasIndex(invoice => invoice.Number).IsUnique();
        builder.Entity<Invoice>().HasOne(invoice => invoice.Payment)
            .WithMany()
            .HasForeignKey(invoice => invoice.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Invoice>().HasOne(invoice => invoice.Subscription)
            .WithMany()
            .HasForeignKey(invoice => invoice.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
