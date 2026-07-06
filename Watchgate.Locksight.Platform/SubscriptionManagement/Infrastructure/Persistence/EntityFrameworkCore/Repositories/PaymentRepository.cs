using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Watchgate.Locksight.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Repositories;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PaymentRepository(AppDbContext context) : BaseRepository<Payment, PaymentId>(context), IPaymentRepository
{
    public async Task<IEnumerable<Payment>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        await Context.Set<Payment>()
            .Where(payment => payment.CompanyId == companyId)
            .OrderByDescending(payment => payment.RequestedAt)
            .ToListAsync(cancellationToken);
}
