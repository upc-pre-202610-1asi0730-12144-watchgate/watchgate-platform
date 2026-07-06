using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Shared.Domain.Model;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;

public partial class CompanyAccount : IAuditableEntity
{
    public CompanyAccountId Id { get; private set; }
    public int CompanyId { get; private set; }
    public string TradeName { get; private set; } = string.Empty;
    public string TaxId { get; private set; } = string.Empty;
    public string? LegalName { get; private set; }
    public string? Industry { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? Address { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string Status { get; private set; } = "ACTIVE";
    public bool IsProfileCompleted { get; private set; }
    public bool IsAdministratorEmailVerified { get; private set; }
    public string? EmailVerificationCode { get; private set; }

    protected CompanyAccount() { }

    public CompanyAccount(int companyId, string tradeName, string taxId)
    {
        CompanyId = companyId;
        TradeName = tradeName;
        TaxId = taxId;
        EmailVerificationCode = Guid.NewGuid().ToString("N");
    }

    public void CompleteProfile(string legalName, string industry, string contactPhone, string address, string? websiteUrl)
    {
        LegalName = legalName;
        Industry = industry;
        ContactPhone = contactPhone;
        Address = address;
        WebsiteUrl = websiteUrl;
        IsProfileCompleted = true;
    }

    public void UpdateInfo(string tradeName, string taxId, string? legalName, string? industry, string? contactPhone, string? address, string? websiteUrl)
    {
        TradeName = tradeName;
        TaxId = taxId;
        LegalName = legalName;
        Industry = industry;
        ContactPhone = contactPhone;
        Address = address;
        WebsiteUrl = websiteUrl;
        IsProfileCompleted = !string.IsNullOrWhiteSpace(legalName)
                             && !string.IsNullOrWhiteSpace(industry)
                             && !string.IsNullOrWhiteSpace(contactPhone)
                             && !string.IsNullOrWhiteSpace(address);
    }

    public bool VerifyAdministratorEmail(string verificationCode)
    {
        if (EmailVerificationCode != verificationCode) return false;
        IsAdministratorEmailVerified = true;
        EmailVerificationCode = null;
        return true;
    }

    public void Deactivate() => Status = "DEACTIVATED";
}
