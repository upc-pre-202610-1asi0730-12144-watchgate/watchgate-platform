using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.Model;
using Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Transform;

public static class WarehouseResourceFromEntityAssembler
{
    public static WarehouseZoneResource ToZoneResourceFromEntity(WarehouseZone zone) =>
        new(zone.Id, zone.Name, zone.Area, zone.RiskLevel, zone.WarehouseId);

    public static WarehouseResource ToResourceFromEntity(Warehouse warehouse) =>
        new(warehouse.Id, warehouse.Name, warehouse.Location, warehouse.Capacity,
            warehouse.Status,
            warehouse.OperationStart?.ToString(@"hh\:mm"),
            warehouse.OperationEnd?.ToString(@"hh\:mm"),
            warehouse.CompanyId,
            warehouse.Zones.Select(ToZoneResourceFromEntity));

    public static WarehouseDashboardResource ToDashboardResourceFromEntity(WarehouseDashboard dashboard) =>
        new(dashboard.CompanyId, dashboard.TotalWarehouses, dashboard.ActiveWarehouses,
            dashboard.InactiveWarehouses, dashboard.TotalZones, dashboard.HighRiskZones,
            dashboard.MediumRiskZones, dashboard.LowRiskZones);
}
