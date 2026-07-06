using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Repositories;

public interface ICompanyAccountRepository : IBaseRepository<CompanyAccount, CompanyAccountId>
{
    Task<CompanyAccount?> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
