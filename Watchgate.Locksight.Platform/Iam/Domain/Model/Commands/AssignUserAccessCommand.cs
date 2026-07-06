namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;

public record AssignUserAccessCommand(int UserId, int CompanyId, string Role, string Permissions);
