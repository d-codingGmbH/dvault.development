using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

/// <summary>
/// Builds provider-neutral Data Vault metadata from additive EF Core code-first declarations.
/// </summary>
public sealed class DataVaultCodeFirstModelBuilder {
  private static readonly DefaultNamingPolicy NamingPolicy = DefaultNamingPolicy.Instance;

  private readonly List<HubDeclaration> _hubs = [];
  private readonly List<LinkDeclaration> _links = [];

  internal DataVaultCodeFirstModelBuilder() {
  }

  /// <summary>
  /// Declares a hub for one CLR entity type using the default logical hub name from the CLR type name.
  /// </summary>
  /// <typeparam name="TEntity">The CLR entity type represented by the hub.</typeparam>
  /// <param name="configure">The optional hub configuration callback.</param>
  /// <returns>The hub builder for additional fluent configuration.</returns>
  public DataVaultCodeFirstHubBuilder<TEntity> Hub<TEntity>(
      Action<DataVaultCodeFirstHubBuilder<TEntity>>? configure = null)
      where TEntity : class {
    var declaration = new HubDeclaration(typeof(TEntity), typeof(TEntity).Name);
    _hubs.Add(declaration);

    var builder = new DataVaultCodeFirstHubBuilder<TEntity>(declaration);
    configure?.Invoke(builder);

    return builder;
  }

  /// <summary>
  /// Declares a link whose default relationship name is derived from participant order.
  /// </summary>
  /// <param name="configure">The link participant configuration callback.</param>
  /// <returns>The link builder for additional fluent configuration.</returns>
  public DataVaultCodeFirstLinkBuilder Link(Action<DataVaultCodeFirstLinkBuilder> configure) {
    return AddLink(relationshipName: null, configure);
  }

  /// <summary>
  /// Declares a link with an explicit relationship name.
  /// </summary>
  /// <param name="relationshipName">The provider-neutral relationship name.</param>
  /// <param name="configure">The link participant configuration callback.</param>
  /// <returns>The link builder for additional fluent configuration.</returns>
  public DataVaultCodeFirstLinkBuilder Link(
      string relationshipName,
      Action<DataVaultCodeFirstLinkBuilder> configure) {
    ArgumentException.ThrowIfNullOrWhiteSpace(relationshipName);

    return AddLink(relationshipName, configure);
  }

  internal DataVaultMetadataModel BuildMetadataModel() {
    var links = _links
        .Select(link => BuildLinkMetadata(link))
        .ToArray();
    ValidateUniqueHubNames();

    var hubs = _hubs
        .Select(hub => new DataVaultHubMetadata(hub.Name, hub.BusinessKeyNames))
        .ToArray();
    var satellites = _hubs
        .SelectMany(hub => hub.Satellites.Select(satellite => CreateSatelliteMetadata(hub, satellite)))
        .ToArray();

    return new DataVaultMetadataModel(hubs, links, satellites);
  }

  private DataVaultCodeFirstLinkBuilder AddLink(
      string? relationshipName,
      Action<DataVaultCodeFirstLinkBuilder> configure) {
    ArgumentNullException.ThrowIfNull(configure);

    var declaration = new LinkDeclaration(relationshipName);
    _links.Add(declaration);

    var builder = new DataVaultCodeFirstLinkBuilder(declaration);
    configure(builder);

    return builder;
  }

  private void ValidateUniqueHubNames() {
    var hubNames = new HashSet<string>(StringComparer.Ordinal);
    foreach (var hub in _hubs) {
      if (!hubNames.Add(hub.Name)) {
        throw new ArgumentException(
            "Code-first Data Vault metadata declares hub logical name '" + hub.Name + "' more than once.",
            "configureModel");
      }
    }
  }

  private static DataVaultSatelliteMetadata CreateSatelliteMetadata(
      HubDeclaration hub,
      SatelliteDeclaration satellite) {
    var hubReference = DataVaultMetadataReference.Hub(hub.Name);
    return satellite.DrivingKeyNames.Count == 0
        ? new DataVaultSatelliteMetadata(satellite.Name, hubReference, satellite.PayloadNames)
        : new DataVaultSatelliteMetadata(
            satellite.Name,
            hubReference,
            satellite.PayloadNames,
            satellite.DrivingKeyNames);
  }

  private DataVaultLinkMetadata BuildLinkMetadata(LinkDeclaration link) {
    if (link.ParticipantClrTypes.Count < 2) {
      throw LinkValidationException(link, "requires at least two participant declarations.");
    }

    var participantHubs = link.ParticipantClrTypes
        .Select(participantClrType => ResolveParticipantHub(link, participantClrType))
        .ToArray();
    var participantHubNames = new HashSet<string>(StringComparer.Ordinal);
    foreach (var participantHub in participantHubs) {
      if (!participantHubNames.Add(participantHub.Name)) {
        throw LinkValidationException(
            link,
            "declares hub '" +
            participantHub.Name +
            "' more than once. Repeated same-hub participants require explicit participant role or alias support and are not supported by v1 code-first link projection.");
      }
    }

    var linkName = link.RelationshipName ?? DeriveRelationshipName(participantHubs);

    return new DataVaultLinkMetadata(
        linkName,
        participantHubs.Select(participantHub => DataVaultMetadataReference.Hub(participantHub.Name)));
  }

  private HubDeclaration ResolveParticipantHub(LinkDeclaration link, Type participantClrType) {
    var matchingHubs = _hubs
        .Where(hub => hub.ClrType == participantClrType)
        .ToArray();

    if (matchingHubs.Length == 0) {
      throw LinkValidationException(
          link,
          "participant CLR type '" +
          FormatClrType(participantClrType) +
          "' has not been configured as a hub in the same code-first model.");
    }

    if (matchingHubs.Length > 1) {
      throw LinkValidationException(
          link,
          "participant CLR type '" +
          FormatClrType(participantClrType) +
          "' resolves to more than one configured hub in the same code-first model.");
    }

    return matchingHubs[0];
  }

  private static string DeriveRelationshipName(IEnumerable<HubDeclaration> participantHubs) {
    return string.Concat(participantHubs.Select(participantHub => NamingPolicy.NormalizeObjectName(participantHub.Name)));
  }

  private static ArgumentException LinkValidationException(LinkDeclaration link, string message) {
    return new ArgumentException("Code-first link " + FormatLinkName(link) + " " + message, "configureModel");
  }

  private static string FormatLinkName(LinkDeclaration link) {
    return link.RelationshipName is null
        ? "with derived relationship name"
        : "'" + link.RelationshipName + "'";
  }

  private static string FormatClrType(Type clrType) {
    return clrType.FullName ?? clrType.Name;
  }

  internal sealed class HubDeclaration(Type clrType, string name) {
    public Type ClrType { get; } = clrType;

    public string Name { get; } = name;

    public List<string> BusinessKeyNames { get; } = [];

    public List<SatelliteDeclaration> Satellites { get; } = [];
  }

  internal sealed class LinkDeclaration(string? relationshipName) {
    public string? RelationshipName { get; } = relationshipName;

    public List<Type> ParticipantClrTypes { get; } = [];
  }

  internal sealed class SatelliteDeclaration(string name) {
    public string Name { get; } = name;

    public List<string> DrivingKeyNames { get; } = [];

    public List<string> PayloadNames { get; } = [];
  }
}
