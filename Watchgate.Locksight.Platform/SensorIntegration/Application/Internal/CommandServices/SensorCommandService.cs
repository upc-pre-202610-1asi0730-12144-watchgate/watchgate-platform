using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SensorIntegration.Application.CommandServices;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SensorIntegration.Application.Internal.CommandServices;

public class SensorCommandService(
    ISensorRepository sensorRepository,
    IUnitOfWork unitOfWork) : ISensorCommandService
{
    public async Task<Result<Sensor>> Handle(CreateSensorCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var sensor = new Sensor(command.Name, command.Type, command.Unit, command.ZoneId, command.CompanyId);
            await sensorRepository.AddAsync(sensor, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (OperationCanceledException)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.DatabaseError, "A database error occurred while creating the sensor.");
        }
        catch (Exception)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<Sensor>> Handle(UpdateSensorStatusCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
            if (sensor is null)
                return Result<Sensor>.Failure(SensorIntegrationError.SensorNotFound, $"Sensor with id {command.SensorId} was not found.");

            sensor.UpdateStatus(command.Status);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (OperationCanceledException)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<Sensor>> Handle(RecordSensorReadingCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var sensor = await sensorRepository.FindByIdAsync(command.SensorId, cancellationToken);
            if (sensor is null)
                return Result<Sensor>.Failure(SensorIntegrationError.SensorNotFound, $"Sensor with id {command.SensorId} was not found.");

            sensor.RecordReading(command.Value);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Sensor>.Success(sensor);
        }
        catch (OperationCanceledException)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<Sensor>.Failure(SensorIntegrationError.InternalServerError, "An unexpected error occurred.");
        }
    }
}
