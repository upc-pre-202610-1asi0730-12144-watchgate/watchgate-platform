using Microsoft.AspNetCore.Mvc;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.CommandServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.QueryServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest.Transform;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SecurityAlertsController(
    ISecurityAlertCommandService alertCommandService,
    ISecurityAlertQueryService alertQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSecurityAlert(
        [FromBody] CreateSecurityAlertResource resource,
        CancellationToken cancellationToken)
    {
        var command = new CreateSecurityAlertCommand(resource.Type, resource.Severity, resource.Description,
            resource.SensorId, resource.CompanyId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => CreatedAtAction(nameof(GetAlertById),
                new { alertId = alert.Id },
                SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpGet("{alertId:int}")]
    public async Task<IActionResult> GetAlertById(int alertId, CancellationToken cancellationToken)
    {
        var query = new GetAlertByIdQuery(alertId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpGet("company/{companyId:int}")]
    public async Task<IActionResult> GetAlertsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var query = new GetAlertsByCompanyIdQuery(companyId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alerts => Ok(alerts.Select(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("warehouse/{warehouseId:int}")]
    public async Task<IActionResult> GetAlertsByWarehouseId(int warehouseId, CancellationToken cancellationToken)
    {
        var query = new GetAlertsByWarehouseIdQuery(warehouseId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alerts => Ok(alerts.Select(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpPatch("{alertId:int}/resolve")]
    public async Task<IActionResult> ResolveAlert(int alertId, CancellationToken cancellationToken)
    {
        var command = new ResolveAlertCommand(alertId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpPost("incidents")]
    public async Task<IActionResult> CreateIncident(
        [FromBody] CreateAlertIncidentResource resource,
        CancellationToken cancellationToken)
    {
        var command = new CreateAlertIncidentCommand(resource.Title, resource.Description, resource.Priority, resource.CompanyId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => CreatedAtAction(nameof(GetIncidentById),
                new { incidentId = incident.Id },
                SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity(incident)));
    }

    [HttpGet("incidents/{incidentId:int}")]
    public async Task<IActionResult> GetIncidentById(int incidentId, CancellationToken cancellationToken)
    {
        var query = new GetIncidentByIdQuery(incidentId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => Ok(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity(incident)));
    }

    [HttpGet("incidents/company/{companyId:int}")]
    public async Task<IActionResult> GetIncidentsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var query = new GetIncidentsByCompanyIdQuery(companyId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incidents => Ok(incidents.Select(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity)));
    }

    [HttpGet("incidents/warehouse/{warehouseId:int}")]
    public async Task<IActionResult> GetIncidentsByWarehouseId(int warehouseId, CancellationToken cancellationToken)
    {
        var query = new GetIncidentsByWarehouseIdQuery(warehouseId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incidents => Ok(incidents.Select(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity)));
    }

    [HttpPatch("incidents/{incidentId:int}/close")]
    public async Task<IActionResult> CloseIncident(int incidentId, CancellationToken cancellationToken)
    {
        var command = new CloseIncidentCommand(incidentId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => Ok(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity(incident)));
    }
}
