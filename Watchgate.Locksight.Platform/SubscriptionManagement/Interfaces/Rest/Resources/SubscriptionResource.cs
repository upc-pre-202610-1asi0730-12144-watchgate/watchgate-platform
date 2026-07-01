namespace Watchgate.Locksight.Platform.SubscriptionManagement.Interfaces.Rest.Resources;

public record SubscriptionResource(
    int Id, 
    int CompanyId, 
    string Tier, 
    string Last4Digits, 
    bool IsActive
);