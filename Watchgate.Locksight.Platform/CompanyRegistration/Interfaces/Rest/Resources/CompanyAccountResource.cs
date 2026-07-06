namespace Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Resources;

public record CompanyAccountResource(
    int Id,
    int CompanyId,
    string TradeName,
    string TaxId,
    string? LegalName,
    string? Industry,
    string? ContactPhone,
    string? Address,
    string? WebsiteUrl,
    string Status,
    bool IsProfileCompleted,
    bool IsAdministratorEmailVerified,
    string? EmailVerificationCode);
