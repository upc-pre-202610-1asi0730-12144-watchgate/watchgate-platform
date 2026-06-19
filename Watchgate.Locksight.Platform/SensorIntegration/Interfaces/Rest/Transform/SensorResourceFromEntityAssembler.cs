using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Transform;

public static class SensorResourceFromEntityAssembler
{
    public static SensorResource ToResourceFromEntity(Sensor sensor) =>
        new(sensor.Id, sensor.Name, sensor.Type, sensor.Status, sensor.Unit,
            sensor.LastReading, sensor.LastReadingAt, sensor.ZoneId, sensor.CompanyId);
}