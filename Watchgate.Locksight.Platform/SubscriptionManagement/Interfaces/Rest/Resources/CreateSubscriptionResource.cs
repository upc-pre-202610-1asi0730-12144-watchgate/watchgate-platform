namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

public record CreateSubscriptionResource(
    int CompanyId, 
    string Tier, 
    string CardNumber
);