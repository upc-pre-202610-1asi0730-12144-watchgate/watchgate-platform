using Watchgate.Locksight.Platform.Reporting.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Transform;

public static class ScheduleReportCommandFromResourceAssembler
{
    public static ScheduleReportCommand ToCommandFromResource(ScheduleReportResource resource) =>
        new(0, resource.WarehouseId, resource.Name, resource.Frequency, resource.Format, resource.RecipientEmail, resource.StartsAt);
}
