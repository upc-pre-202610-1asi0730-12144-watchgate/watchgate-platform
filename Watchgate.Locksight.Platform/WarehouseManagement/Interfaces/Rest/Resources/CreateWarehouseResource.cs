namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;

public record CreateWarehouseResource(string Name, string Location, int Capacity, int CompanyId,
    string? OperationStart, string? OperationEnd);
