using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CompanyAccountRepository(AppDbContext context) : BaseRepository<CompanyAccount, CompanyAccountId>(context), ICompanyAccountRepository
{
    public async Task<CompanyAccount?> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<CompanyAccount>().FirstOrDefaultAsync(account => account.CompanyId == companyId, cancellationToken);

    public async Task<bool> ExistsByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<CompanyAccount>().AnyAsync(account => account.CompanyId == companyId, cancellationToken);
}
