using Watchgate.Locksight.Platform.Shared.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;

public partial class Subscription : IAuditableEntity
{
    public SubscriptionId Id { get; private set; }
    public int CompanyId { get; private set; }
    public SubscriptionPlanId PlanId { get; private set; }
    public string Status { get; private set; } = "ACTIVE";
    public DateTime StartedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public SubscriptionPlan? Plan { get; private set; }

    protected Subscription() { }

    public Subscription(int companyId, SubscriptionPlanId planId)
    {
        CompanyId = companyId;
        PlanId = planId;
        StartedAt = DateTime.UtcNow;
    }

    public void ChangePlan(SubscriptionPlanId planId) => PlanId = planId;

    public void Cancel()
    {
        Status = "CANCELLED";
        CancelledAt = DateTime.UtcNow;
    }
}