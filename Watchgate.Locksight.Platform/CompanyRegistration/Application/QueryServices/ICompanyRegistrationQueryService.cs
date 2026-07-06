using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Application.QueryServices;

public interface ICompanyRegistrationQueryService
{
    Task<Result<CompanyAccount>> Handle(GetCompanyAccountByCompanyIdQuery query, CancellationToken cancellationToken = default);
}
