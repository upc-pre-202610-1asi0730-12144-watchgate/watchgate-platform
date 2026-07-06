namespace Watchgate.Locksight.Platform.SubscriptionManagement.Domain.Model.Commands;
public record ChangeSubscriptionPlanCommand(int SubscriptionId, int PlanId);