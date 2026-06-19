using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.SensorIntegration.Application.CommandServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.QueryServices;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Transform;

namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SensorsController(
    ISensorCommandService sensorCommandService,
    ISensorQueryService sensorQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSensor(
        [FromBody] CreateSensorResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateSensorCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => CreatedAtAction(nameof(GetSensorById),
                new { sensorId = sensor.Id },
                SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }

    [HttpGet("{sensorId:int}")]
    public async Task<IActionResult> GetSensorById(int sensorId, CancellationToken cancellationToken)
    {
        var query = new GetSensorByIdQuery(sensorId);
        var result = await sensorQueryService.Handle(query, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }

    [HttpGet("zone/{zoneId:int}")]
    public async Task<IActionResult> GetSensorsByZoneId(int zoneId, CancellationToken cancellationToken)
    {
        var query = new GetSensorsByZoneIdQuery(zoneId);
        var result = await sensorQueryService.Handle(query, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensors => Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("company/{companyId:int}")]
    public async Task<IActionResult> GetSensorsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var query = new GetSensorsByCompanyIdQuery(companyId);
        var result = await sensorQueryService.Handle(query, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensors => Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpPatch("{sensorId:int}/status")]
    public async Task<IActionResult> UpdateSensorStatus(
        int sensorId,
        [FromBody] UpdateSensorStatusResource resource,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSensorStatusCommand(sensorId, resource.Status);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }

    [HttpPost("{sensorId:int}/readings")]
    public async Task<IActionResult> RecordSensorReading(
        int sensorId,
        [FromBody] RecordSensorReadingResource resource,
        CancellationToken cancellationToken)
    {
        var command = new RecordSensorReadingCommand(sensorId, resource.Value);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }
}
