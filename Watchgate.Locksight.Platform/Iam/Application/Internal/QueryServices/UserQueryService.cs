using Watchgate.Locksight.Platform.Iam.Application.QueryServices;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Queries;
using Watchgate.Locksight.Platform.Iam.Domain.Repositories;

namespace Watchgate.Locksight.Platform.Iam.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default) =>
        await userRepository.FindByIdAsync(query.UserId, cancellationToken);

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken = default) =>
        await userRepository.ListAsync(cancellationToken);

    public async Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken = default) =>
        await userRepository.FindByEmailAsync(query.Email, cancellationToken);
}
