namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

public record UserResource(int Id, string FullName, string Email, string Role, int CompanyId);
