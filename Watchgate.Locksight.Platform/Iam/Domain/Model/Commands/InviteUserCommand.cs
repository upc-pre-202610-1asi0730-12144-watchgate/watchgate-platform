namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;

public record InviteUserCommand(int CompanyId, string Email, string Role, string Permissions, int? ZoneId);
