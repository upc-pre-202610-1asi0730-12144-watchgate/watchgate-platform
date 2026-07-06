using Watchgate.Locksight.Platform.Shared.Domain.Model;
using Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.ValueObjects;

namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Aggregates;

public partial class SubscriptionPlan : IAuditableEntity
{
    public SubscriptionPlanId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal MonthlyPrice { get; private set; }
    public int MaxWarehouses { get; private set; }
    public int MaxSensors { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected SubscriptionPlan() { }

    public SubscriptionPlan(string name, string description, decimal monthlyPrice, int maxWarehouses, int maxSensors)
    {
        Name = name;
        Description = description;
        MonthlyPrice = monthlyPrice;
        MaxWarehouses = maxWarehouses;
        MaxSensors = maxSensors;
    }

    public void Update(string name, string description, decimal monthlyPrice, int maxWarehouses, int maxSensors)
    {
        Name = name;
        Description = description;
        MonthlyPrice = monthlyPrice;
        MaxWarehouses = maxWarehouses;
        MaxSensors = maxSensors;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}