using Watchgate.Locksight.Platform.Shared.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Errors;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;

public partial class Subscription : IAuditableEntity
{
    public SubscriptionId Id { get; private set; }
    public int CompanyId { get; private set; } 
    public EPlanTier Tier { get; private set; }
    public CardNumber PaymentMethod { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime NextBillingDate { get; private set; }
    public bool IsActive { get; private set; }

    protected Subscription() { }

    public Subscription(int companyId, EPlanTier tier, CardNumber paymentMethod)
    {
        if (companyId <= 0) throw new ArgumentException(SubscriptionErrors.CompanyIdRequired.Message);

        CompanyId = companyId;
        Tier = tier;
        PaymentMethod = paymentMethod;
        StartDate = DateTime.UtcNow;
        NextBillingDate = DateTime.UtcNow.AddMonths(1);
        IsActive = true;
    }

    public void Cancel()
    {
        if (!IsActive) throw new InvalidOperationException(SubscriptionErrors.AlreadyCanceled.Message);
        IsActive = false;
    }

    public void ChangePlan(EPlanTier newTier)
    {
        if (!IsActive) throw new InvalidOperationException(SubscriptionErrors.CannotChangeCanceledPlan.Message);
        if (Tier == newTier) throw new InvalidOperationException(SubscriptionErrors.AlreadyOnThisPlan.Message);
        
        Tier = newTier;
        NextBillingDate = DateTime.UtcNow.AddMonths(1); 
    }
}