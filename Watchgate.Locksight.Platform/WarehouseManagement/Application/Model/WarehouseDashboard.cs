namespace Watchgate.Locksight.Platform.WarehouseManagement.Application.Model;

public record WarehouseDashboard(
    int CompanyId,
    int TotalWarehouses,
    int ActiveWarehouses,
    int InactiveWarehouses,
    int TotalZones,
    int HighRiskZones,
    int MediumRiskZones,
    int LowRiskZones);
