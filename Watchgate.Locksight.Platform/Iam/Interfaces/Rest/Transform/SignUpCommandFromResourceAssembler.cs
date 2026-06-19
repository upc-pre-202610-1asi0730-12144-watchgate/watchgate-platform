using Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource) =>
        new(resource.FullName, resource.Email, resource.Password, resource.TradeName, resource.TaxId);
}
