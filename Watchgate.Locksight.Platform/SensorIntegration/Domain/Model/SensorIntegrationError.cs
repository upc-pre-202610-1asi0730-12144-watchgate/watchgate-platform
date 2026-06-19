namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Model;
 
public enum SensorIntegrationError
{
    None,
    SensorNotFound,
    SensorAlreadyExists,
    WarehouseZoneNotFound,
    CompanyNotFound,
    InvalidSensorData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}