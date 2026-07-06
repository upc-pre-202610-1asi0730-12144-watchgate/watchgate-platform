using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Resources;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Interfaces.Rest.Transform;

public static class CompanyAccountResourceFromEntityAssembler
{
    public static CompanyAccountResource ToResourceFromEntity(CompanyAccount account) =>
        new(account.Id, account.CompanyId, account.TradeName, account.TaxId, account.LegalName,
            account.Industry, account.ContactPhone, account.Address, account.WebsiteUrl, account.Status,
            account.IsProfileCompleted, account.IsAdministratorEmailVerified, account.EmailVerificationCode);
}
