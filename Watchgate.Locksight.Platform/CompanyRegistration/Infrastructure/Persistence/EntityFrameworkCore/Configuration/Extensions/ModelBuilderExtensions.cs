using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyCompanyRegistrationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<CompanyAccount>().HasKey(account => account.Id);
        builder.Entity<CompanyAccount>().Property(account => account.Id)
            .HasConversion(id => id.Value, value => new CompanyAccountId(value))
            .IsRequired().ValueGeneratedOnAdd();
        builder.Entity<CompanyAccount>().Property(account => account.CompanyId).IsRequired();
        builder.Entity<CompanyAccount>().Property(account => account.TradeName).IsRequired().HasMaxLength(200);
        builder.Entity<CompanyAccount>().Property(account => account.TaxId).IsRequired().HasMaxLength(20);
        builder.Entity<CompanyAccount>().Property(account => account.LegalName).HasMaxLength(200);
        builder.Entity<CompanyAccount>().Property(account => account.Industry).HasMaxLength(100);
        builder.Entity<CompanyAccount>().Property(account => account.ContactPhone).HasMaxLength(50);
        builder.Entity<CompanyAccount>().Property(account => account.Address).HasMaxLength(300);
        builder.Entity<CompanyAccount>().Property(account => account.WebsiteUrl).HasMaxLength(300);
        builder.Entity<CompanyAccount>().Property(account => account.Status).IsRequired().HasMaxLength(20).HasDefaultValue("ACTIVE");
        builder.Entity<CompanyAccount>().Property(account => account.EmailVerificationCode).HasMaxLength(100);
        builder.Entity<CompanyAccount>().HasIndex(account => account.CompanyId).IsUnique();
    }
}
