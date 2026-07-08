using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.SensorIntegration.Application.CommandServices;
using Watchgate.Locksight.Platform.SensorIntegration.Application.QueryServices;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SensorIntegration.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest.Transform;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.Extensions;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Queries;

namespace Watchgate.Locksight.Platform.SensorIntegration.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Sensor Integration endpoints.")]
public class SensorsController(
    ISensorCommandService sensorCommandService,
    ISensorQueryService sensorQueryService,
    IWarehouseQueryService warehouseQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a sensor", Description = "Registers an IoT sensor and links it to a warehouse zone.", OperationId = "CreateSensor")]
    [SwaggerResponse(StatusCodes.Status201Created, "The sensor was created.", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The sensor could not be created.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CreateSensor([FromBody] CreateSensorResource resource, CancellationToken cancellationToken)
    {
        var companyId = HttpContext.CurrentCompanyId();
        if (companyId is null) return Unauthorized();
        if (!await CanAccessZone(resource.ZoneId, cancellationToken)) return Forbid();

        var command = new CreateSensorCommand(resource.Name, resource.Type, resource.Unit, resource.ZoneId, companyId.Value);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => CreatedAtAction(nameof(GetSensorById), new { sensorId = sensor.Id }, SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }

    [HttpGet("{sensorId:int}")]
    [SwaggerOperation(Summary = "Get sensor by ID", Description = "Gets sensor details by identifier.", OperationId = "GetSensorById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The sensor was retrieved.", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The sensor was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetSensorById(int sensorId, CancellationToken cancellationToken)
    {
        var query = new GetSensorByIdQuery(sensorId);
        var result = await sensorQueryService.Handle(query, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor =>
            {
                if (!BelongsToCurrentCompany(sensor.CompanyId)) return Forbid();
                return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor));
            });
    }

    [HttpGet("zone/{zoneId:int}")]
    [SwaggerOperation(Summary = "Get sensors by zone", Description = "Lists sensors linked to a warehouse zone.", OperationId = "GetSensorsByZoneId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The sensors were retrieved.", typeof(IEnumerable<SensorResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetSensorsByZoneId(int zoneId, CancellationToken cancellationToken)
    {
        if (!await CanAccessZone(zoneId, cancellationToken)) return Forbid();

        var query = new GetSensorsByZoneIdQuery(zoneId);
        var result = await sensorQueryService.Handle(query, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensors => Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get sensors by company", Description = "Lists all sensors registered for a company.", OperationId = "GetSensorsByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The sensors were retrieved.", typeof(IEnumerable<SensorResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetSensorsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var currentCompanyId = HttpContext.CurrentCompanyId();
        if (currentCompanyId is null) return Unauthorized();
        if (companyId != currentCompanyId.Value) return Forbid();

        var query = new GetSensorsByCompanyIdQuery(currentCompanyId.Value);
        var result = await sensorQueryService.Handle(query, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensors => Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpPatch("{sensorId:int}/status")]
    [SwaggerOperation(Summary = "Update sensor status", Description = "Changes the operational status of a sensor.", OperationId = "UpdateSensorStatus")]
    [SwaggerResponse(StatusCodes.Status200OK, "The sensor status was updated.", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The sensor was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> UpdateSensorStatus(int sensorId, [FromBody] UpdateSensorStatusResource resource, CancellationToken cancellationToken)
    {
        if (!await CanAccessSensor(sensorId, cancellationToken)) return Forbid();

        var command = new UpdateSensorStatusCommand(sensorId, resource.Status);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }

    [HttpPost("{sensorId:int}/readings")]
    [SwaggerOperation(Summary = "Record sensor reading", Description = "Stores the latest reading received from a sensor.", OperationId = "RecordSensorReading")]
    [SwaggerResponse(StatusCodes.Status200OK, "The sensor reading was recorded.", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The sensor was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> RecordSensorReading(int sensorId, [FromBody] RecordSensorReadingResource resource, CancellationToken cancellationToken)
    {
        if (!await CanAccessSensor(sensorId, cancellationToken)) return Forbid();

        var command = new RecordSensorReadingCommand(sensorId, resource.Value);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }

    [HttpPatch("{sensorId:int}/unlink")]
    [SwaggerOperation(Summary = "Unlink sensor", Description = "Unlinks a sensor from active monitoring without deleting its historical data.", OperationId = "UnlinkSensor")]
    [SwaggerResponse(StatusCodes.Status200OK, "The sensor was unlinked.", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The sensor was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> UnlinkSensor(int sensorId, CancellationToken cancellationToken)
    {
        if (!await CanAccessSensor(sensorId, cancellationToken)) return Forbid();

        var result = await sensorCommandService.Handle(new UnlinkSensorCommand(sensorId), cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            sensor => Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(sensor)));
    }

    [HttpDelete("{sensorId:int}")]
    [SwaggerOperation(Summary = "Delete sensor", Description = "Deletes an IoT sensor. Only administrators can perform this action.", OperationId = "DeleteSensor")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The sensor was deleted.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "The current user cannot delete this sensor.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The sensor was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> DeleteSensor(int sensorId, CancellationToken cancellationToken)
    {
        if (!HttpContext.IsCurrentUserAdministrator()) return Forbid();
        if (!await CanAccessSensor(sensorId, cancellationToken)) return Forbid();

        var result = await sensorCommandService.Handle(new DeleteSensorCommand(sensorId), cancellationToken);
        return SensorActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, () => NoContent());
    }

    private bool BelongsToCurrentCompany(int companyId) => HttpContext.CurrentCompanyId() == companyId;

    private async Task<bool> CanAccessSensor(int sensorId, CancellationToken cancellationToken)
    {
        var companyId = HttpContext.CurrentCompanyId();
        if (companyId is null) return false;

        var result = await sensorQueryService.Handle(new GetSensorByIdQuery(sensorId), cancellationToken);
        return result.IsSuccess && result.Value!.CompanyId == companyId.Value;
    }

    private async Task<bool> CanAccessZone(int zoneId, CancellationToken cancellationToken)
    {
        var companyId = HttpContext.CurrentCompanyId();
        if (companyId is null) return false;

        var result = await warehouseQueryService.Handle(new GetWarehousesByCompanyIdQuery(companyId.Value), cancellationToken);
        return result.IsSuccess && result.Value!.Any(warehouse => warehouse.Zones.Any(zone => zone.Id == zoneId));
    }
}
