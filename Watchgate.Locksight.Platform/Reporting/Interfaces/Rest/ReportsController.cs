using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Watchgate.Locksight.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Watchgate.Locksight.Platform.Reporting.Application.CommandServices;
using Watchgate.Locksight.Platform.Reporting.Application.QueryServices;
using Watchgate.Locksight.Platform.Reporting.Domain.Model;
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
        if (result.IsFailure)
            return ToReportingProblem(result.Error, result.Message);

        var report = result.Value!;
        var eventsResult = await reportingQueryService.Handle(new GetEventLogQuery(report.CompanyId, report.From, report.To, null, null, report.WarehouseId), cancellationToken);
        if (eventsResult.IsFailure)
            return ToReportingProblem(eventsResult.Error, eventsResult.Message);

        var events = eventsResult.Value?.OrderByDescending(entry => entry.OccurredAt).ToList() ?? [];
        var content = BuildTextReport(report.Title, report.CompanyId, report.WarehouseId, report.From, report.To,
            report.TotalEvents, report.CriticalEvents, report.ResolvedEvents, report.GeneratedAt, events);

        return File(Encoding.UTF8.GetBytes(content), MediaTypeNames.Text.Plain, $"locksight-report-{report.Id}.txt");
    }

    [HttpGet("{reportId:int}/export/pdf")]
    [SwaggerOperation(Summary = "Export report as PDF", Description = "Exports the report as PDF-compatible output placeholder for frontend download flow.", OperationId = "ExportReportAsPdf")]
    [SwaggerResponse(StatusCodes.Status200OK, "The report was exported as PDF.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The report was not found.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "JWT token is missing or invalid.")]
    public async Task<IActionResult> ExportReportAsPdf(int reportId, CancellationToken cancellationToken)
    {
        var result = await reportingQueryService.Handle(new GetReportByIdQuery(reportId), cancellationToken);
        if (result.IsFailure)
            return ToReportingProblem(result.Error, result.Message);

        var report = result.Value!;
        var eventsResult = await reportingQueryService.Handle(new GetEventLogQuery(report.CompanyId, report.From, report.To, null, null, report.WarehouseId), cancellationToken);
        if (eventsResult.IsFailure)
            return ToReportingProblem(eventsResult.Error, eventsResult.Message);

        var events = eventsResult.Value?.OrderByDescending(entry => entry.OccurredAt).ToList() ?? [];
        var lines = BuildPdfReportLines(report.Title, report.CompanyId, report.WarehouseId, report.From, report.To,
            report.TotalEvents, report.CriticalEvents, report.ResolvedEvents, report.GeneratedAt, events);
        var bytes = SimplePdfDocumentBuilder.Build("LockSight Security Report", lines);

        return File(bytes, MediaTypeNames.Application.Pdf, $"locksight-report-{report.Id}.pdf");
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

    private IActionResult ToReportingProblem(Enum? error, string message)
    {
        var statusCode = error switch
        {
            ReportingError.ReportNotFound => StatusCodes.Status404NotFound,
            ReportingError.ScheduledReportNotFound => StatusCodes.Status404NotFound,
            ReportingError.InvalidReportRange => StatusCodes.Status400BadRequest,
            ReportingError.OperationCancelled => StatusCodes.Status409Conflict,
            ReportingError.DatabaseError => StatusCodes.Status500InternalServerError,
            ReportingError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        return problemDetailsFactory.CreateProblemDetails(this, statusCode, error, message);
    }

    private static string BuildTextReport(
        string title,
        int companyId,
        int? warehouseId,
        DateTime from,
        DateTime to,
        int totalEvents,
        int criticalEvents,
        int resolvedEvents,
        DateTime generatedAt,
        IReadOnlyCollection<EventLogEntry> events)
    {
        var builder = new StringBuilder();
        builder.AppendLine(title);
        builder.AppendLine($"Company ID: {companyId}");
        builder.AppendLine($"Warehouse ID: {warehouseId?.ToString() ?? "All"}");
        builder.AppendLine($"Period: {from:yyyy-MM-dd HH:mm} to {to:yyyy-MM-dd HH:mm}");
        builder.AppendLine($"Total events: {totalEvents}");
        builder.AppendLine($"Critical events: {criticalEvents}");
        builder.AppendLine($"Resolved events: {resolvedEvents}");
        builder.AppendLine($"Generated at UTC: {generatedAt:O}");
        builder.AppendLine();
        builder.AppendLine("Event details");

        if (events.Count == 0)
        {
            builder.AppendLine("No events were registered in this report range.");
            return builder.ToString();
        }

        foreach (var entry in events)
            builder.AppendLine($"- {FormatEvent(entry)}");

        return builder.ToString();
    }

    private static IEnumerable<string> BuildPdfReportLines(
        string title,
        int companyId,
        int? warehouseId,
        DateTime from,
        DateTime to,
        int totalEvents,
        int criticalEvents,
        int resolvedEvents,
        DateTime generatedAt,
        IReadOnlyCollection<EventLogEntry> events)
    {
        var lines = new List<string>
        {
            $"Title: {title}",
            $"Company ID: {companyId}",
            $"Warehouse ID: {warehouseId?.ToString() ?? "All"}",
            $"Period: {from:yyyy-MM-dd} to {to:yyyy-MM-dd}",
            $"Total events: {totalEvents}",
            $"Critical events: {criticalEvents}",
            $"Resolved events: {resolvedEvents}",
            $"Generated at UTC: {generatedAt:O}",
            "Event details:"
        };

        if (events.Count == 0)
            lines.Add("No events were registered in this report range.");
        else
            lines.AddRange(events.Take(20).Select(FormatEvent));

        return lines;
    }

    private static string FormatEvent(EventLogEntry entry) =>
        $"{entry.OccurredAt:yyyy-MM-dd HH:mm} UTC | {entry.EventType} | {entry.Severity} | {entry.Status} | Sensor {entry.SensorId} | {entry.Description}";
}
