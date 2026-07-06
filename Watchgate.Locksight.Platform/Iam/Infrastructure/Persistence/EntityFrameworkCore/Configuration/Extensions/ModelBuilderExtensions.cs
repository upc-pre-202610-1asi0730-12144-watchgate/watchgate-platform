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

        // User Access Management
        builder.Entity<UserInvitation>().HasKey(invitation => invitation.Id);
        builder.Entity<UserInvitation>().Property(invitation => invitation.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<UserInvitation>().Property(invitation => invitation.CompanyId).IsRequired();
        builder.Entity<UserInvitation>().Property(invitation => invitation.Email).IsRequired().HasMaxLength(200);
        builder.Entity<UserInvitation>().Property(invitation => invitation.Role).IsRequired().HasMaxLength(50);
        builder.Entity<UserInvitation>().Property(invitation => invitation.Permissions).HasMaxLength(500);
        builder.Entity<UserInvitation>().Property(invitation => invitation.Token).IsRequired().HasMaxLength(80);
        builder.Entity<UserInvitation>().Property(invitation => invitation.Status).IsRequired().HasMaxLength(20);
        builder.Entity<UserInvitation>().Property(invitation => invitation.ExpiresAt).IsRequired();
        builder.Entity<UserInvitation>().HasIndex(invitation => invitation.Token).IsUnique();

        builder.Entity<UserAccessProfile>().HasKey(profile => profile.Id);
        builder.Entity<UserAccessProfile>().Property(profile => profile.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<UserAccessProfile>().Property(profile => profile.UserId).IsRequired();
        builder.Entity<UserAccessProfile>().Property(profile => profile.CompanyId).IsRequired();
        builder.Entity<UserAccessProfile>().Property(profile => profile.Role).IsRequired().HasMaxLength(50);
        builder.Entity<UserAccessProfile>().Property(profile => profile.Permissions).HasMaxLength(500);
        builder.Entity<UserAccessProfile>().Property(profile => profile.Status).IsRequired().HasMaxLength(20);
        builder.Entity<UserAccessProfile>().HasIndex(profile => profile.UserId).IsUnique();
    }
}
