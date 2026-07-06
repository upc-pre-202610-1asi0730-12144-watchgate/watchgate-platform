namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Commands;
public record RegisterCompanyAccountCommand(int CompanyId, string TradeName, string TaxId);
