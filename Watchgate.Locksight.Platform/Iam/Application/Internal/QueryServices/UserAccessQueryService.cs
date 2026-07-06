using Watchgate.Locksight.Platform.Iam.Application.QueryServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.Iam.Application.Internal.QueryServices;

public class UserAccessQueryService(
    IUserInvitationRepository invitationRepository,
    IUserAccessProfileRepository accessProfileRepository) : IUserAccessQueryService
{
    public async Task<Result<UserAccessProfile>> Handle(GetUserAccessProfileByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        var profile = await accessProfileRepository.FindByUserIdAsync(query.UserId, cancellationToken);
        return profile is null
            ? Result<UserAccessProfile>.Failure(IamError.UserNotFound, $"Access profile for user {query.UserId} was not found.")
            : Result<UserAccessProfile>.Success(profile);
    }

    public async Task<Result<IEnumerable<UserAccessProfile>>> Handle(GetUserAccessProfilesByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var profiles = await accessProfileRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<UserAccessProfile>>.Success(profiles);
    }

    public async Task<Result<IEnumerable<UserInvitation>>> Handle(GetInvitationsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        var invitations = await invitationRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
        return Result<IEnumerable<UserInvitation>>.Success(invitations);
    }
}
