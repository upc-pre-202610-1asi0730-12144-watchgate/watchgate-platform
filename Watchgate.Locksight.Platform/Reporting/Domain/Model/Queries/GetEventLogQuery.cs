namespace Watchgate.Locksight.Platform.Reporting.Domain.Model.Queries;
public record GetEventLogQuery(int CompanyId, DateTime? From, DateTime? To, string? Type, int? ZoneId, int? WarehouseId);