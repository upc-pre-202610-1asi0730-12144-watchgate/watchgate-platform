using Watchgate.Locksight.Platform.Reporting.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Reporting.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.Reporting.Application.CommandServices;

public interface IReportingCommandService
{
    Task<Result<SecurityReport>> Handle(GenerateSecurityReportCommand command, CancellationToken cancellationToken = default);
    Task<Result<ScheduledReport>> Handle(ScheduleReportCommand command, CancellationToken cancellationToken = default);
}