namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Resources;

public record CreateSensorResource(string Name, string Type, string? Unit, int ZoneId, int CompanyId);