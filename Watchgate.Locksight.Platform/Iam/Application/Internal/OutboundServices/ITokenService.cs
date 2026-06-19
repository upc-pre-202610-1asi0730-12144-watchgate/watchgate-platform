using Watchgate.Locksight.Platform.Iam.Domain.Model.Aggregates;

namespace Watchgate.Locksight.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
    Task<int?> ValidateToken(string token);
}
