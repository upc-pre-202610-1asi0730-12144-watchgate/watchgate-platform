namespace Watchgate.Locksight.Platform.Iam.Interfaces.Rest.Resources;

public record SignUpResource(string FullName, string Email, string Password, string TradeName, string TaxId);
