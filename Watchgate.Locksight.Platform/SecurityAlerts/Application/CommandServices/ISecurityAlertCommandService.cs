using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Commands;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Application.CommandServices;

public interface ISecurityAlertCommandService
{
    Task<Result<SecurityAlert>> Handle(CreateSecurityAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<SecurityAlert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<SecurityAlert>> Handle(AcknowledgeAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<SecurityAlert>> Handle(MarkAlertAsAttendedCommand command, CancellationToken cancellationToken = default);
    Task<Result<SecurityAlert>> Handle(EscalateAlertCommand command, CancellationToken cancellationToken = default);
    Task<Result<SecurityAlert>> Handle(FlagAlertAsFalseAlarmCommand command, CancellationToken cancellationToken = default);
    Task<Result<SecurityAlert>> Handle(ClassifyAlertPriorityCommand command, CancellationToken cancellationToken = default);
    Task<Result<AlertIncident>> Handle(CreateAlertIncidentCommand command, CancellationToken cancellationToken = default);
    Task<Result<AlertIncident>> Handle(CloseIncidentCommand command, CancellationToken cancellationToken = default);
}
