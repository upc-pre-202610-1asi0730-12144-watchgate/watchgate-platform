using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;
using Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;
using Watchgate.Locksight.Platform.Shared.Application.Model;

namespace Watchgate.Locksight.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken);
    Task<Result<(User user, string token)>> Handle(SignUpCommand command, CancellationToken cancellationToken);
    Task<Result<User>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken);
}
