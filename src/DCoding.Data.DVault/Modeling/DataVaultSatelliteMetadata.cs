using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes the descriptive metadata associated with a hub or link parent.
/// </summary>
public sealed class DataVaultSatelliteMetadata {
  /// <summary>
  /// Initializes a new satellite metadata declaration.
  /// </summary>
  public DataVaultSatelliteMetadata(
      string name,
      DataVaultMetadataReference parent,
      IEnumerable<string> descriptiveAttributeNames)
      : this(name, parent, descriptiveAttributeNames, [], requireDrivingKeyNames: false, []) {
  }

  /// <summary>
  /// Initializes a new multi-active satellite metadata declaration.
  /// </summary>
  public DataVaultSatelliteMetadata(
      string name,
      DataVaultMetadataReference parent,
      IEnumerable<string> descriptiveAttributeNames,
      IEnumerable<string> drivingKeyNames)
      : this(name, parent, descriptiveAttributeNames, drivingKeyNames, requireDrivingKeyNames: true, []) {
  }

  /// <summary>
  /// Initializes a new satellite metadata declaration with optional driving keys and marked personal-data payload fields.
  /// </summary>
  public DataVaultSatelliteMetadata(
      string name,
      DataVaultMetadataReference parent,
      IEnumerable<string> descriptiveAttributeNames,
      IEnumerable<string> drivingKeyNames,
      IEnumerable<DataVaultSatellitePersonalDataMetadata> personalDataFields)
      : this(name, parent, descriptiveAttributeNames, drivingKeyNames, requireDrivingKeyNames: false, personalDataFields) {
  }

  private DataVaultSatelliteMetadata(
      string name,
      DataVaultMetadataReference parent,
      IEnumerable<string> descriptiveAttributeNames,
      IEnumerable<string> drivingKeyNames,
      bool requireDrivingKeyNames,
      IEnumerable<DataVaultSatellitePersonalDataMetadata> personalDataFields) {
    Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
    ArgumentNullException.ThrowIfNull(parent);

    Parent = parent;
    DescriptiveAttributeNames = DataVaultMetadataValidation.RequireNames(
        descriptiveAttributeNames,
        nameof(descriptiveAttributeNames),
        "A satellite requires at least one descriptive attribute name.");
    DrivingKeyNames = ReadDrivingKeyNames(drivingKeyNames, DescriptiveAttributeNames, requireDrivingKeyNames);
    PersonalDataFields = RequirePersonalDataFields(
        personalDataFields,
        DescriptiveAttributeNames,
        DrivingKeyNames);
    PayloadColumns = DescriptiveAttributeNames
        .Select(columnName => new DataVaultSatellitePayloadMetadata(columnName))
        .ToArray();
    HashDiffMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashDiff);
    LoadTimestampMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.LoadTimestamp);
    RecordSourceMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.RecordSource);
    TechnicalMetadataColumns =
    [
        HashDiffMetadata,
        LoadTimestampMetadata,
        RecordSourceMetadata,
    ];
  }

  /// <summary>
  /// Gets the satellite name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the hub or link parent associated with the satellite.
  /// </summary>
  public DataVaultMetadataReference Parent { get; }

  /// <summary>
  /// Gets the descriptive attribute names carried by the satellite.
  /// </summary>
  public IReadOnlyList<string> DescriptiveAttributeNames { get; }

  /// <summary>
  /// Gets the declared multi-active driving-key names in canonical declaration order.
  /// </summary>
  public IReadOnlyList<string> DrivingKeyNames { get; }

  /// <summary>
  /// Gets marked personal-data payload fields in canonical declaration order.
  /// </summary>
  public IReadOnlyList<DataVaultSatellitePersonalDataMetadata> PersonalDataFields { get; }

  /// <summary>
  /// Gets the payload column metadata carried by the satellite.
  /// </summary>
  public IReadOnlyList<DataVaultSatellitePayloadMetadata> PayloadColumns { get; }

  /// <summary>
  /// Gets the required hash-diff technical metadata for the satellite.
  /// </summary>
  public TechnicalMetadataColumnContract HashDiffMetadata { get; }

  /// <summary>
  /// Gets the required load-timestamp technical metadata for the satellite.
  /// </summary>
  public TechnicalMetadataColumnContract LoadTimestampMetadata { get; }

  /// <summary>
  /// Gets the required record-source technical metadata for the satellite.
  /// </summary>
  public TechnicalMetadataColumnContract RecordSourceMetadata { get; }

  /// <summary>
  /// Gets the required technical metadata columns for satellite records.
  /// </summary>
  public IReadOnlyList<TechnicalMetadataColumnContract> TechnicalMetadataColumns { get; }

  private static IReadOnlyList<string> ReadDrivingKeyNames(
      IEnumerable<string> drivingKeyNames,
      IReadOnlyList<string> payloadNames,
      bool requireDrivingKeyNames) {
    var values = requireDrivingKeyNames
        ? DataVaultMetadataValidation.RequireNames(
            drivingKeyNames,
            nameof(drivingKeyNames),
            "A multi-active satellite requires at least one driving-key name.")
        : ReadOptionalDrivingKeyNames(drivingKeyNames);
    var drivingKeyNameSet = new HashSet<string>(StringComparer.Ordinal);
    foreach (var value in values) {
      if (!drivingKeyNameSet.Add(value)) {
        throw new ArgumentException(
            "Multi-active satellite driving-key names must be unique by ordinal comparison.",
            nameof(drivingKeyNames));
      }
    }

    var payloadNameSet = payloadNames.ToHashSet(StringComparer.Ordinal);
    foreach (var value in values) {
      if (payloadNameSet.Contains(value)) {
        throw new ArgumentException(
            "Multi-active satellite driving-key names must not overlap descriptive attribute names.",
            nameof(drivingKeyNames));
      }
    }

    return values;
  }

  private static IReadOnlyList<string> ReadOptionalDrivingKeyNames(IEnumerable<string> drivingKeyNames) {
    ArgumentNullException.ThrowIfNull(drivingKeyNames);

    var values = drivingKeyNames.ToArray();
    foreach (var value in values) {
      ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(drivingKeyNames));
    }

    return values;
  }

  private static IReadOnlyList<DataVaultSatellitePersonalDataMetadata> RequirePersonalDataFields(
      IEnumerable<DataVaultSatellitePersonalDataMetadata> personalDataFields,
      IReadOnlyList<string> payloadNames,
      IReadOnlyList<string> drivingKeyNames) {
    ArgumentNullException.ThrowIfNull(personalDataFields);

    var values = personalDataFields.ToArray();
    foreach (var value in values) {
      if (value is null) {
        throw new ArgumentException(
            "Satellite personal-data metadata declarations must not contain null values.",
            nameof(personalDataFields));
      }
    }

    var payloadNameSet = payloadNames.ToHashSet(StringComparer.Ordinal);
    var drivingKeyNameSet = drivingKeyNames.ToHashSet(StringComparer.Ordinal);
    var fields = new HashSet<string>(StringComparer.Ordinal);
    var aliases = new HashSet<string>(StringComparer.Ordinal);

    foreach (var value in values) {
      if (!fields.Add(value.FieldName)) {
        throw new ArgumentException(
            "Satellite personal-data field '" + value.FieldName + "' is declared more than once.",
            nameof(personalDataFields));
      }

      if (!aliases.Add(value.EncryptedPayloadAlias)) {
        throw new ArgumentException(
            "Satellite encrypted-payload alias '" + value.EncryptedPayloadAlias + "' is declared more than once.",
            nameof(personalDataFields));
      }

      if (payloadNameSet.Contains(value.FieldName)) {
        continue;
      }

      if (drivingKeyNameSet.Contains(value.FieldName)) {
        throw new ArgumentException(
            "Satellite personal-data field '" + value.FieldName + "' references a driving key instead of a payload field.",
            nameof(personalDataFields));
      }

      throw new ArgumentException(
          "Satellite personal-data field '" + value.FieldName + "' does not match a declared payload field.",
          nameof(personalDataFields));
    }

    return values;
  }
}
