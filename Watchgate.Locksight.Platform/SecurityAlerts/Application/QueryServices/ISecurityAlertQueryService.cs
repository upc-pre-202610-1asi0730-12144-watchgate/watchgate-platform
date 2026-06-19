using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SecurityAlerts.Domain.Model.Queries;

namespace Watchgate.Locksight.Platform.SecurityAlerts.Application.QueryServices;

public interface ISecurityAlertQueryService
{
    Task<Result<SecurityAlert>> Handle(GetAlertByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SecurityAlert>>> Handle(GetAlertsByCompanyIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<AlertIncident>> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<AlertIncident>>> Handle(GetIncidentsByCompanyIdQuery query, CancellationToken cancellationToken = default);
}