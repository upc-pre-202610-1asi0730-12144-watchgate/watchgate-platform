namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Resources;
public record GenerateSecurityReportResource(int? WarehouseId, DateTime From, DateTime To, string Format);
