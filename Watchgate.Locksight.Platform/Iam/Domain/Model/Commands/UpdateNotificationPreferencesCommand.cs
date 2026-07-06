namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;

public record UpdateNotificationPreferencesCommand(int UserId, bool EmailEnabled, bool PushEnabled, bool CriticalOnly);
