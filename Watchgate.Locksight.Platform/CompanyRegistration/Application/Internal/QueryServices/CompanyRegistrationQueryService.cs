using Watchgate.Locksight.Platform.CompanyRegistration.Application.QueryServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Queries;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Application.Internal.QueryServices;

public class CompanyRegistrationQueryService(ICompanyAccountRepository companyAccountRepository) : ICompanyRegistrationQueryService
{
    public async Task<Result<CompanyAccount>> Handle(GetCompanyAccountByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var companyAccount = await companyAccountRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return companyAccount is null
            ? Result<CompanyAccount>.Failure(CompanyRegistrationError.CompanyAccountNotFound, "The company account was not found.")
            : Result<CompanyAccount>.Success(companyAccount);
    }
}
