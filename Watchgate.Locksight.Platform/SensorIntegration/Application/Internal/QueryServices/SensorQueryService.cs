using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SensorIntegration.Application.QueryServices;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SensorIntegration.Application.Internal.QueryServices;

public class SensorQueryService(ISensorRepository sensorRepository) : ISensorQueryService
{
    public async Task<Result<Sensor>> Handle(GetSensorByIdQuery query, CancellationToken cancellationToken = default)
    {
        var sensor = await sensorRepository.FindByIdAsync(query.SensorId, cancellationToken);
        return sensor is null
            ? Result<Sensor>.Failure(SensorIntegrationError.SensorNotFound, $"Sensor with id {query.SensorId} was not found.")
            : Result<Sensor>.Success(sensor);
    }

    public async Task<Result<IEnumerable<Sensor>>> Handle(GetSensorsByZoneIdQuery query, CancellationToken cancellationToken = default)
    {
        var sensors = await sensorRepository.FindByZoneIdAsync(query.ZoneId, cancellationToken);
        return Result<IEnumerable<Sensor>>.Success(sensors);
    }

    public async Task<Result<IEnumerable<Sensor>>> Handle(GetSensorsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var sensors = await sensorRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<Sensor>>.Success(sensors);
    }
}