using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Application.CommandServices;

public interface ICompanyRegistrationCommandService
{
    Task<Result<CompanyAccount>> Handle(RegisterCompanyAccountCommand command, CancellationToken cancellationToken = default);
    Task<Result<CompanyAccount>> Handle(CompleteCompanyProfileCommand command, CancellationToken cancellationToken = default);
    Task<Result<CompanyAccount>> Handle(UpdateCompanyInfoCommand command, CancellationToken cancellationToken = default);
    Task<Result<CompanyAccount>> Handle(VerifyCompanyEmailCommand command, CancellationToken cancellationToken = default);
    Task<Result<CompanyAccount>> Handle(DeactivateCompanyAccountCommand command, CancellationToken cancellationToken = default);
}
