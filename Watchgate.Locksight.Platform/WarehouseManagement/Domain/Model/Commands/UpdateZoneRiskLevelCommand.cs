namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
public record UpdateZoneRiskLevelCommand(int WarehouseId, int ZoneId, string RiskLevel);