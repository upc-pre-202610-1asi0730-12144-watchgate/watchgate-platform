namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;

public record ResetPasswordCommand(int UserId, string NewPassword);
