namespace Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model;

public enum SecurityAlertsError
{
    None,
    AlertNotFound,
    IncidentNotFound,
    SensorNotFound,
    CompanyNotFound,
    InvalidAlertData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}