namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
public record CreateWarehouseCommand(string Name, string Location, int Capacity, int CompanyId, string? OperationStart, string? OperationEnd);
