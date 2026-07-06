namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Commands;
public record UpdateCompanyInfoCommand(int CompanyId, string TradeName, string TaxId, string? LegalName, string? Industry, string? ContactPhone, string? Address, string? WebsiteUrl);
