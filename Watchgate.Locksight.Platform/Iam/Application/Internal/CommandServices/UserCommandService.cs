using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Watchgate.Locksight.Platform.Iam.Application.CommandServices;
using Watchgate.Locksight.Platform.Iam.Application.Internal.OutboundServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Resources.Errors;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    ICompanyRepository companyRepository,
    IUserAccessProfileRepository userAccessProfileRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IUserCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<(User user, string token)>> Handle(SignInCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User user, string token)>.Failure(IamError.InvalidCredentials,
                _localizer[nameof(IamError.InvalidCredentials)]);

        var token = tokenService.GenerateToken(user);
        return Result<(User user, string token)>.Success((user, token));
    }

    public async Task<Result<(User user, string token)>> Handle(SignUpCommand command,
        CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<(User user, string token)>.Failure(IamError.EmailAlreadyRegistered,
                _localizer[nameof(IamError.EmailAlreadyRegistered), command.Email]);

        try
        {
            var company = new Company(command.TradeName, command.TaxId);
            await companyRepository.AddAsync(company, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            var hashedPassword = hashingService.HashPassword(command.Password);
            var user = new User(command.FullName, command.Email, hashedPassword, company.Id, "Administrator");
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            var accessProfile = new UserAccessProfile(
                user.Id,
                company.Id,
                "Administrator",
                "WAREHOUSES_MANAGE,SENSORS_MANAGE,ALERTS_MANAGE,REPORTS_VIEW,BILLING_MANAGE,TEAM_MANAGE");
            await userAccessProfileRepository.AddAsync(accessProfile, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            var token = tokenService.GenerateToken(user);
            return Result<(User user, string token)>.Success((user, token));
        }
        catch (OperationCanceledException)
        {
            return Result<(User user, string token)>.Failure(IamError.OperationCancelled,
                _localizer[nameof(IamError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<(User user, string token)>.Failure(IamError.DatabaseError,
                _localizer[nameof(IamError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<(User user, string token)>.Failure(IamError.InternalServerError,
                _localizer[nameof(IamError.InternalServerError)]);
        }
    }

    public async Task<Result<User>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result<User>.Failure(IamError.UserNotFound, _localizer[nameof(IamError.UserNotFound)]);

        user.UpdatePassword(hashingService.HashPassword(command.NewPassword));
        userRepository.Update(user);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<User>.Success(user);
    }
}
