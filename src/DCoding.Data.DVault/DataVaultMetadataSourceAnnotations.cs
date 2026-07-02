using System.Security.Cryptography;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultMetadataSourceAnnotations {
  public static bool TryRecordSource(
      ModelBuilder modelBuilder,
      string sourceKind,
      string sourceFingerprint) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(sourceKind);
    ArgumentException.ThrowIfNullOrWhiteSpace(sourceFingerprint);

    var existingKind = modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceKind)?.Value as string;
    var existingFingerprint =
        modelBuilder.Model.FindAnnotation(DataVaultAnnotationNames.MetadataSourceFingerprint)?.Value as string;

    if (existingKind is null && existingFingerprint is null) {
      modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.MetadataSourceKind, sourceKind);
      modelBuilder.Model.SetAnnotation(DataVaultAnnotationNames.MetadataSourceFingerprint, sourceFingerprint);
      return true;
    }

    if (string.Equals(existingFingerprint, sourceFingerprint, StringComparison.Ordinal)) {
      return false;
    }

    throw new InvalidOperationException(
        "DVault metadata source conflict: the EF model already uses source kind '" +
        (existingKind ?? "<unknown>") +
        "', but source kind '" +
        sourceKind +
        "' was also configured. Configure one authoritative DVault metadata source for the model, or make the sources identical.");
  }

  public static string CreateFingerprint(DataVaultMetadataModel metadataModel) {
    ArgumentNullException.ThrowIfNull(metadataModel);

    var builder = new StringBuilder();
    AppendMetadataModel(builder, metadataModel);

    return Hash(builder.ToString());
  }

  public static string CreateFingerprint(DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    var builder = new StringBuilder();
    AppendMetadataModel(builder, CreateMetadataModel(metadataRegistry));
    if (metadataRegistry.ProviderCapabilityProfiles.Count > 0) {
      AppendValue(builder, "provider-profiles");
      foreach (var providerCapabilityProfile in metadataRegistry.ProviderCapabilityProfiles) {
        AppendProviderCapabilityProfile(builder, providerCapabilityProfile);
      }
    }

    return Hash(builder.ToString());
  }

  public static DataVaultMetadataModel CreateMetadataModel(DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    return new DataVaultMetadataModel(
        metadataRegistry.Hubs,
        metadataRegistry.Links,
        metadataRegistry.Satellites,
        metadataRegistry.PointInTimeTables,
        metadataRegistry.Bridges,
        metadataRegistry.Pits);
  }

  public static DataVaultProviderCapabilityProfile SelectProviderCapabilities(
      ModelBuilder modelBuilder,
      DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(modelBuilder);
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    return SelectProviderCapabilities(
        DataVaultProviderCapabilityProfileSelection.Select(modelBuilder),
        metadataRegistry);
  }

  public static DataVaultProviderCapabilityProfile SelectProviderCapabilities(
      DataVaultProviderCapabilityProfile selectedProfile,
      DataVaultMetadataRegistry metadataRegistry) {
    ArgumentNullException.ThrowIfNull(selectedProfile);
    ArgumentNullException.ThrowIfNull(metadataRegistry);

    if (metadataRegistry.TryGetProviderCapabilityProfile(selectedProfile.ProfileName, out var registryProfile) &&
        registryProfile is not null) {
      return registryProfile;
    }

    var importedStorageProfilePrefix = selectedProfile.ProfileName + "-loadts-";
    var importedStorageProfiles = metadataRegistry.ProviderCapabilityProfiles
        .Where(profile => profile.ProfileName.StartsWith(importedStorageProfilePrefix, StringComparison.Ordinal))
        .ToArray();

    return importedStorageProfiles.Length == 1 ? importedStorageProfiles[0] : selectedProfile;
  }

  private static void AppendMetadataModel(StringBuilder builder, DataVaultMetadataModel metadataModel) {
    AppendValue(builder, "hubs");
    foreach (var hub in metadataModel.Hubs) {
      AppendValue(builder, hub.Name);
      AppendValues(builder, hub.BusinessKeyNames);
    }

    AppendValue(builder, "links");
    foreach (var link in metadataModel.Links) {
      AppendValue(builder, link.Name);
      foreach (var participant in link.Participants) {
        AppendReference(builder, participant.HubReference);
        AppendValue(builder, participant.SourceEndpointName);
      }

      AppendValue(builder, "dependent-child-keys");
      AppendValues(builder, link.DependentChildKeyNames);
    }

    AppendValue(builder, "satellites");
    foreach (var satellite in metadataModel.Satellites) {
      AppendValue(builder, satellite.Name);
      AppendReference(builder, satellite.Parent);
      AppendValues(builder, satellite.DescriptiveAttributeNames);
      AppendValues(builder, satellite.DrivingKeyNames);
      AppendValue(builder, "personal-data");
      foreach (var personalData in satellite.PersonalDataFields) {
        AppendValue(builder, personalData.FieldName);
        AppendValue(builder, personalData.EncryptedPayloadAlias);
      }

      AppendValue(builder, "effectivity");
      if (satellite.Effectivity is not null) {
        AppendValue(builder, satellite.Effectivity.EffectiveFromFieldName);
        AppendValue(builder, satellite.Effectivity.EffectiveToFieldName ?? string.Empty);
        AppendValue(builder, satellite.Effectivity.CurrentFlagFieldName ?? string.Empty);
      }
    }

    AppendValue(builder, "point-in-time-tables");
    foreach (var pointInTimeTable in metadataModel.PointInTimeTables) {
      AppendValue(builder, pointInTimeTable.Name);
      AppendReference(builder, pointInTimeTable.HubReference);
      AppendReferences(builder, pointInTimeTable.SatelliteReferences);
    }

    AppendValue(builder, "bridges");
    foreach (var bridge in metadataModel.Bridges) {
      AppendValue(builder, bridge.Name);
      AppendValue(builder, bridge.Kind.ToString());
      AppendReference(builder, bridge.Source);
      foreach (var endpoint in bridge.Endpoints) {
        AppendValue(builder, endpoint.Role.ToString());
        AppendReference(builder, endpoint.HubReference);
        AppendValue(builder, endpoint.SourceEndpointName);
      }

      AppendValue(builder, bridge.ProjectionFeatures.ToString());
    }

    AppendValue(builder, "pits");
    foreach (var pit in metadataModel.Pits) {
      AppendValue(builder, pit.Name);
      AppendReference(builder, pit.Parent);
      foreach (var satellite in pit.Satellites) {
        AppendValue(builder, satellite.SatelliteName);
        AppendValue(builder, satellite.IsMultiActive ? "true" : "false");
      }
    }
  }

  private static void AppendProviderCapabilityProfile(
      StringBuilder builder,
      DataVaultProviderCapabilityProfile providerCapabilityProfile) {
    AppendValue(builder, providerCapabilityProfile.ProfileName);
    AppendValue(builder, providerCapabilityProfile.SqlFunctionSupport.ToString());
    AppendValue(builder, providerCapabilityProfile.ConcurrencySupport.ToString());
    AppendValue(builder, providerCapabilityProfile.MaximumIdentifierLength?.ToString() ?? string.Empty);
    AppendValue(builder, providerCapabilityProfile.AllowsIndexesCoveredByPrimaryKey ? "true" : "false");
    AppendValue(builder, providerCapabilityProfile.UnsupportedIncludedIndexColumnMode.ToString());

    foreach (var typeMapping in providerCapabilityProfile.TypeMappings) {
      AppendValue(builder, typeMapping.LogicalPropertyKind.ToString());
      AppendValue(builder, typeMapping.ModelClrType.AssemblyQualifiedName ?? typeMapping.ModelClrType.FullName ?? typeMapping.ModelClrType.Name);
      AppendValue(builder, typeMapping.NativeStoreType);
      AppendValue(builder, typeMapping.ValueFormat.ToString());
      AppendValue(builder, typeMapping.HashKeyStorageProfile?.ToString() ?? string.Empty);
      AppendValue(builder, typeMapping.StableHashAlgorithmId ?? string.Empty);
      AppendValue(builder, typeMapping.DigestByteLength?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty);
      AppendValue(builder, typeMapping.ConversionBehavior ?? string.Empty);
    }
  }

  private static void AppendReferences(
      StringBuilder builder,
      IEnumerable<DataVaultMetadataReference> references) {
    foreach (var reference in references) {
      AppendReference(builder, reference);
    }
  }

  private static void AppendReference(StringBuilder builder, DataVaultMetadataReference reference) {
    AppendValue(builder, reference.Kind.ToString());
    AppendValue(builder, reference.Name);
  }

  private static void AppendValues(StringBuilder builder, IEnumerable<string> values) {
    foreach (var value in values) {
      AppendValue(builder, value);
    }
  }

  private static void AppendValue(StringBuilder builder, string value) {
    builder.Append(value.Length);
    builder.Append(':');
    builder.Append(value);
    builder.Append(';');
  }

  private static string Hash(string value) {
    return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
  }
}
