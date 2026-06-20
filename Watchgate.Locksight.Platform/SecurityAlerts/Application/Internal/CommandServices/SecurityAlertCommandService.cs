using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Model.Events;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.CommandServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Commands;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Events;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.CommandServices;

public class SecurityAlertCommandService(
    ISecurityAlertRepository alertRepository,
    IAlertIncidentRepository incidentRepository,
    IUnitOfWork unitOfWork,
    IEventDispatcher eventDispatcher) : ISecurityAlertCommandService
{
    public async Task<Result<SecurityAlert>> Handle(CreateSecurityAlertCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var alert = new SecurityAlert(command.Type, command.Severity, command.Description, command.SensorId, command.CompanyId);
            await alertRepository.AddAsync(alert, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await eventDispatcher.DispatchAsync(
                new SecurityAlertTriggeredEvent(alert.Id, alert.Type, alert.Severity, alert.SensorId, alert.CompanyId),
                cancellationToken);

            return Result<SecurityAlert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return Result<SecurityAlert>.Failure(SecurityAlertsError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<SecurityAlert>.Failure(SecurityAlertsError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<SecurityAlert>.Failure(SecurityAlertsError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<SecurityAlert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
            if (alert is null)
                return Result<SecurityAlert>.Failure(SecurityAlertsError.AlertNotFound, $"Alert with id {command.AlertId} was not found.");

            alert.Resolve();
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SecurityAlert>.Success(alert);
        }
        catch (OperationCanceledException)
        {
            return Result<SecurityAlert>.Failure(SecurityAlertsError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<SecurityAlert>.Failure(SecurityAlertsError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<SecurityAlert>.Failure(SecurityAlertsError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<AlertIncident>> Handle(CreateAlertIncidentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var incident = new AlertIncident(command.Title, command.Description, command.Priority, command.CompanyId);
            await incidentRepository.AddAsync(incident, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<AlertIncident>.Success(incident);
        }
        catch (OperationCanceledException)
        {
            return Result<AlertIncident>.Failure(SecurityAlertsError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<AlertIncident>.Failure(SecurityAlertsError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<AlertIncident>.Failure(SecurityAlertsError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<AlertIncident>> Handle(CloseIncidentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var incident = await incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
            if (incident is null)
                return Result<AlertIncident>.Failure(SecurityAlertsError.IncidentNotFound, $"Incident with id {command.IncidentId} was not found.");

            incident.Close();
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<AlertIncident>.Success(incident);
        }
        catch (OperationCanceledException)
        {
            return Result<AlertIncident>.Failure(SecurityAlertsError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<AlertIncident>.Failure(SecurityAlertsError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<AlertIncident>.Failure(SecurityAlertsError.InternalServerError, "An unexpected error occurred.");
        }
    }
}
