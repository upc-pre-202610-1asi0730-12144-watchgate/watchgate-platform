namespace Watchgate.Locksight.Platform.Iam.Domain.Model.Commands;

public record SignUpCommand(string FullName, string Email, string Password, string TradeName, string TaxId);
