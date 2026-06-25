using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Creates provider-neutral structural privacy coverage reports from configured aliases and EF model mappings.
/// </summary>
public static class DataVaultPrivacyCoverageReporter {
  /// <summary>
  /// Analyzes encrypted-payload alias coverage using the model from a DbContext without querying the database.
  /// </summary>
  /// <param name="configuration">The opt-in privacy configuration registered by the application.</param>
  /// <param name="context">The DbContext whose EF model should be inspected.</param>
  /// <returns>A deterministic redaction-safe privacy coverage report.</returns>
  public static DataVaultPrivacyCoverageReport Analyze(
      IDataVaultPrivacyConfiguration configuration,
      DbContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return Analyze(configuration, context.Model);
  }

  /// <summary>
  /// Analyzes encrypted-payload alias coverage using an EF model without querying the database.
  /// </summary>
  /// <param name="configuration">The opt-in privacy configuration registered by the application.</param>
  /// <param name="model">The EF model whose mapped properties should be inspected.</param>
  /// <returns>A deterministic redaction-safe privacy coverage report.</returns>
  public static DataVaultPrivacyCoverageReport Analyze(
      IDataVaultPrivacyConfiguration configuration,
      IModel model) {
    ArgumentNullException.ThrowIfNull(configuration);
    ArgumentNullException.ThrowIfNull(model);

    var coveredPropertiesByAlias = CreateCoveredPropertiesByAlias(model);
    var aliasCoverages = configuration.EncryptedPayloadAliases
        .Distinct(StringComparer.Ordinal)
        .Order(StringComparer.Ordinal)
        .Select(alias => CreateAliasCoverage(alias, coveredPropertiesByAlias))
        .ToArray();

    return new DataVaultPrivacyCoverageReport(
        ClassifyKeyProviderPosture(configuration.KeyProvider),
        aliasCoverages);
  }

  private static DataVaultPrivacyAliasCoverage CreateAliasCoverage(
      string encryptedPayloadAlias,
      IReadOnlyDictionary<string, IReadOnlyList<DataVaultPrivacyCoveredProperty>> coveredPropertiesByAlias) {
    var coveredProperties = coveredPropertiesByAlias.TryGetValue(encryptedPayloadAlias, out var properties)
        ? properties
        : Array.Empty<DataVaultPrivacyCoveredProperty>();

    var status = coveredProperties.Count == 0
        ? DataVaultPrivacyAliasCoverageStatus.RegisteredButUnmapped
        : DataVaultPrivacyAliasCoverageStatus.Covered;

    return new DataVaultPrivacyAliasCoverage(encryptedPayloadAlias, status, coveredProperties);
  }

  private static Dictionary<string, IReadOnlyList<DataVaultPrivacyCoveredProperty>> CreateCoveredPropertiesByAlias(
      IModel model) {
    var coveredPropertiesByAlias = new Dictionary<string, List<DataVaultPrivacyCoveredProperty>>(
        StringComparer.Ordinal);

    foreach (var entityType in model.GetEntityTypes().OrderBy(entityType => entityType.Name, StringComparer.Ordinal)) {
      foreach (var property in entityType.GetProperties().OrderBy(property => property.Name, StringComparer.Ordinal)) {
        if (property.GetValueConverter() is not DataVaultEncryptedPayloadValueConverter converter) {
          continue;
        }

        if (!coveredPropertiesByAlias.TryGetValue(converter.EncryptedPayloadAlias, out var coveredProperties)) {
          coveredProperties = [];
          coveredPropertiesByAlias.Add(converter.EncryptedPayloadAlias, coveredProperties);
        }

        coveredProperties.Add(new DataVaultPrivacyCoveredProperty(entityType.Name, property.Name));
      }
    }

    return coveredPropertiesByAlias.ToDictionary(
        pair => pair.Key,
        pair => (IReadOnlyList<DataVaultPrivacyCoveredProperty>)pair.Value.ToArray(),
        StringComparer.Ordinal);
  }

  private static DataVaultPrivacyKeyProviderPosture ClassifyKeyProviderPosture(
      IDataVaultPrivacyKeyProvider? keyProvider) {
    return keyProvider switch {
      null => DataVaultPrivacyKeyProviderPosture.None,
      IDataVaultEncryptedPayloadKeyProvider => DataVaultPrivacyKeyProviderPosture.EncryptedPayloadCapable,
      _ => DataVaultPrivacyKeyProviderPosture.MarkerOnly,
    };
  }
}
