using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

internal static class DataVaultMetadataValidation {
  public static string RequireName(string name, string parameterName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(name, parameterName);

    return name;
  }

  public static IReadOnlyList<string> RequireNames(
      IEnumerable<string> names,
      string parameterName,
      string emptyMessage) {
    ArgumentNullException.ThrowIfNull(names, parameterName);

    var values = names.ToArray();
    if (values.Length == 0) {
      throw new ArgumentException(emptyMessage, parameterName);
    }

    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
    }

    return values;
  }

  public static DataVaultMetadataReference RequireHubReference(
      DataVaultMetadataReference reference,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(reference, parameterName);

    if (reference.Kind != DataVaultMetadataReferenceKind.Hub) {
      throw new ArgumentException("A link participant must reference a hub.", parameterName);
    }

    return reference;
  }

  public static DataVaultMetadataReference RequireSatelliteReference(
      DataVaultMetadataReference reference,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(reference, parameterName);

    if (reference.Kind != DataVaultMetadataReferenceKind.Satellite) {
      throw new ArgumentException("A point-in-time satellite reference must reference a satellite.", parameterName);
    }

    return reference;
  }
  public static DataVaultMetadataReference RequireLinkReference(
      DataVaultMetadataReference reference,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(reference, parameterName);

    if (reference.Kind != DataVaultMetadataReferenceKind.Link) {
      throw new ArgumentException("A bridge source must reference a link.", parameterName);
    }

    return reference;
  }

  public static DataVaultBridgeKind RequireBridgeKind(DataVaultBridgeKind kind, string parameterName) {
    if (!Enum.IsDefined(typeof(DataVaultBridgeKind), kind)) {
      throw new ArgumentOutOfRangeException(parameterName, kind, "Unsupported Data Vault bridge kind.");
    }

    return kind;
  }

  public static IReadOnlyList<T> RequireItems<T>(
      IEnumerable<T> items,
      string parameterName,
      string emptyMessage)
      where T : class {
    ArgumentNullException.ThrowIfNull(items, parameterName);

    var values = items.ToArray();
    if (values.Length == 0) {
      throw new ArgumentException(emptyMessage, parameterName);
    }
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Metadata declaration collections must not contain null values.", parameterName);
      }
    }

    return values;
  }

  public static IReadOnlyList<T> RequireItems<T>(IEnumerable<T> items, string parameterName)
      where T : class {
    ArgumentNullException.ThrowIfNull(items, parameterName);

    var values = items.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException("Metadata declaration collections must not contain null values.", parameterName);
      }
    }

    return values;
  }
}
