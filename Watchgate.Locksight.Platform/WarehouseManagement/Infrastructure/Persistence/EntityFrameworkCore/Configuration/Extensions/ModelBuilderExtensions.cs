using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyWarehouseManagementConfiguration(this ModelBuilder builder)
    {
        // Warehouse
        builder.Entity<Warehouse>().HasKey(w => w.Id);
        builder.Entity<Warehouse>().Property(w => w.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Warehouse>().Property(w => w.Name).IsRequired().HasMaxLength(200);
        builder.Entity<Warehouse>().Property(w => w.Location).HasMaxLength(300);
        builder.Entity<Warehouse>().Property(w => w.Capacity).IsRequired();
        builder.Entity<Warehouse>().Property(w => w.Status).HasMaxLength(20).HasDefaultValue("ACTIVE");
        builder.Entity<Warehouse>().Property(w => w.CompanyId).IsRequired();
        builder.Entity<Warehouse>().HasMany(w => w.Zones).WithOne().HasForeignKey(z => z.WarehouseId);

        // WarehouseZone
        builder.Entity<WarehouseZone>().HasKey(z => z.Id);
        builder.Entity<WarehouseZone>().Property(z => z.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<WarehouseZone>().Property(z => z.Name).IsRequired().HasMaxLength(200);
        builder.Entity<WarehouseZone>().Property(z => z.Area).IsRequired();
        builder.Entity<WarehouseZone>().Property(z => z.RiskLevel).HasMaxLength(20).HasDefaultValue("LOW");
        builder.Entity<WarehouseZone>().Property(z => z.WarehouseId).IsRequired();
    }
}
