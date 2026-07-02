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
    var hubSatellites = _hubs
        .SelectMany(hub => hub.Satellites.Select(satellite => CreateSatelliteMetadata(hub, satellite)));
    var linkSatellites = _links
        .Zip(links, (linkDeclaration, linkMetadata) => linkDeclaration.Satellites
            .Select(satellite => CreateSatelliteMetadata(linkMetadata, satellite)))
        .SelectMany(satellites => satellites);
    var satellites = hubSatellites
        .Concat(linkSatellites)
        .ToArray();

    return new DataVaultMetadataModel(hubs, links, satellites);
  }

  private DataVaultCodeFirstLinkBuilder AddLink(
      string? relationshipName,
      Action<DataVaultCodeFirstLinkBuilder> configure) {
    ArgumentNullException.ThrowIfNull(configure);

    var declaration = new LinkDeclaration(relationshipName, _hubs.Count);
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
    return CreateSatelliteMetadata(DataVaultMetadataReference.Hub(hub.Name), satellite);
  }

  private static DataVaultSatelliteMetadata CreateSatelliteMetadata(
      DataVaultLinkMetadata link,
      SatelliteDeclaration satellite) {
    return CreateSatelliteMetadata(link.ToReference(), satellite);
  }

  private static DataVaultSatelliteMetadata CreateSatelliteMetadata(
      DataVaultMetadataReference parentReference,
      SatelliteDeclaration satellite) {
    var effectivity = CreateEffectivityMetadata(satellite);

    if (satellite.DrivingKeyNames.Count == 0) {
      return effectivity is null
          ? new DataVaultSatelliteMetadata(satellite.Name, parentReference, satellite.PayloadNames)
          : new DataVaultSatelliteMetadata(satellite.Name, parentReference, satellite.PayloadNames, effectivity);
    }

    return effectivity is null
        ? new DataVaultSatelliteMetadata(
            satellite.Name,
            parentReference,
            satellite.PayloadNames,
            satellite.DrivingKeyNames)
        : new DataVaultSatelliteMetadata(
            satellite.Name,
            parentReference,
            satellite.PayloadNames,
            satellite.DrivingKeyNames,
            effectivity);
  }

  private static DataVaultSatelliteEffectivityMetadata? CreateEffectivityMetadata(SatelliteDeclaration satellite) {
    if (satellite.EffectiveFromName is null &&
        satellite.EffectiveToName is null &&
        satellite.CurrentFlagName is null) {
      return null;
    }

    if (satellite.EffectiveFromName is null) {
      throw new ArgumentException(
          "Code-first satellite '" + satellite.Name + "' declares effectivity metadata without an EffectiveFrom member.",
          "configureModel");
    }

    return new DataVaultSatelliteEffectivityMetadata(
        satellite.EffectiveFromName,
        satellite.EffectiveToName,
        satellite.CurrentFlagName);
  }

  private DataVaultLinkMetadata BuildLinkMetadata(LinkDeclaration link) {
    if (link.Participants.Count < 2) {
      throw LinkValidationException(link, "requires at least two participant declarations.");
    }

    var participantDeclarations = link.Participants
        .Select((participant, ordinal) => new ParticipantHub(
            participant,
            ResolveParticipantHub(link, participant.ClrType),
            ordinal))
        .ToArray();
    ValidateRepeatedParticipantRoles(link, participantDeclarations);
    var producedParticipantNames = CreateProducedParticipantNames(participantDeclarations);
    ValidateProducedParticipantNames(link, producedParticipantNames);
    ValidateLinkDependentChildKeys(link, producedParticipantNames);

    var participantHubs = participantDeclarations
        .Select(participant => participant.Hub)
        .ToArray();
    var linkName = link.RelationshipName ?? DeriveRelationshipName(participantHubs);

    return new DataVaultLinkMetadata(
        linkName,
        participantDeclarations.Select((participant, index) => new DataVaultLinkParticipantMetadata(
            DataVaultMetadataReference.Hub(participant.Hub.Name),
            producedParticipantNames[index])),
        link.DependentChildKeyNames);
  }

  private static void ValidateRepeatedParticipantRoles(
      LinkDeclaration link,
      IReadOnlyList<ParticipantHub> participants) {
    var repeatedHubGroups = participants
        .GroupBy(participant => participant.Hub.Name, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .ToArray();

    if (repeatedHubGroups.Length == 0) {
      return;
    }

    if (link.RelationshipName is null) {
      throw LinkValidationException(
          link,
          "repeats a hub participant and therefore requires an explicit relationship name.");
    }

    foreach (var group in repeatedHubGroups) {
      var roles = new HashSet<string>(StringComparer.Ordinal);
      foreach (var participant in group.Where(participant => !string.IsNullOrWhiteSpace(participant.Participant.Role))) {
        if (!roles.Add(participant.Participant.Role!)) {
          throw LinkValidationException(
              link,
              "declares repeated hub '" +
              group.Key +
              "' with duplicate participant role '" +
              participant.Participant.Role +
              "'. Repeated same-hub participant roles must be distinct.");
        }
      }
    }
  }

  private static void ValidateProducedParticipantNames(
      LinkDeclaration link,
      IEnumerable<string> producedParticipantNames) {
    var participantNames = new HashSet<string>(StringComparer.Ordinal);
    foreach (var participantName in producedParticipantNames) {
      if (!participantNames.Add(participantName)) {
        throw LinkValidationException(
            link,
            "declares produced participant name '" +
            participantName +
            "' more than once. Participant names must be unique by StringComparer.Ordinal.");
      }
    }
  }

  private static IReadOnlyList<string> CreateProducedParticipantNames(IReadOnlyList<ParticipantHub> participants) {
    var hubCounts = participants
        .GroupBy(participant => participant.Hub.Name, StringComparer.Ordinal)
        .ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal);
    var hubOrdinals = new Dictionary<string, int>(StringComparer.Ordinal);
    var producedNames = new List<string>(participants.Count);

    foreach (var participant in participants.OrderBy(participant => participant.Ordinal)) {
      if (!string.IsNullOrWhiteSpace(participant.Participant.Role)) {
        producedNames.Add(participant.Participant.Role!);
        continue;
      }

      if (hubCounts[participant.Hub.Name] == 1) {
        producedNames.Add(participant.Hub.Name);
        continue;
      }

      var ordinal = hubOrdinals.TryGetValue(participant.Hub.Name, out var current)
          ? current + 1
          : 1;
      hubOrdinals[participant.Hub.Name] = ordinal;
      producedNames.Add(participant.Hub.Name + ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    return producedNames;
  }

  private static void ValidateLinkDependentChildKeys(
      LinkDeclaration link,
      IReadOnlyList<string> producedParticipantNames) {
    var dependentChildKeys = new HashSet<string>(StringComparer.Ordinal);
    foreach (var name in link.DependentChildKeyNames) {
      if (!dependentChildKeys.Add(name)) {
        throw LinkValidationException(
            link,
            "declares dependent child key '" + name + "' more than once.");
      }
    }

    var participantNames = producedParticipantNames.ToHashSet(StringComparer.Ordinal);
    foreach (var name in link.DependentChildKeyNames) {
      if (participantNames.Contains(name)) {
        throw LinkValidationException(
            link,
            "declares dependent child key '" + name + "' that overlaps a produced participant name.");
      }
    }
  }

  private HubDeclaration ResolveParticipantHub(LinkDeclaration link, Type participantClrType) {
    var matchingHubs = _hubs
        .Take(link.PrecedingHubCount)
        .Where(hub => hub.ClrType == participantClrType)
        .ToArray();

    if (matchingHubs.Length == 0) {
      throw LinkValidationException(
          link,
          "participant CLR type '" +
          FormatClrType(participantClrType) +
          "' has not been configured as a hub before this link declaration in the same code-first model.");
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

  internal sealed class LinkDeclaration(string? relationshipName, int precedingHubCount) {
    public string? RelationshipName { get; } = relationshipName;

    public int PrecedingHubCount { get; } = precedingHubCount;

    public List<ParticipantDeclaration> Participants { get; } = [];

    public List<string> DependentChildKeyNames { get; } = [];

    public List<SatelliteDeclaration> Satellites { get; } = [];
  }

  internal sealed class ParticipantDeclaration(Type clrType, string? role) {
    public Type ClrType { get; } = clrType;

    public string? Role { get; } = role;
  }

  private sealed record ParticipantHub(ParticipantDeclaration Participant, HubDeclaration Hub, int Ordinal);

  internal sealed class SatelliteDeclaration(string name) {
    public string Name { get; } = name;

    public List<string> DrivingKeyNames { get; } = [];

    public List<string> PayloadNames { get; } = [];

    public string? EffectiveFromName { get; set; }

    public string? EffectiveToName { get; set; }

    public string? CurrentFlagName { get; set; }
  }
}
