using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CompanyRepository(AppDbContext context) : BaseRepository<Company, CompanyId>(context), ICompanyRepository
{
    public async Task<Company?> FindByTaxIdAsync(string taxId, CancellationToken cancellationToken = default) =>
        await Context.Set<Company>().FirstOrDefaultAsync(c => c.TaxId == taxId, cancellationToken);
}
