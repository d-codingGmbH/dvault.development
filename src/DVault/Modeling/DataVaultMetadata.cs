namespace DVault.Modeling;

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
        Endpoints = RequireHubEndpoints(endpoints);
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
    /// Creates a reference to this link metadata declaration.
    /// </summary>
    public DataVaultMetadataReference ToReference()
    {
        return DataVaultMetadataReference.Link(Name);
    }

    private static IReadOnlyList<DataVaultMetadataReference> RequireHubEndpoints(IEnumerable<DataVaultMetadataReference> endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var values = endpoints.ToArray();
        if (values.Length < 2)
        {
            throw new ArgumentException("A link requires at least two hub endpoints.", nameof(endpoints));
        }

        foreach (var endpoint in values)
        {
            ArgumentNullException.ThrowIfNull(endpoint, nameof(endpoints));

            if (endpoint.Kind != DataVaultMetadataReferenceKind.Hub)
            {
                throw new ArgumentException("A link endpoint must reference a hub.", nameof(endpoints));
            }
        }

        return values;
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
}