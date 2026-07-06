using Microsoft.EntityFrameworkCore;
using Watchgate.Locksight.Platform.Iam.Application.CommandServices;
using Watchgate.Locksight.Platform.Iam.Application.Internal.OutboundServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Application.Model;
using Watchgate.Locksight.Platform.Shared.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Application.Internal.CommandServices;

public class UserAccessCommandService(
    IUserRepository userRepository,
    IUserInvitationRepository invitationRepository,
    IUserAccessProfileRepository accessProfileRepository,
    IHashingService hashingService,
    IUnitOfWork unitOfWork) : IUserAccessCommandService
{
    public async Task<Result<UserAccessProfile>> Handle(CreateTeamUserCommand command, CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<UserAccessProfile>.Failure(IamError.EmailAlreadyRegistered, "Email already registered.");

        var user = new User(command.FullName, new EmailAddress(command.Email),
            hashingService.HashPassword(command.Password), new CompanyId(command.CompanyId), command.Role);
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        var profile = new UserAccessProfile(user.Id, command.CompanyId, command.Role, command.Permissions);
        if (command.ZoneId.HasValue) profile.RestrictToZone(command.ZoneId.Value);
        await accessProfileRepository.AddAsync(profile, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<UserAccessProfile>.Success(profile);
    }

    public async Task<Result<UserInvitation>> Handle(InviteUserCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var invitation = new UserInvitation(command.CompanyId, command.Email, command.Role, command.Permissions, command.ZoneId);
            await invitationRepository.AddAsync(invitation, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<UserInvitation>.Success(invitation);
        }
        catch (OperationCanceledException)
        {
            return Result<UserInvitation>.Failure(IamError.OperationCancelled, "Operation was cancelled.");
        }
        catch (DbUpdateException)
        {
            return Result<UserInvitation>.Failure(IamError.DatabaseError, "A database error occurred.");
        }
        catch (Exception)
        {
            return Result<UserInvitation>.Failure(IamError.InternalServerError, "An unexpected error occurred.");
        }
    }

    public async Task<Result<UserInvitation>> Handle(AcceptInvitationCommand command, CancellationToken cancellationToken = default)
    {
        var invitation = await invitationRepository.FindByTokenAsync(command.Token, cancellationToken);
        if (invitation is null || invitation.Status != "PENDING" || invitation.ExpiresAt < DateTime.UtcNow)
            return Result<UserInvitation>.Failure(IamError.InvalidCredentials, "Invitation token is invalid or expired.");

        invitation.Accept();
        invitationRepository.Update(invitation);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<UserInvitation>.Success(invitation);
    }

    public async Task<Result<UserAccessProfile>> Handle(AssignUserAccessCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result<UserAccessProfile>.Failure(IamError.UserNotFound, $"User with id {command.UserId} was not found.");

        var profile = await accessProfileRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (profile is null)
        {
            profile = new UserAccessProfile(command.UserId, command.CompanyId, command.Role, command.Permissions);
            await accessProfileRepository.AddAsync(profile, cancellationToken);
        }
        else
        {
            profile.AssignRole(command.Role, command.Permissions);
            accessProfileRepository.Update(profile);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<UserAccessProfile>.Success(profile);
    }

    public async Task<Result<UserAccessProfile>> Handle(RestrictUserZoneAccessCommand command, CancellationToken cancellationToken = default)
    {
        var profile = await accessProfileRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (profile is null)
            return Result<UserAccessProfile>.Failure(IamError.UserNotFound, $"Access profile for user {command.UserId} was not found.");

        profile.RestrictToZone(command.ZoneId);
        accessProfileRepository.Update(profile);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<UserAccessProfile>.Success(profile);
    }

    public async Task<Result<UserAccessProfile>> Handle(RevokeUserAccessCommand command, CancellationToken cancellationToken = default)
    {
        var profile = await accessProfileRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (profile is null)
            return Result<UserAccessProfile>.Failure(IamError.UserNotFound, $"Access profile for user {command.UserId} was not found.");

        profile.Revoke();
        accessProfileRepository.Update(profile);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<UserAccessProfile>.Success(profile);
    }

    public async Task<Result<UserAccessProfile>> Handle(UpdateNotificationPreferencesCommand command, CancellationToken cancellationToken = default)
    {
        var profile = await accessProfileRepository.FindByUserIdAsync(command.UserId, cancellationToken);
        if (profile is null)
            return Result<UserAccessProfile>.Failure(IamError.UserNotFound, $"Access profile for user {command.UserId} was not found.");

        profile.UpdateNotificationPreferences(command.EmailEnabled, command.PushEnabled, command.CriticalOnly);
        accessProfileRepository.Update(profile);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<UserAccessProfile>.Success(profile);
    }
}
