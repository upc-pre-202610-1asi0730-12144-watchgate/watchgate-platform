using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Reporting.Application.CommandServices;
using Watchgate.Locksight.Platform.Reporting.Application.QueryServices;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Resources;
using Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Transform;
using Watchgate.Locksight.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Reporting endpoints.")]
public class ReportsController(
    IReportingCommandService reportingCommandService,
    IReportingQueryService reportingQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("event-log")]
    [SwaggerOperation(Summary = "View event history", Description = "Lists event history and supports filters by date, type, zone and warehouse.", OperationId = "GetEventLog")]
    [SwaggerResponse(StatusCodes.Status200OK, "The event history was retrieved.", typeof(IEnumerable<EventLogEntryResource>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The date range is invalid.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetEventLog(
        [FromQuery] int companyId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? type,
        [FromQuery] int? zoneId,
        [FromQuery] int? warehouseId,
        CancellationToken cancellationToken)
    {
        var query = new GetEventLogQuery(companyId, from, to, type, zoneId, warehouseId);
        var result = await reportingQueryService.Handle(query, cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            events => Ok(events.Select(ReportingResourceFromEntityAssembler.ToResourceFromEventLogEntry)));
    }

    [HttpGet("dashboard")]
    [SwaggerOperation(Summary = "View consolidated reporting dashboard", Description = "Returns consolidated event counters for the reporting dashboard.", OperationId = "GetReportingDashboard")]
    [SwaggerResponse(StatusCodes.Status200OK, "The dashboard summary was retrieved.", typeof(ReportingDashboardResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The date range is invalid.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetDashboard([FromQuery] int companyId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
    {
        var result = await reportingQueryService.Handle(new GetReportingDashboardQuery(companyId, from, to), cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            dashboard => Ok(ReportingResourceFromEntityAssembler.ToResourceFromDashboard(dashboard)));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Generate security report", Description = "Generates a security report for a date range and optional warehouse.", OperationId = "GenerateSecurityReport")]
    [SwaggerResponse(StatusCodes.Status201Created, "The security report was generated.", typeof(SecurityReportResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The report request is invalid.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GenerateReport([FromBody] GenerateSecurityReportResource resource, CancellationToken cancellationToken)
    {
        var command = GenerateSecurityReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reportingCommandService.Handle(command, cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            report => CreatedAtAction(nameof(GetReportById), new { reportId = report.Id }, ReportingResourceFromEntityAssembler.ToResourceFromSecurityReport(report)));
    }

    [HttpGet("{reportId:int}")]
    [SwaggerOperation(Summary = "Get generated report", Description = "Gets metadata of a generated report.", OperationId = "GetReportById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The report was retrieved.", typeof(SecurityReportResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The report was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetReportById(int reportId, CancellationToken cancellationToken)
    {
        var result = await reportingQueryService.Handle(new GetReportByIdQuery(reportId), cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            report => Ok(ReportingResourceFromEntityAssembler.ToResourceFromSecurityReport(report)));
    }

    [HttpGet("company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get generated reports by company", Description = "Lists generated reports for a company.", OperationId = "GetReportsByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The reports were retrieved.", typeof(IEnumerable<SecurityReportResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetReportsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var result = await reportingQueryService.Handle(new GetReportsByCompanyIdQuery(companyId), cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            reports => Ok(reports.Select(ReportingResourceFromEntityAssembler.ToResourceFromSecurityReport)));
    }

    [HttpGet("{reportId:int}/download")]
    [SwaggerOperation(Summary = "Download report", Description = "Downloads the generated report representation.", OperationId = "DownloadReport")]
    [SwaggerResponse(StatusCodes.Status200OK, "The report was downloaded.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The report was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> DownloadReport(int reportId, CancellationToken cancellationToken)
    {
        var result = await reportingQueryService.Handle(new GetReportByIdQuery(reportId), cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            report => File(Encoding.UTF8.GetBytes($"{report.Title}\nTotal events: {report.TotalEvents}\nCritical events: {report.CriticalEvents}\nResolved events: {report.ResolvedEvents}"), MediaTypeNames.Text.Plain, $"locksight-report-{report.Id}.txt"));
    }

    [HttpGet("{reportId:int}/export/pdf")]
    [SwaggerOperation(Summary = "Export report as PDF", Description = "Exports the report as PDF-compatible output placeholder for frontend download flow.", OperationId = "ExportReportAsPdf")]
    [SwaggerResponse(StatusCodes.Status200OK, "The report was exported as PDF.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The report was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> ExportReportAsPdf(int reportId, CancellationToken cancellationToken)
    {
        var result = await reportingQueryService.Handle(new GetReportByIdQuery(reportId), cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            report =>
            {
                var bytes = SimplePdfDocumentBuilder.Build("LockSight Security Report",
                [
                    $"Title: {report.Title}",
                    $"Company ID: {report.CompanyId}",
                    $"Warehouse ID: {report.WarehouseId?.ToString() ?? "All"}",
                    $"Period: {report.From:yyyy-MM-dd} to {report.To:yyyy-MM-dd}",
                    $"Total events: {report.TotalEvents}",
                    $"Critical events: {report.CriticalEvents}",
                    $"Resolved events: {report.ResolvedEvents}",
                    $"Generated at UTC: {report.GeneratedAt:O}"
                ]);
                return File(bytes, MediaTypeNames.Application.Pdf, $"locksight-report-{report.Id}.pdf");
            });
    }

    [HttpPost("schedule")]
    [SwaggerOperation(Summary = "Schedule periodic report", Description = "Schedules periodic report generation for a company.", OperationId = "ScheduleReport")]
    [SwaggerResponse(StatusCodes.Status201Created, "The periodic report was scheduled.", typeof(ScheduledReportResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The schedule request is invalid.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> ScheduleReport([FromBody] ScheduleReportResource resource, CancellationToken cancellationToken)
    {
        var command = ScheduleReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reportingCommandService.Handle(command, cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            report => CreatedAtAction(nameof(GetScheduledReportsByCompanyId), new { companyId = report.CompanyId }, ReportingResourceFromEntityAssembler.ToResourceFromScheduledReport(report)));
    }

    [HttpGet("schedule/company/{companyId:int}")]
    [SwaggerOperation(Summary = "Get scheduled reports by company", Description = "Lists periodic report schedules for a company.", OperationId = "GetScheduledReportsByCompanyId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The scheduled reports were retrieved.", typeof(IEnumerable<ScheduledReportResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> GetScheduledReportsByCompanyId(int companyId, CancellationToken cancellationToken)
    {
        var result = await reportingQueryService.Handle(new GetScheduledReportsByCompanyIdQuery(companyId), cancellationToken);
        return ReportingActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            reports => Ok(reports.Select(ReportingResourceFromEntityAssembler.ToResourceFromScheduledReport)));
    }
}
