namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;

public record RestrictUserZoneAccessCommand(int UserId, int ZoneId);
