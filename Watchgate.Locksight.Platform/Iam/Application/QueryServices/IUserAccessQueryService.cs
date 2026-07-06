using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.Iam.Application.QueryServices;

public interface IUserAccessQueryService
{
    Task<Result<UserAccessProfile>> Handle(GetUserAccessProfileByUserIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserAccessProfile>>> Handle(GetUserAccessProfilesByCompanyIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserInvitation>>> Handle(GetInvitationsByCompanyIdQuery query, CancellationToken cancellationToken = default);
}
