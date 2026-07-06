using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;

namespace Watchgate.Locksight.Platform.SensorIntegration.Application.CommandServices;

public interface ISensorCommandService
{
    Task<Result<Sensor>> Handle(CreateSensorCommand command, CancellationToken cancellationToken = default);
    Task<Result<Sensor>> Handle(UpdateSensorStatusCommand command, CancellationToken cancellationToken = default);
    Task<Result<Sensor>> Handle(RecordSensorReadingCommand command, CancellationToken cancellationToken = default);
    Task<Result<Sensor>> Handle(UnlinkSensorCommand command, CancellationToken cancellationToken = default);
}
