namespace Watchgate.Locksight.Platform.Reporting.Domain.Model;

public enum ReportingError
{
    None,
    ReportNotFound,
    ScheduledReportNotFound,
    InvalidReportRange,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}