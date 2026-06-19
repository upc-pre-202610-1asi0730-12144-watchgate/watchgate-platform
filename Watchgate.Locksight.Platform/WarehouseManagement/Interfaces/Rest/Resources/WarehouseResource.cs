namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;

public record WarehouseZoneResource(int Id, string Name, double Area, string RiskLevel, int WarehouseId);

public record WarehouseResource(int Id, string Name, string Location, int Capacity,
    string Status, string? OperationStart, string? OperationEnd, int CompanyId,
    IEnumerable<WarehouseZoneResource> Zones);
