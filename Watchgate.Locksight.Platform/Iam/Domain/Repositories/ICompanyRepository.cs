using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Domain.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<Company?> FindByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);
}
