namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;

public record CreateSensorCommand(string Name, string Type, string? Unit, int ZoneId, int CompanyId);