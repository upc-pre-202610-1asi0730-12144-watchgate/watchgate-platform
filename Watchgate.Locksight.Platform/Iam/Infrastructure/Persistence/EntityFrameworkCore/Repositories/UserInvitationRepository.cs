using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserInvitationRepository(AppDbContext context) : BaseRepository<UserInvitation, int>(context), IUserInvitationRepository
{
    public async Task<UserInvitation?> FindByTokenAsync(string token, CancellationToken cancellationToken = default) =>
        await Context.Set<UserInvitation>().FirstOrDefaultAsync(invitation => invitation.Token == token, cancellationToken);

    public async Task<IEnumerable<UserInvitation>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<UserInvitation>()
            .Where(invitation => invitation.CompanyId == companyId)
            .OrderByDescending(invitation => invitation.CreatedAt)
            .ToListAsync(cancellationToken);
}
