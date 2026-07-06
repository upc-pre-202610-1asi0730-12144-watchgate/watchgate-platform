namespace Watchgate.Locksight.Platform.Reporting.Domain.Model.Commands;
public record GenerateSecurityReportCommand(int CompanyId, int? WarehouseId, DateTime From, DateTime To, string Format);