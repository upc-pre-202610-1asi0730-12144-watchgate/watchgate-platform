namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;

public record CreateWarehouseZoneResource(string Name, double Area, string RiskLevel = "LOW");
