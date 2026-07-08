using Watchgate.Locksight.Platform.Reporting.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Transform;

public static class GenerateSecurityReportCommandFromResourceAssembler
{
    public static GenerateSecurityReportCommand ToCommandFromResource(GenerateSecurityReportResource resource) =>
        new(0, resource.WarehouseId, resource.From, resource.To, resource.Format);
}
