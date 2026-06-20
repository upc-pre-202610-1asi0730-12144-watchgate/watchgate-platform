using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        // User
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id)
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.FullName).IsRequired().HasMaxLength(150);
        builder.Entity<User>().Property(u => u.Email)
            .HasConversion(email => email.Value, value => new EmailAddress(value))
            .IsRequired().HasMaxLength(200);
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        builder.Entity<User>().Property(u => u.Role).HasMaxLength(50).HasDefaultValue("Visitor");
        builder.Entity<User>().Property(u => u.CompanyId)
            .HasConversion(id => id.Value, value => new CompanyId(value))
            .IsRequired();
        builder.Entity<User>().HasOne(u => u.Company).WithMany().HasForeignKey(u => u.CompanyId);
        builder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // Company
        builder.Entity<Company>().HasKey(c => c.Id);
        builder.Entity<Company>().Property(c => c.Id)
            .HasConversion(id => id.Value, value => new CompanyId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Company>().Property(c => c.TradeName).IsRequired().HasMaxLength(200);
        builder.Entity<Company>().Property(c => c.TaxId).HasMaxLength(20);
    }
}
