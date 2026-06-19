namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Resources;

public record SensorResource(
    int Id,
    string Name,
    string Type,
    string Status,
    string? Unit,
    double? LastReading,
    DateTime? LastReadingAt,
    int ZoneId,
    int CompanyId);