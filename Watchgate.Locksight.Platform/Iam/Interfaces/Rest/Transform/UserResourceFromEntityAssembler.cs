using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user) =>
        new(user.Id, user.FullName, user.Email, user.Role, user.CompanyId);
}
