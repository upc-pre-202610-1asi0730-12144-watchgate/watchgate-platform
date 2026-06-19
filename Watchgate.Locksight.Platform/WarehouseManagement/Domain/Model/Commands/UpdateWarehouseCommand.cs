namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
public record UpdateWarehouseCommand(int WarehouseId, string Name, string Location, int Capacity, string? OperationStart, string? OperationEnd);
