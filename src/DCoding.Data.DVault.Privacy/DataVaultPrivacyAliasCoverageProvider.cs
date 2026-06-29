using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault.Privacy;

internal sealed class DataVaultPrivacyAliasCoverageProvider(
    IDataVaultPrivacyConfiguration configuration) : IDataVaultPrivacyAliasCoverageProvider {
  public DataVaultPrivacyAliasCoverageReport Analyze(IReadOnlyModel? model) {
    var coveredPropertiesByAlias = model is null
        ? new Dictionary<string, IReadOnlyList<DataVaultPrivacyCoveredPropertyFact>>(StringComparer.Ordinal)
        : CreateCoveredPropertiesByAlias(model);
    var aliasCoverages = configuration.EncryptedPayloadAliases
        .Distinct(StringComparer.Ordinal)
        .Order(StringComparer.Ordinal)
        .Select(alias => CreateAliasCoverage(alias, coveredPropertiesByAlias))
        .ToArray();

    return new DataVaultPrivacyAliasCoverageReport(
        FormatKeyProviderPosture(ClassifyKeyProviderPosture(configuration.KeyProvider)),
        aliasCoverages);
  }

  private static DataVaultPrivacyAliasCoverageFact CreateAliasCoverage(
      string encryptedPayloadAlias,
      IReadOnlyDictionary<string, IReadOnlyList<DataVaultPrivacyCoveredPropertyFact>> coveredPropertiesByAlias) {
    var coveredProperties = coveredPropertiesByAlias.TryGetValue(encryptedPayloadAlias, out var properties)
        ? properties
        : Array.Empty<DataVaultPrivacyCoveredPropertyFact>();
    var status = coveredProperties.Count == 0
        ? "registered-but-unmapped"
        : "covered";

    return new DataVaultPrivacyAliasCoverageFact(encryptedPayloadAlias, status, coveredProperties);
  }

  private static Dictionary<string, IReadOnlyList<DataVaultPrivacyCoveredPropertyFact>> CreateCoveredPropertiesByAlias(
      IReadOnlyModel model) {
    var coveredPropertiesByAlias = new Dictionary<string, List<DataVaultPrivacyCoveredPropertyFact>>(
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

        coveredProperties.Add(new DataVaultPrivacyCoveredPropertyFact(entityType.Name, property.Name));
      }
    }

    return coveredPropertiesByAlias.ToDictionary(
        pair => pair.Key,
        pair => (IReadOnlyList<DataVaultPrivacyCoveredPropertyFact>)pair.Value.ToArray(),
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

  private static string FormatKeyProviderPosture(DataVaultPrivacyKeyProviderPosture posture) {
    return posture switch {
      DataVaultPrivacyKeyProviderPosture.None => "none",
      DataVaultPrivacyKeyProviderPosture.MarkerOnly => "marker-only",
      DataVaultPrivacyKeyProviderPosture.EncryptedPayloadCapable => "encrypted-payload-capable",
      _ => posture.ToString().ToLowerInvariant(),
    };
  }
}
