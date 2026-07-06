namespace Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Resources;
public record CompleteCompanyProfileResource(string LegalName, string Industry, string ContactPhone, string Address, string? WebsiteUrl);
