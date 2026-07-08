namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model;

public enum WarehouseManagementError
{
    None,
    WarehouseNotFound,
    ZoneNotFound,
    ZoneAreaExceedsWarehouseCapacity,
    CompanyNotFound,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
