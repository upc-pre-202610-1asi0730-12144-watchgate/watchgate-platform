namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;

public record WarehouseDashboardResource(
    int CompanyId,
    int TotalWarehouses,
    int ActiveWarehouses,
    int InactiveWarehouses,
    int TotalZones,
    int HighRiskZones,
    int MediumRiskZones,
    int LowRiskZones);
