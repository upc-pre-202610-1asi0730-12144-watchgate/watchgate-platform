using Microsoft.AspNetCore.Mvc;
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
[Produces("application/json")]
public class WarehousesController(
    IWarehouseCommandService warehouseCommandService,
    IWarehouseQueryService warehouseQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
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
    public async Task<IActionResult> GetWarehouseById(int warehouseId, CancellationToken cancellationToken)
    {
        var query = new GetWarehouseByIdQuery(warehouseId);
        var result = await warehouseQueryService.Handle(query, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            warehouse => Ok(WarehouseResourceFromEntityAssembler.ToResourceFromEntity(warehouse)));
    }

    [HttpGet("company/{companyId:int}")]
    public async Task<IActionResult> GetWarehousesByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var query = new GetWarehousesByCompanyIdQuery(companyId);
        var result = await warehouseQueryService.Handle(query, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            warehouses => Ok(warehouses.Select(WarehouseResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpPut("{warehouseId:int}")]
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

    [HttpPost("{warehouseId:int}/zones")]
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
    public async Task<IActionResult> UpdateZoneRiskLevel(
        int warehouseId,
        int zoneId,
        [FromBody] UpdateZoneRiskLevelResource resource,
        CancellationToken cancellationToken)
    {
        var command = new UpdateZoneRiskLevelCommand(zoneId, resource.RiskLevel);
        var result = await warehouseCommandService.Handle(command, cancellationToken);
        return WarehouseActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            zone => Ok(WarehouseResourceFromEntityAssembler.ToZoneResourceFromEntity(zone)));
    }
}
