namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
public record CreateWarehouseZoneCommand(string Name, double Area, int WarehouseId, string RiskLevel = "LOW");
