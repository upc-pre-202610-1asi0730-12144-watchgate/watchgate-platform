namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model;

public enum SubscriptionError
{
    None,
    InvalidCardNumber,
    CompanyIdRequired,
    AlreadyCanceled,
    CannotChangeCanceledPlan,
    AlreadyOnThisPlan,
    SubscriptionNotFound,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}