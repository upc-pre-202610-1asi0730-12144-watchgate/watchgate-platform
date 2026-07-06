namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model;

public enum CompanyRegistrationError
{
    None,
    CompanyAccountNotFound,
    CompanyAccountAlreadyExists,
    InvalidVerificationCode,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
