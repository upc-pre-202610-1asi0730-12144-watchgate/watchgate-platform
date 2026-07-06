namespace Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Resources;
public record UpdateCompanyInfoResource(string TradeName, string TaxId, string? LegalName, string? Industry, string? ContactPhone, string? Address, string? WebsiteUrl);
