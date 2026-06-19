namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;

public record UpdateWarehouseResource(
    string Name,
    string Location,
    int Capacity,
    string? OperationStart,
    string? OperationEnd);
