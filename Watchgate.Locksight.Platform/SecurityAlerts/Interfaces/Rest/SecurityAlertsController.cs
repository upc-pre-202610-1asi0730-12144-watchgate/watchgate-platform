using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Security Alerts endpoints.")]
public class SecurityAlertsController(
    ISecurityAlertCommandService alertCommandService,
    ISecurityAlertQueryService alertQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create security alert", Description = "Registers a security alert triggered by a sensor.", OperationId = "CreateSecurityAlert")]
    [SwaggerResponse(StatusCodes.Status201Created, "The alert was created.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The alert could not be created.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CreateSecurityAlert([FromBody] CreateSecurityAlertResource resource, CancellationToken cancellationToken)
    {
        var command = new CreateSecurityAlertCommand(resource.Type, resource.Severity, resource.Description, resource.SensorId, resource.CompanyId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => CreatedAtAction(nameof(GetAlertById), new { alertId = alert.Id }, SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpGet("{alertId:int}")]
    [SwaggerOperation(Summary = "Get alert by ID", Description = "Gets a security alert by identifier.", OperationId = "GetAlertById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was retrieved.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetAlertById(int alertId, CancellationToken cancellationToken)
    {
        var query = new GetAlertByIdQuery(alertId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpGet("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get alerts by company", Description = "Lists security alerts for a company.", OperationId = "GetAlertsByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alerts were retrieved.", typeof(IEnumerable<SecurityAlertResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetAlertsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var query = new GetAlertsByCompanyIdQuery(companyId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alerts => Ok(alerts.Select(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("warehouse/{warehouseId:int}")]
    [SwaggerOperation(Summary = "Get alerts by warehouse", Description = "Lists security alerts related to sensors installed in a warehouse.", OperationId = "GetAlertsByWarehouseId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alerts were retrieved.", typeof(IEnumerable<SecurityAlertResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetAlertsByWarehouseId(int warehouseId, CancellationToken cancellationToken)
    {
        var query = new GetAlertsByWarehouseIdQuery(warehouseId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alerts => Ok(alerts.Select(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpPatch("{alertId:int}/resolve")]
    [SwaggerOperation(Summary = "Resolve alert", Description = "Marks a security alert as resolved.", OperationId = "ResolveAlert")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was resolved.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> ResolveAlert(int alertId, CancellationToken cancellationToken)
    {
        var command = new ResolveAlertCommand(alertId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpPatch("{alertId:int}/acknowledge")]
    [SwaggerOperation(Summary = "Acknowledge alert", Description = "Marks a security alert as acknowledged by the operations manager.", OperationId = "AcknowledgeAlert")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was acknowledged.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> AcknowledgeAlert(int alertId, CancellationToken cancellationToken)
    {
        var result = await alertCommandService.Handle(new AcknowledgeAlertCommand(alertId), cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpPatch("{alertId:int}/attend")]
    [SwaggerOperation(Summary = "Mark alert as attended", Description = "Marks a security alert as attended.", OperationId = "MarkAlertAsAttended")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was marked as attended.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> MarkAlertAsAttended(int alertId, CancellationToken cancellationToken)
    {
        var result = await alertCommandService.Handle(new MarkAlertAsAttendedCommand(alertId), cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpPatch("{alertId:int}/escalate")]
    [SwaggerOperation(Summary = "Escalate alert", Description = "Escalates a security alert and classifies it as critical.", OperationId = "EscalateAlert")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was escalated.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> EscalateAlert(int alertId, CancellationToken cancellationToken)
    {
        var result = await alertCommandService.Handle(new EscalateAlertCommand(alertId), cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpPatch("{alertId:int}/false-alarm")]
    [SwaggerOperation(Summary = "Flag alert as false alarm", Description = "Flags a security alert as a false alarm.", OperationId = "FlagAlertAsFalseAlarm")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert was flagged as false alarm.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> FlagAlertAsFalseAlarm(int alertId, CancellationToken cancellationToken)
    {
        var result = await alertCommandService.Handle(new FlagAlertAsFalseAlarmCommand(alertId), cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpPatch("{alertId:int}/priority")]
    [SwaggerOperation(Summary = "Classify alert priority", Description = "Updates the priority/severity assigned to a security alert.", OperationId = "ClassifyAlertPriority")]
    [SwaggerResponse(StatusCodes.Status200OK, "The alert priority was classified.", typeof(SecurityAlertResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The alert was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> ClassifyAlertPriority(int alertId, [FromBody] ClassifyAlertPriorityResource resource, CancellationToken cancellationToken)
    {
        var result = await alertCommandService.Handle(new ClassifyAlertPriorityCommand(alertId, resource.Severity), cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            alert => Ok(SecurityAlertResourceFromEntityAssembler.ToResourceFromEntity(alert)));
    }

    [HttpPost("incidents")]
    [SwaggerOperation(Summary = "Create alert incident", Description = "Creates an incident record for alert follow-up.", OperationId = "CreateAlertIncident")]
    [SwaggerResponse(StatusCodes.Status201Created, "The incident was created.", typeof(AlertIncidentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The incident could not be created.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CreateIncident([FromBody] CreateAlertIncidentResource resource, CancellationToken cancellationToken)
    {
        var command = new CreateAlertIncidentCommand(resource.Title, resource.Description, resource.Priority, resource.CompanyId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => CreatedAtAction(nameof(GetIncidentById), new { incidentId = incident.Id }, SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity(incident)));
    }

    [HttpGet("incidents/{incidentId:int}")]
    [SwaggerOperation(Summary = "Get incident by ID", Description = "Gets an alert incident by identifier.", OperationId = "GetAlertIncidentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The incident was retrieved.", typeof(AlertIncidentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The incident was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetIncidentById(int incidentId, CancellationToken cancellationToken)
    {
        var query = new GetIncidentByIdQuery(incidentId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => Ok(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity(incident)));
    }

    [HttpGet("incidents/company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get incidents by company", Description = "Lists alert incidents for a company.", OperationId = "GetAlertIncidentsByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The incidents were retrieved.", typeof(IEnumerable<AlertIncidentResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetIncidentsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var query = new GetIncidentsByCompanyIdQuery(companyId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incidents => Ok(incidents.Select(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity)));
    }

    [HttpGet("incidents/warehouse/{warehouseId:int}")]
    [SwaggerOperation(Summary = "Get incidents by warehouse", Description = "Lists incidents related to alerts from a warehouse.", OperationId = "GetAlertIncidentsByWarehouseId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The incidents were retrieved.", typeof(IEnumerable<AlertIncidentResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetIncidentsByWarehouseId(int warehouseId, CancellationToken cancellationToken)
    {
        var query = new GetIncidentsByWarehouseIdQuery(warehouseId);
        var result = await alertQueryService.Handle(query, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incidents => Ok(incidents.Select(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity)));
    }

    [HttpPatch("incidents/{incidentId:int}/close")]
    [SwaggerOperation(Summary = "Close incident", Description = "Marks an alert incident as closed.", OperationId = "CloseAlertIncident")]
    [SwaggerResponse(StatusCodes.Status200OK, "The incident was closed.", typeof(AlertIncidentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The incident was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> CloseIncident(int incidentId, CancellationToken cancellationToken)
    {
        var command = new CloseIncidentCommand(incidentId);
        var result = await alertCommandService.Handle(command, cancellationToken);
        return SecurityAlertActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => Ok(SecurityAlertResourceFromEntityAssembler.ToIncidentResourceFromEntity(incident)));
    }
}
