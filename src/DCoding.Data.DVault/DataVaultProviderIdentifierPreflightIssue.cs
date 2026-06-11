using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed record DataVaultProviderIdentifierPreflightIssue(
    DataVaultProviderIdentifierKind Kind,
    string LogicalName,
    string? MetadataName,
    string Scope,
    string Path,
    string ProviderProfileName,
    string FailureClass,
    string Message) {
  public string? AttemptedPhysicalName { get; init; }

  public int? MaximumIdentifierLength { get; init; }
}
