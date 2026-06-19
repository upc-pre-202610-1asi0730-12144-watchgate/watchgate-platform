namespace Watchgate.Locksight.Platform.Iam.Domain.Model;

public enum IamError
{
    None,
    UserNotFound,
    EmailAlreadyRegistered,
    InvalidCredentials,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
