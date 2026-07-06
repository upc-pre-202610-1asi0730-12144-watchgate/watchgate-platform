using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Reporting.Domain.Model;
using Watchgate.Locksight.Platform.Reporting.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Entities;

namespace Watchgate.Locksight.Platform.Reporting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class EventLogRepository(AppDbContext context) : IEventLogRepository
{
    public async Task<IEnumerable<EventLogEntry>> FindEventsAsync(
        int companyId,
        DateTime from,
        DateTime to,
        string? type,
        int? zoneId,
        int? warehouseId,
        CancellationToken cancellationToken = default)
    {
        var query = context.Set<SecurityAlert>().AsNoTracking()
            .Where(alert => alert.CompanyId == companyId && alert.TriggeredAt >= from && alert.TriggeredAt <= to);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(alert => alert.Type == type);

        if (zoneId.HasValue)
        {
            var sensorIds = await context.Set<Sensor>().AsNoTracking()
                .Where(sensor => sensor.ZoneId == zoneId.Value)
                .Select(sensor => (int)sensor.Id)
                .ToListAsync(cancellationToken);
            query = query.Where(alert => sensorIds.Contains(alert.SensorId));
        }

        if (warehouseId.HasValue)
        {
            var zoneIds = await context.Set<WarehouseZone>().AsNoTracking()
                .Where(zone => zone.WarehouseId == warehouseId.Value)
                .Select(zone => zone.Id)
                .ToListAsync(cancellationToken);
            var sensorIds = await context.Set<Sensor>().AsNoTracking()
                .Where(sensor => zoneIds.Contains(sensor.ZoneId))
                .Select(sensor => (int)sensor.Id)
                .ToListAsync(cancellationToken);
            query = query.Where(alert => sensorIds.Contains(alert.SensorId));
        }

        return await query.OrderByDescending(alert => alert.TriggeredAt)
            .Select(alert => new EventLogEntry(
                alert.Id,
                alert.Type,
                alert.Severity,
                alert.Status,
                alert.Description,
                alert.SensorId,
                alert.CompanyId,
                alert.TriggeredAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<ReportingDashboard> GetDashboardAsync(int companyId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var events = await context.Set<SecurityAlert>().AsNoTracking()
            .Where(alert => alert.CompanyId == companyId && alert.TriggeredAt >= from && alert.TriggeredAt <= to)
            .ToListAsync(cancellationToken);

        return new ReportingDashboard(
            companyId,
            from,
            to,
            events.Count,
            events.Count(e => e.Status.Equals("OPEN", StringComparison.OrdinalIgnoreCase)),
            events.Count(e => e.Status.Equals("RESOLVED", StringComparison.OrdinalIgnoreCase)),
            events.Count(e => e.Severity.Equals("CRITICAL", StringComparison.OrdinalIgnoreCase)),
            events.Count(e => e.Severity.Equals("HIGH", StringComparison.OrdinalIgnoreCase)),
            events.Count(e => e.Severity.Equals("MEDIUM", StringComparison.OrdinalIgnoreCase)),
            events.Count(e => e.Severity.Equals("LOW", StringComparison.OrdinalIgnoreCase)));
    }
}