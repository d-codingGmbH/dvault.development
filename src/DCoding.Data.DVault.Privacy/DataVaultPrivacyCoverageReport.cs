using System.Globalization;
using System.Text;

namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Structured and displayable provider-neutral report for encrypted-payload alias coverage.
/// </summary>
public sealed record DataVaultPrivacyCoverageReport(
    DataVaultPrivacyKeyProviderPosture KeyProviderPosture,
    IReadOnlyList<DataVaultPrivacyAliasCoverage> AliasCoverages) {
  /// <summary>
  /// Gets a value indicating whether at least one registered alias is not mapped by an encrypted-payload converter.
  /// </summary>
  public bool HasUnmappedAliases => AliasCoverages.Any(
      coverage => coverage.Status == DataVaultPrivacyAliasCoverageStatus.RegisteredButUnmapped);

  /// <summary>
  /// Produces deterministic redaction-safe privacy coverage output for logs, tests, or automation.
  /// </summary>
  /// <returns>A stable display string that names aliases and mapped EF properties without key or payload material.</returns>
  public string ToDisplayString() {
    var coveredCount = AliasCoverages.Count(
        coverage => coverage.Status == DataVaultPrivacyAliasCoverageStatus.Covered);
    var unmappedCount = AliasCoverages.Count(
        coverage => coverage.Status == DataVaultPrivacyAliasCoverageStatus.RegisteredButUnmapped);

    var builder = new StringBuilder();
    builder.Append("DVault privacy coverage: aliases ");
    builder.Append(AliasCoverages.Count.ToString(CultureInfo.InvariantCulture));
    builder.Append(", covered ");
    builder.Append(coveredCount.ToString(CultureInfo.InvariantCulture));
    builder.Append(", registered-but-unmapped ");
    builder.Append(unmappedCount.ToString(CultureInfo.InvariantCulture));
    builder.Append(", key provider ");
    builder.Append(FormatKeyProviderPosture(KeyProviderPosture));
    builder.Append('.');

    foreach (var coverage in AliasCoverages) {
      builder.AppendLine();
      builder.Append("- ");
      builder.Append(FormatAliasStatus(coverage.Status));
      builder.Append(' ');
      builder.Append(coverage.EncryptedPayloadAlias);
      builder.Append(": ");
      if (coverage.CoveredProperties.Count > 0) {
        builder.Append(string.Join(", ", coverage.CoveredProperties.Select(FormatCoveredProperty)));
      }
      else {
        builder.Append("no mapped properties use DataVaultEncryptedPayloadValueConverter for this alias.");
      }
    }

    return builder.ToString();
  }

  private static string FormatCoveredProperty(DataVaultPrivacyCoveredProperty property) {
    return property.EntityTypeName + "." + property.PropertyName;
  }

  private static string FormatAliasStatus(DataVaultPrivacyAliasCoverageStatus status) {
    return status switch {
      DataVaultPrivacyAliasCoverageStatus.Covered => "covered",
      DataVaultPrivacyAliasCoverageStatus.RegisteredButUnmapped => "registered-but-unmapped",
      _ => status.ToString().ToLowerInvariant(),
    };
  }

  private static string FormatKeyProviderPosture(DataVaultPrivacyKeyProviderPosture posture) {
    return posture switch {
      DataVaultPrivacyKeyProviderPosture.None => "none",
      DataVaultPrivacyKeyProviderPosture.MarkerOnly => "marker-only",
      DataVaultPrivacyKeyProviderPosture.EncryptedPayloadCapable => "encrypted-payload-capable",
      _ => posture.ToString().ToLowerInvariant(),
    };
  }
}
