namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Resources;
public record ScheduleReportResource(int? WarehouseId, string Name, string Frequency, string Format, string RecipientEmail, DateTime StartsAt);
