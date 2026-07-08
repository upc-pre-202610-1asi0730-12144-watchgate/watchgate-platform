using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Transform;

public static class CreateSensorCommandFromResourceAssembler
{
    public static CreateSensorCommand ToCommandFromResource(CreateSensorResource resource) =>
        new(resource.Name, resource.Type, resource.Unit, resource.ZoneId, 0);
}
