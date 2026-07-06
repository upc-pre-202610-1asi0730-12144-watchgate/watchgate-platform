using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class InvoiceRepository(AppDbContext context) : BaseRepository<Invoice, InvoiceId>(context), IInvoiceRepository
{
    public async Task<IEnumerable<Invoice>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<Invoice>()
            .Where(invoice => invoice.CompanyId == companyId)
            .OrderByDescending(invoice => invoice.IssuedAt)
            .ToListAsync(cancellationToken);
}
