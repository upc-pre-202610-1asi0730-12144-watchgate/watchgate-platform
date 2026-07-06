using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Reporting.Application.CommandServices;
using Watchgate.Locksight.Platform.Reporting.Domain.Model;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Reporting.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Reporting.Application.Internal.CommandServices;

public class ReportingCommandService(
    ISecurityReportRepository reportRepository,
    IScheduledReportRepository scheduledReportRepository,
    IEventLogRepository eventLogRepository,
    IUnitOfWork unitOfWork) : IReportingCommandService
{
    public async Task<Result<SecurityReport>> Handle(GenerateSecurityReportCommand command, CancellationToken cancellationToken = default)
    {
        if (command.From > command.To)
            return Result<SecurityReport>.Failure(ReportingError.InvalidReportRange, "The report start date must be earlier than or equal to the end date.");

        try
        {
            var events = (await eventLogRepository.FindEventsAsync(command.CompanyId, command.From, command.To, null, null, command.WarehouseId, cancellationToken)).ToList();
            var report = new SecurityReport(
                command.CompanyId,
                command.WarehouseId,
                $"Security report {command.From:yyyy-MM-dd} - {command.To:yyyy-MM-dd}",
                command.From,
                command.To,
                command.Format,
                events.Count,
                events.Count(e => e.Severity.Equals("CRITICAL", StringComparison.OrdinalIgnoreCase) || e.Severity.Equals("HIGH", StringComparison.OrdinalIgnoreCase)),
                events.Count(e => e.Status.Equals("RESOLVED", StringComparison.OrdinalIgnoreCase)));

            await reportRepository.AddAsync(report, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SecurityReport>.Success(report);
        }
        catch (OperationCanceledException)
        {
            return Result<SecurityReport>.Failure(ReportingError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<SecurityReport>.Failure(ReportingError.DatabaseError, "A database error occurred while generating the report.");
        }
        catch (Exception)
        {
            return Result<SecurityReport>.Failure(ReportingError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<ScheduledReport>> Handle(ScheduleReportCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var scheduledReport = new ScheduledReport(command.CompanyId, command.WarehouseId, command.Name, command.Frequency, command.Format, command.RecipientEmail, command.StartsAt);
            await scheduledReportRepository.AddAsync(scheduledReport, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ScheduledReport>.Success(scheduledReport);
        }
        catch (OperationCanceledException)
        {
            return Result<ScheduledReport>.Failure(ReportingError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<ScheduledReport>.Failure(ReportingError.DatabaseError, "A database error occurred while scheduling the report.");
        }
        catch (Exception)
        {
            return Result<ScheduledReport>.Failure(ReportingError.InternalServerError, "An unexpected error occurred.");
        }
    }
}