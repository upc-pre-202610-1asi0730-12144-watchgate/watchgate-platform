using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.CompanyRegistration.Application.CommandServices;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Model.Commands;
using Watchgate.Locksight.Platform.CompanyRegistration.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.CompanyRegistration.Application.Internal.CommandServices;

public class CompanyRegistrationCommandService(
    ICompanyAccountRepository companyAccountRepository,
    IUnitOfWork unitOfWork) : ICompanyRegistrationCommandService
{
    public async Task<Result<CompanyAccount>> Handle(RegisterCompanyAccountCommand command, CancellationToken cancellationToken = default)
    {
        if (await companyAccountRepository.ExistsByCompanyIdAsync(command.CompanyId, cancellationToken))
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.CompanyAccountAlreadyExists, "The company account already exists.");

        try
        {
            var companyAccount = new CompanyAccount(command.CompanyId, command.TradeName, command.TaxId);
            await companyAccountRepository.AddAsync(companyAccount, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<CompanyAccount>.Success(companyAccount);
        }
        catch (OperationCanceledException)
        {
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<CompanyAccount>> Handle(CompleteCompanyProfileCommand command, CancellationToken cancellationToken = default)
    {
        var companyAccount = await companyAccountRepository.FindByCompanyIdAsync(command.CompanyId, cancellationToken);
        if (companyAccount is null)
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.CompanyAccountNotFound, "The company account was not found.");

        companyAccount.CompleteProfile(command.LegalName, command.Industry, command.ContactPhone, command.Address, command.WebsiteUrl);
        companyAccountRepository.Update(companyAccount);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<CompanyAccount>.Success(companyAccount);
    }

    public async Task<Result<CompanyAccount>> Handle(UpdateCompanyInfoCommand command, CancellationToken cancellationToken = default)
    {
        var companyAccount = await companyAccountRepository.FindByCompanyIdAsync(command.CompanyId, cancellationToken);
        if (companyAccount is null)
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.CompanyAccountNotFound, "The company account was not found.");

        companyAccount.UpdateInfo(command.TradeName, command.TaxId, command.LegalName, command.Industry, command.ContactPhone, command.Address, command.WebsiteUrl);
        companyAccountRepository.Update(companyAccount);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<CompanyAccount>.Success(companyAccount);
    }

    public async Task<Result<CompanyAccount>> Handle(VerifyCompanyEmailCommand command, CancellationToken cancellationToken = default)
    {
        var companyAccount = await companyAccountRepository.FindByCompanyIdAsync(command.CompanyId, cancellationToken);
        if (companyAccount is null)
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.CompanyAccountNotFound, "The company account was not found.");

        if (!companyAccount.VerifyAdministratorEmail(command.VerificationCode))
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.InvalidVerificationCode, "The verification code is invalid.");

        companyAccountRepository.Update(companyAccount);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<CompanyAccount>.Success(companyAccount);
    }

    public async Task<Result<CompanyAccount>> Handle(DeactivateCompanyAccountCommand command, CancellationToken cancellationToken = default)
    {
        var companyAccount = await companyAccountRepository.FindByCompanyIdAsync(command.CompanyId, cancellationToken);
        if (companyAccount is null)
            return Result<CompanyAccount>.Failure(CompanyRegistrationError.CompanyAccountNotFound, "The company account was not found.");

        companyAccount.Deactivate();
        companyAccountRepository.Update(companyAccount);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<CompanyAccount>.Success(companyAccount);
    }
}
