using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserAccessProfileRepository(AppDbContext context) : BaseRepository<UserAccessProfile, int>(context), IUserAccessProfileRepository
{
    public async Task<UserAccessProfile?> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default) =>
        await Context.Set<UserAccessProfile>().FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);

    public async Task<IEnumerable<UserAccessProfile>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<UserAccessProfile>()
            .Where(profile => profile.CompanyId == companyId)
            .OrderBy(profile => profile.UserId)
            .ToListAsync(cancellationToken);
}
