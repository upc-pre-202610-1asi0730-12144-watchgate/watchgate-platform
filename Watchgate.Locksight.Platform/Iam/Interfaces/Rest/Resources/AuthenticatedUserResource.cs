namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

public record AuthenticatedUserResource(int Id, string FullName, string Email, string Role, string Token);
