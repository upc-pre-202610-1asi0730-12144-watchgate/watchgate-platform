namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;

public enum SubscriptionManagementError
{
    None,
    PlanNotFound,
    SubscriptionNotFound,
    InvoiceNotFound,
    ActiveSubscriptionAlreadyExists,
    InvalidSubscriptionData,
    PaymentFailed,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
