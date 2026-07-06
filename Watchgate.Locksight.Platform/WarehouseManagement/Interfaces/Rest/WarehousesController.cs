using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.CommandServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Application.QueryServices;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Commands;
using Watchgate.Locksight.Platform.WarehouseManagement.Domain.Model.Queries;
using Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest.Transform;

namespace Watchgate.Locksight.Platform.WarehouseManagement.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Warehouse Management endpoints.")]
public class WarehousesController(
    IWarehouseCommandService warehouseCommandService,
    IWarehouseQueryService warehouseQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a warehouse", Description = "Creates a monitored warehouse for a company.", OperationId = "CreateWarehouse")]
    [SwaggerResponse(StatusCodes.Status201Created, "The warehouse was created.", typeof(WarehouseResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The warehouse could not be created.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CreateWarehouse(
        [FromBody] CreateWarehouseResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateWarehouseCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await warehouseCommandService.Handle(command, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            warehouse => CreatedAtAction(nameof(GetWarehouseById),
                new { warehouseId = warehouse.Id },
                WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse)));
    }

    [HttpGet("{warehouseId:int}")]
    [SwaggerOperation(Summary = "Get warehouse by ID", Description = "Gets the warehouse details for the requested identifier.", OperationId = "GetWarehouseById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The warehouse was retrieved.", typeof(WarehouseResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The warehouse was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetWarehouseById(int warehouseId, CancellationToken cancellationToken)
    {
        var query = new GetWarehouseByIdQuery(warehouseId);
        var result = await warehouseQueryService.Handle(query, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            warehouse => Ok(WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse)));
    }

    [HttpGet("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get warehouses by company", Description = "Lists all warehouses registered for a company.", OperationId = "GetWarehousesByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The warehouses were retrieved.", typeof(IEnumerable<WarehouseResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetWarehousesByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var query = new GetWarehousesByCompanyIdQuery(companyId);
        var result = await warehouseQueryService.Handle(query, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            warehouses => Ok(warehouses.Select(WarehouseResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("company/{companyId:int}/dashboard")]
    [SwaggerOperation(Summary = "View multi-warehouse dashboard", Description = "Returns consolidated counters for warehouses and zones in a company.", OperationId = "GetWarehouseDashboard")]
    [SwaggerResponse(StatusCodes.Status200OK, "The warehouse dashboard was retrieved.", typeof(WarehouseDashboardResource))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetWarehouseDashboard(int companyId, CancellationToken cancellationToken)
    {
        var result = await warehouseQueryService.Handle(new GetWarehouseDashboardByCompanyIdQuery(companyId), cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            dashboard => Ok(WarehouseResourceFromEntityAssembler.ToDashboardResourceFromEntity(dashboard)));
    }

    [HttpPut("{warehouseId:int}")]
    [SwaggerOperation(Summary = "Update warehouse", Description = "Updates the main operational data of a warehouse.", OperationId = "UpdateWarehouse")]
    [SwaggerResponse(StatusCodes.Status200OK, "The warehouse was updated.", typeof(WarehouseResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The warehouse was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> UpdateWarehouse(
        int warehouseId,
        [FromBody] UpdateWarehouseResource resource,
        CancellationToken cancellationToken)
    {
        var command = new UpdateWarehouseCommand(warehouseId, resource.Name, resource.Location,
            resource.Capacity, resource.OperationStart, resource.OperationEnd);
        var result = await warehouseCommandService.Handle(command, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            warehouse => Ok(WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse)));
    }

    [HttpPatch("{warehouseId:int}/deactivate")]
    [SwaggerOperation(Summary = "Deactivate warehouse", Description = "Deactivates a warehouse without deleting its historical data.", OperationId = "DeactivateWarehouse")]
    [SwaggerResponse(StatusCodes.Status200OK, "The warehouse was deactivated.", typeof(WarehouseResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The warehouse was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> DeactivateWarehouse(int warehouseId, CancellationToken cancellationToken)
    {
        var result = await warehouseCommandService.Handle(new DeactivateWarehouseCommand(warehouseId), cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            warehouse => Ok(WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse)));
    }

    [HttpDelete("{warehouseId:int}")]
    [SwaggerOperation(Summary = "Delete warehouse", Description = "Deletes a warehouse. This action is intended for administrator roles.", OperationId = "DeleteWarehouse")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The warehouse was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The warehouse was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> DeleteWarehouse(int warehouseId, CancellationToken cancellationToken)
    {
        var result = await warehouseCommandService.Handle(new DeleteWarehouseCommand(warehouseId), cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, _ => NoContent());
    }

    [HttpPost("{warehouseId:int}/zones")]
    [SwaggerOperation(Summary = "Add zone to warehouse", Description = "Registers a security zone inside a warehouse.", OperationId = "AddZoneToWarehouse")]
    [SwaggerResponse(StatusCodes.Status200OK, "The zone was created.", typeof(WarehouseZoneResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The warehouse was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> AddZoneToWarehouse(
        int warehouseId,
        [FromBody] CreateWarehouseZoneResource resource,
        CancellationToken cancellationToken)
    {
        var command = new CreateWarehouseZoneCommand(resource.Name, resource.Area, warehouseId, resource.RiskLevel);
        var result = await warehouseCommandService.Handle(command, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            zone => Ok(WarehouseResourceFromEntityAssembler.ToZoneResourceFromEntity(zone)));
    }

    [HttpPatch("{warehouseId:int}/zones/{zoneId:int}/risk-level")]
    [SwaggerOperation(Summary = "Update zone risk level", Description = "Changes the risk level of a warehouse zone.", OperationId = "UpdateZoneRiskLevel")]
    [SwaggerResponse(StatusCodes.Status200OK, "The zone risk level was updated.", typeof(WarehouseZoneResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The zone was not found for the given warehouse.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> UpdateZoneRiskLevel(
        int warehouseId,
        int zoneId,
        [FromBody] UpdateZoneRiskLevelResource resource,
        CancellationToken cancellationToken)
    {
        var command = new UpdateZoneRiskLevelCommand(warehouseId, zoneId, resource.RiskLevel);
        var result = await warehouseCommandService.Handle(command, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            zone => Ok(WarehouseResourceFromEntityAssembler.ToZoneResourceFromEntity(zone)));
    }
}
