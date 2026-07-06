namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Commands;
public record CompleteCompanyProfileCommand(int CompanyId, string LegalName, string Industry, string ContactPhone, string Address, string? WebsiteUrl);
