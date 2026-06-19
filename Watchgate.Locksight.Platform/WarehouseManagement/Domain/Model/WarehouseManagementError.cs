namespace Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model;

public enum WarehouseManagementError
{
    None,
    WarehouseNotFound,
    ZoneNotFound,
    CompanyNotFound,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
