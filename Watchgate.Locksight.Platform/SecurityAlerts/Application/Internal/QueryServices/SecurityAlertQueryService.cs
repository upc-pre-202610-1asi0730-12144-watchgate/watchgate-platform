using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SecurityAlerts.Application.QueryServices;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Queries;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Application.Internal.QueryServices;

public class SecurityAlertQueryService(
    ISecurityAlertRepository alertRepository,
    IAlertIncidentRepository incidentRepository) : ISecurityAlertQueryService
{
    public async Task<Result<SecurityAlert>> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default)
    {
        var alert = await alertRepository.FindByIdAsync(query.AlertId, cancellationToken);
        return alert is null
            ? Result<SecurityAlert>.Failure(SecurityAlertsError.AlertNotFound, $"Alert with id {query.AlertId} was not found.")
            : Result<SecurityAlert>.Success(alert);
    }

    public async Task<Result<IEnumerable<SecurityAlert>>> Handle(GetAlertsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var alerts = await alertRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<SecurityAlert>>.Success(alerts);
    }

    public async Task<Result<AlertIncident>> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var incident = await incidentRepository.FindByIdAsync(query.IncidentId, cancellationToken);
        return incident is null
            ? Result<AlertIncident>.Failure(SecurityAlertsError.IncidentNotFound, $"Incident with id {query.IncidentId} was not found.")
            : Result<AlertIncident>.Success(incident);
    }

    public async Task<Result<IEnumerable<AlertIncident>>> Handle(GetIncidentsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var incidents = await incidentRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<AlertIncident>>.Success(incidents);
    }
}
