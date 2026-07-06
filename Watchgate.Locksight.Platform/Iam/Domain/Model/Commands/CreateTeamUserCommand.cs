namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;

public record CreateTeamUserCommand(int CompanyId, string FullName, string Email, string Password, string Role, string Permissions, int? ZoneId);
