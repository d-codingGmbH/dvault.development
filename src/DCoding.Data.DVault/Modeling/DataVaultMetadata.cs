using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Identifies the Data Vault metadata structures that can be referenced by another metadata declaration.
/// </summary>
public enum DataVaultMetadataReferenceKind
{
    /// <summary>
    /// References a hub metadata declaration.
    /// </summary>
    Hub,

    /// <summary>
    /// References a link metadata declaration.
    /// </summary>
    Link,
}

/// <summary>
/// Represents a named hub or link metadata target.
/// </summary>
public sealed class DataVaultMetadataReference
{
    private DataVaultMetadataReference(DataVaultMetadataReferenceKind kind, string name)
    {
        Kind = kind;
        Name = name;
    }

    /// <summary>
    /// Gets the kind of metadata declaration being referenced.
    /// </summary>
    public DataVaultMetadataReferenceKind Kind { get; }

    /// <summary>
    /// Gets the referenced hub or link name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Creates a reference to hub metadata.
    /// </summary>
    public static DataVaultMetadataReference Hub(string name)
    {
        return new DataVaultMetadataReference(
            DataVaultMetadataReferenceKind.Hub,
            DataVaultMetadataValidation.RequireName(name, nameof(name)));
    }

    /// <summary>
    /// Creates a reference to link metadata.
    /// </summary>
    public static DataVaultMetadataReference Link(string name)
    {
        return new DataVaultMetadataReference(
            DataVaultMetadataReferenceKind.Link,
            DataVaultMetadataValidation.RequireName(name, nameof(name)));
    }
}

/// <summary>
/// Describes one business-key column declared by a Data Vault hub.
/// </summary>
public sealed class DataVaultBusinessKeyMetadata
{
    /// <summary>
    /// Initializes a new business-key metadata declaration.
    /// </summary>
    /// <param name="columnName">The provider-neutral business-key column name.</param>
    public DataVaultBusinessKeyMetadata(string columnName)
    {
        ColumnName = DataVaultMetadataValidation.RequireName(columnName, nameof(columnName));
    }

    /// <summary>
    /// Gets the provider-neutral business-key column name.
    /// </summary>
    public string ColumnName { get; }
}

/// <summary>
/// Describes one participating hub and hash-key reference in a Data Vault link.
/// </summary>
public sealed class DataVaultLinkParticipantMetadata
{
    /// <summary>
    /// Initializes a new link participant metadata declaration.
    /// </summary>
    /// <param name="hubReference">The hub referenced by this link participant.</param>
    public DataVaultLinkParticipantMetadata(DataVaultMetadataReference hubReference)
    {
        HubReference = DataVaultMetadataValidation.RequireHubReference(hubReference, nameof(hubReference));
        HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
    }

    /// <summary>
    /// Gets the participating hub reference.
    /// </summary>
    public DataVaultMetadataReference HubReference { get; }

    /// <summary>
    /// Gets the technical hash-key metadata used to reference the participating hub key.
    /// </summary>
    public TechnicalMetadataColumnContract HashKeyMetadata { get; }
}

/// <summary>
/// Describes one payload column declared by a Data Vault satellite.
/// </summary>
public sealed class DataVaultSatellitePayloadMetadata
{
    /// <summary>
    /// Initializes a new satellite payload metadata declaration.
    /// </summary>
    /// <param name="columnName">The provider-neutral satellite payload column name.</param>
    public DataVaultSatellitePayloadMetadata(string columnName)
    {
        ColumnName = DataVaultMetadataValidation.RequireName(columnName, nameof(columnName));
    }

    /// <summary>
    /// Gets the provider-neutral satellite payload column name.
    /// </summary>
    public string ColumnName { get; }
}

/// <summary>
/// Describes the identifying metadata for a Data Vault hub.
/// </summary>
public sealed class DataVaultHubMetadata
{
    /// <summary>
    /// Initializes a new hub metadata declaration.
    /// </summary>
    public DataVaultHubMetadata(string name, IEnumerable<string> businessKeyNames)
    {
        Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
        BusinessKeyNames = DataVaultMetadataValidation.RequireNames(
            businessKeyNames,
            nameof(businessKeyNames),
            "A hub requires at least one business-key name.");
        BusinessKeyColumns = BusinessKeyNames
            .Select(columnName => new DataVaultBusinessKeyMetadata(columnName))
            .ToArray();
        HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
        LoadTimestampMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.LoadTimestamp);
        RecordSourceMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.RecordSource);
        TechnicalMetadataColumns =
        [
            HashKeyMetadata,
            LoadTimestampMetadata,
            RecordSourceMetadata,
        ];
    }

    /// <summary>
    /// Gets the hub name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the business-key names that identify the hub.
    /// </summary>
    public IReadOnlyList<string> BusinessKeyNames { get; }

    /// <summary>
    /// Gets the business-key column metadata that identifies the hub.
    /// </summary>
    public IReadOnlyList<DataVaultBusinessKeyMetadata> BusinessKeyColumns { get; }

    /// <summary>
    /// Gets the required hash-key technical metadata for the hub.
    /// </summary>
    public TechnicalMetadataColumnContract HashKeyMetadata { get; }

    /// <summary>
    /// Gets the required load-timestamp technical metadata for the hub.
    /// </summary>
    public TechnicalMetadataColumnContract LoadTimestampMetadata { get; }

    /// <summary>
    /// Gets the required record-source technical metadata for the hub.
    /// </summary>
    public TechnicalMetadataColumnContract RecordSourceMetadata { get; }

    /// <summary>
    /// Gets the required technical metadata columns for hub records.
    /// </summary>
    public IReadOnlyList<TechnicalMetadataColumnContract> TechnicalMetadataColumns { get; }

    /// <summary>
    /// Creates a reference to this hub metadata declaration.
    /// </summary>
    public DataVaultMetadataReference ToReference()
    {
        return DataVaultMetadataReference.Hub(Name);
    }
}

/// <summary>
/// Describes the hub endpoints that participate in a Data Vault link.
/// </summary>
public sealed class DataVaultLinkMetadata
{
    /// <summary>
    /// Initializes a new link metadata declaration.
    /// </summary>
    public DataVaultLinkMetadata(string name, IEnumerable<DataVaultMetadataReference> endpoints)
    {
        Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
        Participants = RequireHubParticipants(endpoints);
        Endpoints = Participants.Select(participant => participant.HubReference).ToArray();
        HashKeyMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.HashKey);
        LoadTimestampMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.LoadTimestamp);
        RecordSourceMetadata = TechnicalMetadataColumnContract.ForRole(TechnicalMetadataColumnRole.RecordSource);
        TechnicalMetadataColumns =
        [
            HashKeyMetadata,
            LoadTimestampMetadata,
            RecordSourceMetadata,
        ];
    }

    /// <summary>
    /// Gets the link name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the hub endpoints connected by the link.
    /// </summary>
    public IReadOnlyList<DataVaultMetadataReference> Endpoints { get; }

    /// <summary>
    /// Gets the participating hub and hash-key metadata connected by the link.
    /// </summary>
    public IReadOnlyList<DataVaultLinkParticipantMetadata> Participants { get; }

    /// <summary>
    /// Gets the required relationship hash-key technical metadata for the link.
    /// </summary>
    public TechnicalMetadataColumnContract HashKeyMetadata { get; }

    /// <summary>
    /// Gets the required load-timestamp technical metadata for the link.
    /// </summary>
    public TechnicalMetadataColumnContract LoadTimestampMetadata { get; }

    /// <summary>
    /// Gets the required record-source technical metadata for the link.
    /// </summary>
    public TechnicalMetadataColumnContract RecordSourceMetadata { get; }

    /// <summary>
    /// Gets the required technical metadata columns for link records.
    /// </summary>
    public IReadOnlyList<TechnicalMetadataColumnContract> TechnicalMetadataColumns { get; }

    /// <summary>
    /// Creates a reference to this link metadata declaration.
    /// </summary>
    public DataVaultMetadataReference ToReference()
    {
        return DataVaultMetadataReference.Link(Name);
    }

    private static IReadOnlyList<DataVaultLinkParticipantMetadata> RequireHubParticipants(IEnumerable<DataVaultMetadataReference> endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var values = endpoints.ToArray();
        if (values.Length < 2)
        {
            throw new ArgumentException("A link requires at least two hub endpoints.", nameof(endpoints));
        }

        foreach (var endpoint in values)
        {
            DataVaultMetadataValidation.RequireHubReference(endpoint, nameof(endpoints));
        }

        return values
            .Select(endpoint => new DataVaultLinkParticipantMetadata(endpoint))
            .ToArray();
    }
}

/// <summary>
/// Describes the descriptive metadata associated with a hub or link parent.
/// </summary>
public sealed class DataVaultSatelliteMetadata
{
    /// <summary>
    /// Initializes a new satellite metadata declaration.
    /// </summary>
    public DataVaultSatelliteMetadata(
        string name,
        DataVaultMetadataReference parent,
        IEnumerable<string> descriptiveAttributeNames)
    {
        Name = DataVaultMetadataValidation.RequireName(name, nameof(name));
        ArgumentNullException.ThrowIfNull(parent);

        Parent = parent;
        DescriptiveAttributeNames = DataVaultMetadataValidation.RequireNames(
            descriptiveAttributeNames,
            nameof(descriptiveAttributeNames),
            "A satellite requires at least one descriptive attribute name.");
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
}

internal static class DataVaultMetadataValidation
{
    public static string RequireName(string name, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, parameterName);

        return name;
    }

    public static IReadOnlyList<string> RequireNames(
        IEnumerable<string> names,
        string parameterName,
        string emptyMessage)
    {
        ArgumentNullException.ThrowIfNull(names, parameterName);

        var values = names.ToArray();
        if (values.Length == 0)
        {
            throw new ArgumentException(emptyMessage, parameterName);
        }

        foreach (var value in values)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
        }

        return values;
    }

    public static DataVaultMetadataReference RequireHubReference(
        DataVaultMetadataReference reference,
        string parameterName)
    {
        ArgumentNullException.ThrowIfNull(reference, parameterName);

        if (reference.Kind != DataVaultMetadataReferenceKind.Hub)
        {
            throw new ArgumentException("A link participant must reference a hub.", parameterName);
        }

        return reference;
    }
}
