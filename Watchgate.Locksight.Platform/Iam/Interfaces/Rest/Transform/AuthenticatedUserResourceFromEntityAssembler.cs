using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token) =>
        new(user.Id, user.FullName, user.Email, user.Role, token);
}
