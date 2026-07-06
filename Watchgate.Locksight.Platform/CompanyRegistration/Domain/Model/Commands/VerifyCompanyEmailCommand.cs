namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Commands;
public record VerifyCompanyEmailCommand(int CompanyId, string VerificationCode);
