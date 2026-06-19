namespace Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;
 
public record RecordSensorReadingCommand(int SensorId, double Value);