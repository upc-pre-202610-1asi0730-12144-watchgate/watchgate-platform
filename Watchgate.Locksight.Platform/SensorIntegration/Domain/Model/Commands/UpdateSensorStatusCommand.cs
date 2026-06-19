namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;

public record UpdateSensorStatusCommand(int SensorId, string Status);