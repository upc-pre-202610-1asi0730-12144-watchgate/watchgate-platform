using System.Text.RegularExpressions;

namespace Watchgate.Locksight.Platform.Iam.Domain.Model.ValueObjects;

/// <summary>Value Object that wraps a validated, normalized email address, avoiding primitive obsession on raw strings.</summary>
public sealed partial record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email address cannot be empty.", nameof(value));

        var normalized = value.Trim();
        if (!EmailRegex().IsMatch(normalized))
            throw new ArgumentException($"'{value}' is not a valid email address.", nameof(value));

        Value = normalized.ToLowerInvariant();
    }

    public static implicit operator string(EmailAddress email) => email.Value;
    public static implicit operator EmailAddress(string value) => new(value);

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
