using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Queries;

namespace Watchgate.Locksight.Platform.SensorIntegration.Application.QueryServices;

public interface ISensorQueryService
{
    Task<Result<Sensor>> Handle(GetSensorByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Sensor>>> Handle(GetSensorsByZoneIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Sensor>>> Handle(GetSensorsByCompanyIdQuery query, CancellationToken cancellationToken = default);
}