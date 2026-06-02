using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace DCoding.Data.DVault.Analyzers;

/// <summary>
/// Generates typed read-model helpers from deterministic DVault support-bundle metadata declarations.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class DataVaultTypedReadModelSourceGenerator : IIncrementalGenerator {
  private const string RootNamespaceProperty = "build_property.RootNamespace";
  private const string EnableTypedReadModelsProperty = "build_property.DVaultGenerateTypedReadModels";
  private const string ExpectedFingerprintProperty = "build_property.DVaultTypedReadModelMetadataSourceFingerprint";
  private const string LegacyExpectedFingerprintProperty = "build_property.DVaultReadModelMetadataSourceFingerprint";
  private const string SupportBundleSchemaVersion = "dvault.support-bundle.v1";

  /// <inheritdoc />
  public void Initialize(IncrementalGeneratorInitializationContext context) {
    var inputs = context.AdditionalTextsProvider.Collect()
        .Combine(context.AnalyzerConfigOptionsProvider);

    context.RegisterSourceOutput(inputs, static (context, input) =>
        Execute(input.Left, input.Right, context));
  }

  private static void Execute(
      ImmutableArray<AdditionalText> additionalTexts,
      AnalyzerConfigOptionsProvider optionsProvider,
      SourceProductionContext context) {
    if (!IsTypedReadModelGenerationEnabled(optionsProvider)) {
      return;
    }

    var declarations = new List<IReadModelDeclaration>();
    var authoritativeMetadataSourceCount = 0;
    var authoritativeSourceKeys = new HashSet<string>(StringComparer.Ordinal);

    foreach (var additionalText in additionalTexts) {
      context.CancellationToken.ThrowIfCancellationRequested();
      declarations.AddRange(CreateSupportBundleDeclarations(
          additionalText,
          context,
          out var wasSupportBundle,
          out var sourceKey));
      if (!wasSupportBundle) {
        continue;
      }

      authoritativeMetadataSourceCount++;
      if (!string.IsNullOrWhiteSpace(sourceKey)) {
        authoritativeSourceKeys.Add(sourceKey);
      }
    }

    if (authoritativeMetadataSourceCount == 0) {
      Report(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.MetadataSourceUnavailable,
          Location.None,
          "Typed read-model generation requires exactly one authoritative dvault.support-bundle.v1 additional file projected from the resolved EF/DVault metadata source.");
      return;
    }

    if (authoritativeMetadataSourceCount > 1 || authoritativeSourceKeys.Count > 1) {
      Report(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.MetadataSourceUnavailable,
          Location.None,
          "Typed read-model generation found more than one authoritative Data Vault support-bundle metadata source in the same generated scope.");
      return;
    }

    if (authoritativeSourceKeys.Count == 0) {
      return;
    }

    if (declarations.Count == 0) {
      return;
    }

    var rootNamespace = ResolveGeneratedNamespace(optionsProvider);
    var expectedFingerprint = ResolveExpectedFingerprint(optionsProvider);
    var validDeclarations = new List<IReadModelDeclaration>();

    foreach (var declaration in declarations) {
      if (!string.IsNullOrWhiteSpace(expectedFingerprint) &&
          !string.Equals(expectedFingerprint, declaration.SourceFingerprint, StringComparison.Ordinal)) {
        Report(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.MetadataSourceFingerprintDrift,
            declaration.Location,
            "Typed " + declaration.ShapeKind + " read model '" + declaration.MetadataName + "' from " + declaration.SourceKind +
            " metadata source fingerprint '" + declaration.SourceFingerprint +
            "' does not match configured expected fingerprint '" + expectedFingerprint + "'.");
        continue;
      }

      validDeclarations.Add(declaration);
    }

    foreach (var collisionGroup in validDeclarations
        .GroupBy(declaration => declaration.TypeNamePrefix, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)) {
      foreach (var declaration in collisionGroup) {
        Report(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.NameCollision,
            declaration.Location,
            "Typed " + declaration.ShapeKind + " read model '" + declaration.MetadataName + "' from " + declaration.SourceKind +
            " metadata source fingerprint '" + declaration.SourceFingerprint +
            "' produced generated type prefix '" + declaration.TypeNamePrefix +
            "', which collides with another typed read model in the same compilation.");
      }
    }

    var collidingTypeNames = validDeclarations
        .GroupBy(declaration => declaration.TypeNamePrefix, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .ToHashSet(StringComparer.Ordinal);

    foreach (var declaration in validDeclarations) {
      context.CancellationToken.ThrowIfCancellationRequested();
      if (collidingTypeNames.Contains(declaration.TypeNamePrefix)) {
        continue;
      }

      var source = GenerateSource(rootNamespace, declaration);
      context.AddSource(
          "DVault.GeneratedReadModels." + declaration.TypeNamePrefix + ".g.cs",
          SourceText.From(source, Encoding.UTF8));
    }
  }

  private static IReadOnlyList<IReadModelDeclaration> CreateSupportBundleDeclarations(
      AdditionalText additionalText,
      SourceProductionContext context,
      out bool wasSupportBundle,
      out string sourceKey) {
    wasSupportBundle = false;
    sourceKey = string.Empty;

    var sourceText = additionalText.GetText(context.CancellationToken);
    if (sourceText is null) {
      return [];
    }

    var text = sourceText.ToString();
    if (!text.Contains("\"schemaVersion\"", StringComparison.Ordinal) ||
        !text.Contains(SupportBundleSchemaVersion, StringComparison.Ordinal)) {
      return [];
    }

    wasSupportBundle = true;
    JsonDocument document;
    try {
      document = JsonDocument.Parse(text);
    }
    catch (JsonException exception) {
      Report(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.MetadataSourceUnavailable,
          Location.None,
          "Data Vault support-bundle additional file '" + additionalText.Path + "' is not valid JSON: " + exception.Message);
      return [];
    }

    using var parsedDocument = document;
    var root = parsedDocument.RootElement;
    if (!root.TryGetProperty("schemaVersion", out var schemaVersion) ||
        schemaVersion.ValueKind != JsonValueKind.String ||
        !string.Equals(schemaVersion.GetString(), SupportBundleSchemaVersion, StringComparison.Ordinal)) {
      return [];
    }

    if (!root.TryGetProperty("diagnostics", out var diagnostics) ||
        diagnostics.ValueKind != JsonValueKind.Object ||
        !diagnostics.TryGetProperty("explain", out var explain) ||
        explain.ValueKind != JsonValueKind.Object) {
      Report(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.MetadataSourceUnavailable,
          Location.None,
          "Data Vault support-bundle additional file '" + additionalText.Path + "' does not contain diagnostics.explain metadata.");
      return [];
    }

    if (!TryGetJsonString(explain, "metadataSourceKind", out var sourceKind) ||
        !TryGetOptionalJsonString(explain, "metadataSourceFingerprint", out var sourceFingerprint) ||
        string.IsNullOrWhiteSpace(sourceFingerprint)) {
      Report(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.MetadataSourceUnavailable,
          Location.None,
          "Data Vault support-bundle additional file '" + additionalText.Path + "' does not contain authoritative metadataSourceKind and metadataSourceFingerprint values.");
      return [];
    }

    sourceKey = sourceKind + "|" + sourceFingerprint;
    if (!explain.TryGetProperty("entities", out var entities) ||
        entities.ValueKind != JsonValueKind.Array) {
      return [];
    }

    var bridgeReadShape = TryGetSupportBundleBridgeReadShape(diagnostics, out var parsedBridgeReadShape)
        ? parsedBridgeReadShape
        : (JsonElement?)null;
    var declarations = new List<IReadModelDeclaration>();
    foreach (var entity in entities.EnumerateArray()) {
      context.CancellationToken.ThrowIfCancellationRequested();
      if (entity.ValueKind != JsonValueKind.Object ||
          !TryGetJsonString(entity, "tableKind", out var tableKind)) {
        continue;
      }

      if (string.Equals(tableKind, "Bridge", StringComparison.Ordinal)) {
        if (TryCreateSupportBundleBridge(
            entity,
            bridgeReadShape,
            additionalText.Path,
            sourceKind,
            sourceFingerprint,
            context,
            out var bridgeDeclaration)) {
          declarations.Add(bridgeDeclaration);
        }

        continue;
      }

      if (!string.Equals(tableKind, "Satellite", StringComparison.Ordinal)) {
        if (tableKind is "Pit" or "PointInTime") {
          if (TryCreateSupportBundlePit(
              entity,
              diagnostics,
              additionalText.Path,
              sourceKind,
              sourceFingerprint,
              context,
              out var pitDeclaration)) {
            declarations.Add(pitDeclaration);
          }
        }
        else {
          ReportUnsupportedSupportBundleReadModelShape(
              entity,
              additionalText.Path,
              sourceKind,
              sourceFingerprint,
              tableKind,
              context);
        }

        continue;
      }

      if (TryCreateSupportBundleSatellite(
          entity,
          additionalText.Path,
          sourceKind,
          sourceFingerprint,
          context,
          out var satelliteDeclaration)) {
        declarations.Add(satelliteDeclaration);
      }
    }

    return declarations;
  }

  private static void ReportUnsupportedSupportBundleReadModelShape(
      JsonElement entity,
      string sourcePath,
      string sourceKind,
      string sourceFingerprint,
      string tableKind,
      SourceProductionContext context) {
    if (tableKind is "Hub" or "Link") {
      return;
    }

    var producedTableName = GetSupportBundleEntityString(entity, "tableName", "<unknown>");
    var metadataName = GetSupportBundleEntityString(entity, "metadataName", producedTableName);
    if (tableKind is "Pit" or "PointInTime") {
      ReportUnsupportedSupportBundlePit(
          entity,
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          context);
      return;
    }

    ReportUnsupportedReadModelShape(
        context,
        DataVaultTypedReadModelDiagnosticCatalog.HelperSkipped,
        tableKind,
        sourceKind,
        sourceFingerprint,
        metadataName,
        producedTableName,
        "support-bundle entity kind from '" + sourcePath + "' is not a generated typed read-model helper target.");
  }

  private static void ReportUnsupportedSupportBundlePit(
      JsonElement entity,
      string sourceKind,
      string sourceFingerprint,
      string metadataName,
      string producedTableName,
      SourceProductionContext context) {
    if (!TryGetSupportBundleParentReference(entity, out _) ||
        !HasSupportBundleTechnicalProperty(entity, "HashKey") ||
        !HasSupportBundleTechnicalProperty(entity, "LoadTimestamp") ||
        !HasSupportBundlePropertyRole(entity, "SnapshotReference")) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "authoritative explain metadata is missing the PIT parent reference, parent hash key, load timestamp, or satellite snapshot reference binding.");
      return;
    }

    if (HasSupportBundlePropertyRole(entity, "DrivingKey")) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.DynamicQueryShapeRequired,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "PIT driving-key tuple projection requires dynamic runtime query behavior outside the residual generator helper contract.");
      return;
    }

    ReportUnsupportedReadModelShape(
        context,
        DataVaultTypedReadModelDiagnosticCatalog.HelperSkipped,
        "PIT",
        sourceKind,
        sourceFingerprint,
        metadataName,
        producedTableName,
        "the runtime PIT metadata shape is valid for IDataVaultReadService usage but no typed PIT helper is emitted by this diagnostic-only generator path.");
  }

  private static bool TryGetSupportBundleBridgeReadShape(JsonElement diagnostics, out JsonElement bridgeReadShape) {
    bridgeReadShape = default;
    if (!diagnostics.TryGetProperty("readShape", out var readShape) ||
        readShape.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(readShape, "kind", out var readShapeKind) ||
        !string.Equals(readShapeKind, "Bridge", StringComparison.Ordinal) ||
        !readShape.TryGetProperty("bridge", out bridgeReadShape) ||
        bridgeReadShape.ValueKind != JsonValueKind.Object) {
      return false;
    }

    return true;
  }

  private static bool TryCreateSupportBundleBridge(
      JsonElement entity,
      JsonElement? bridgeReadShape,
      string sourcePath,
      string sourceKind,
      string sourceFingerprint,
      SourceProductionContext context,
      out BridgeReadModelDeclaration declaration) {
    declaration = null!;
    if (!TryGetJsonString(entity, "tableName", out var producedTableName) ||
        !TryGetJsonString(entity, "metadataName", out var metadataName) ||
        !entity.TryGetProperty("properties", out var propertiesElement) ||
        propertiesElement.ValueKind != JsonValueKind.Array) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          GetSupportBundleEntityString(entity, "metadataName", "<unknown>"),
          GetSupportBundleEntityString(entity, "tableName", "<unknown>"),
          "authoritative explain metadata is missing the bridge produced name, metadata name, or property descriptors.");
      return false;
    }

    if (!TryGetMatchingBridgeReadShape(
        bridgeReadShape,
        metadataName,
        producedTableName,
        out var matchedBridgeReadShape)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "authoritative support-bundle evidence does not include bounded readShape.bridge facts for this bridge entity.");
      return false;
    }

    var properties = new List<SupportBundleProperty>();
    foreach (var property in propertiesElement.EnumerateArray()) {
      if (!TryCreateSupportBundleProperty(
          property,
          sourcePath,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "bridge",
          context,
          out var descriptor)) {
        return false;
      }

      properties.Add(descriptor);
    }

    if (!TryGetJsonString(matchedBridgeReadShape, "bridgeKind", out var bridgeKind) ||
        bridgeKind is not ("ManyToMany" or "Hierarchy")) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "readShape.bridge does not expose a supported bridgeKind value.");
      return false;
    }

    if (!TryCreateBridgeReadShapeEndpoints(
        matchedBridgeReadShape,
        bridgeKind,
        out var endpoints,
        out var endpointFailureReason)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          endpointFailureReason);
      return false;
    }

    var residualBridgeProperties = properties
        .Where(property => !IsSupportedBridgeReadModelProperty(property))
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.ProducedName, StringComparer.Ordinal)
        .ToArray();
    if (residualBridgeProperties.Length > 0) {
      var residualProperty = residualBridgeProperties[0];
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.HelperSkipped,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "valid runtime bridge metadata includes residual projected property '" +
          residualProperty.ProducedName +
          "' with role '" +
          residualProperty.Role +
          "', which is outside the generated bridge helper boundary.");
      return false;
    }

    var participantReferences = properties
        .Where(IsBridgeParticipantReference)
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.ProducedName, StringComparer.Ordinal)
        .ToArray();
    if (participantReferences.Length != 2 ||
        participantReferences.Any(property => !IsSupportBundleStringValue(property, "ParticipantReference"))) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "authoritative explain metadata does not expose exactly two string bridge endpoint participant reference bindings.");
      return false;
    }

    var participantColumnNames = participantReferences
        .Select(property => property.ProducedName)
        .ToHashSet(StringComparer.Ordinal);
    if (participantColumnNames.Count != 2 ||
        endpoints.Any(endpoint => !participantColumnNames.Contains(endpoint.ColumnName))) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "readShape.bridge endpoint hash-key columns do not match the bridge participant reference columns.");
      return false;
    }

    var endpointColumnNames = endpoints.Select(endpoint => endpoint.ColumnName).ToArray();
    var expectedOrderingColumns = string.Equals(bridgeKind, "Hierarchy", StringComparison.Ordinal)
        ? endpointColumnNames.Append("TraversalDepth").ToArray()
        : endpointColumnNames;
    if (!HasReadShapeColumnSet(
            matchedBridgeReadShape,
            "projectedColumns",
            "endpointProjection",
            endpointColumnNames) ||
        !HasReadShapeColumnSet(
            matchedBridgeReadShape,
            "deterministicOrdering",
            "resultOrdering",
            expectedOrderingColumns)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "readShape.bridge does not prove deterministic endpoint projection and result ordering columns.");
      return false;
    }

    var traversalDepth = SingleOrDefault(properties, IsBridgeDepth);
    var hasDepthPredicate = HasReadShapeSingleColumnSet(
        matchedBridgeReadShape,
        "depthPredicate",
        "maximumDepthPredicate",
        "TraversalDepth");
    var hasDepthProjection = HasReadShapeColumnSet(
        matchedBridgeReadShape,
        "projectedColumns",
        "depthProjection",
        ["TraversalDepth"]);

    if (string.Equals(bridgeKind, "ManyToMany", StringComparison.Ordinal)) {
      if (traversalDepth is not null || hasDepthPredicate || hasDepthProjection) {
        ReportUnsupportedReadModelShape(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
            "Bridge",
            sourceKind,
            sourceFingerprint,
            metadataName,
            producedTableName,
            "many-to-many bridge evidence includes hierarchy TraversalDepth facts.");
        return false;
      }
    }
    else if (traversalDepth is null ||
        !string.Equals(traversalDepth.ProducedName, "TraversalDepth", StringComparison.Ordinal) ||
        !IsSupportBundleInt32Value(traversalDepth, "BridgeDepth")) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedBridgeShape,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "hierarchy bridge evidence does not expose the required integer TraversalDepth projection column.");
      return false;
    }
    else if (!hasDepthPredicate || !hasDepthProjection) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.DynamicQueryShapeRequired,
          "Bridge",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "hierarchy bridge read-shape evidence does not prove a bounded maximumDepth predicate and TraversalDepth projection.");
      return false;
    }

    var rowProperties = endpoints
        .Select(endpoint => new RowProperty(
            ResolvePropertyName(endpoint.ColumnName),
            endpoint.ColumnName,
            endpoint.ColumnName,
            "string",
            IsNullable: false))
        .ToList();
    if (traversalDepth is not null) {
      rowProperties.Add(new RowProperty(
          "TraversalDepth",
          traversalDepth.ProducedName,
          "TraversalDepth",
          "int",
          IsNullable: false));
    }

    rowProperties = ResolveRowPropertyNames(rowProperties);
    declaration = new BridgeReadModelDeclaration(
        sourceKind,
        sourceFingerprint,
        Location.None,
        metadataName,
        producedTableName,
        NormalizePublicIdentifier(producedTableName),
        bridgeKind,
        endpoints,
        rowProperties);
    return true;
  }

  private static bool TryGetMatchingBridgeReadShape(
      JsonElement? bridgeReadShape,
      string metadataName,
      string producedTableName,
      out JsonElement matchedBridgeReadShape) {
    matchedBridgeReadShape = default;
    if (!bridgeReadShape.HasValue) {
      return false;
    }

    matchedBridgeReadShape = bridgeReadShape.Value;
    if (!matchedBridgeReadShape.TryGetProperty("bridge", out var bridgeEntity) ||
        bridgeEntity.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(bridgeEntity, "metadataName", out var shapeMetadataName) ||
        !TryGetJsonString(bridgeEntity, "tableName", out var shapeTableName) ||
        !TryGetJsonString(bridgeEntity, "tableKind", out var shapeTableKind) ||
        !string.Equals(shapeTableKind, "Bridge", StringComparison.Ordinal)) {
      return false;
    }

    return string.Equals(shapeMetadataName, metadataName, StringComparison.Ordinal) &&
        string.Equals(shapeTableName, producedTableName, StringComparison.Ordinal);
  }

  private static bool TryCreateBridgeReadShapeEndpoints(
      JsonElement bridgeReadShape,
      string bridgeKind,
      out IReadOnlyList<BridgeEndpointDeclaration> endpoints,
      out string failureReason) {
    endpoints = Array.Empty<BridgeEndpointDeclaration>();
    failureReason = string.Empty;
    if (!bridgeReadShape.TryGetProperty("endpoints", out var endpointsElement) ||
        endpointsElement.ValueKind != JsonValueKind.Array) {
      failureReason = "readShape.bridge does not expose endpoint roles, endpoint names, and endpoint hash-key columns.";
      return false;
    }

    var endpointDeclarations = new List<BridgeEndpointDeclaration>();
    foreach (var endpointElement in endpointsElement.EnumerateArray()) {
      if (endpointElement.ValueKind != JsonValueKind.Object ||
          !TryGetJsonString(endpointElement, "endpoint", out var endpoint) ||
          !TryGetJsonString(endpointElement, "endpointName", out var endpointName) ||
          !TryGetJsonString(endpointElement, "columnName", out var columnName) ||
          !IsSupportedBridgeEndpoint(bridgeKind, endpoint) ||
          !TryDeriveBridgeHubName(endpoint, columnName, out var hubName)) {
        failureReason = "readShape.bridge contains an endpoint outside the supported closed endpoint vocabulary or without a generated hash-key column.";
        return false;
      }

      endpointDeclarations.Add(new BridgeEndpointDeclaration(endpoint, endpointName, columnName, hubName));
    }

    string[] requiredEndpoints = string.Equals(bridgeKind, "ManyToMany", StringComparison.Ordinal)
        ? ["From", "To"]
        : ["Ancestor", "Descendant"];
    if (endpointDeclarations.Count != 2 ||
        requiredEndpoints.Any(endpoint =>
            endpointDeclarations.Count(declaration =>
                string.Equals(declaration.Endpoint, endpoint, StringComparison.Ordinal)) != 1)) {
      failureReason = "readShape.bridge does not expose exactly the endpoint roles required by bridgeKind '" + bridgeKind + "'.";
      return false;
    }

    endpoints = endpointDeclarations;
    return true;
  }

  private static bool IsSupportedBridgeEndpoint(string bridgeKind, string endpoint) {
    return bridgeKind switch {
      "ManyToMany" => endpoint is "From" or "To",
      "Hierarchy" => endpoint is "Ancestor" or "Descendant",
      _ => false,
    };
  }

  private static bool TryDeriveBridgeHubName(string endpoint, string columnName, out string hubName) {
    hubName = string.Empty;
    if (!columnName.EndsWith("HashKey", StringComparison.Ordinal)) {
      return false;
    }

    var baseName = columnName[..^"HashKey".Length];
    if (string.Equals(endpoint, "Ancestor", StringComparison.Ordinal)) {
      if (!baseName.StartsWith("Ancestor", StringComparison.Ordinal)) {
        return false;
      }

      baseName = baseName["Ancestor".Length..];
    }
    else if (string.Equals(endpoint, "Descendant", StringComparison.Ordinal)) {
      if (!baseName.StartsWith("Descendant", StringComparison.Ordinal)) {
        return false;
      }

      baseName = baseName["Descendant".Length..];
    }

    if (string.IsNullOrWhiteSpace(baseName)) {
      return false;
    }

    hubName = baseName;
    return true;
  }

  private static bool TryCreateSupportBundlePit(
      JsonElement entity,
      JsonElement diagnostics,
      string sourcePath,
      string sourceKind,
      string sourceFingerprint,
      SourceProductionContext context,
      out PitReadModelDeclaration declaration) {
    declaration = null!;
    if (!TryGetJsonString(entity, "tableName", out var producedTableName) ||
        !TryGetJsonString(entity, "metadataName", out var metadataName) ||
        !TryGetSupportBundleParentReference(entity, out var parent) ||
        !entity.TryGetProperty("properties", out var propertiesElement) ||
        propertiesElement.ValueKind != JsonValueKind.Array) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          GetSupportBundleEntityString(entity, "metadataName", "<unknown>"),
          GetSupportBundleEntityString(entity, "tableName", "<unknown>"),
          "authoritative explain metadata is missing the produced PIT table, metadata name, parent reference, or property descriptors.");
      return false;
    }

    if (!TryCreateSupportBundlePitReadShape(
        diagnostics,
        metadataName,
        producedTableName,
        out var readShape,
        out var readShapeFailure)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          readShapeFailure);
      return false;
    }

    if (!string.Equals(readShape.Parent.Kind, parent.Kind, StringComparison.Ordinal) ||
        !string.Equals(readShape.Parent.Name, parent.Name, StringComparison.Ordinal)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit parent facts do not match the PIT entity parent reference.");
      return false;
    }

    var properties = new List<SupportBundleProperty>();
    foreach (var property in propertiesElement.EnumerateArray()) {
      if (!TryCreateSupportBundleProperty(
          property,
          sourcePath,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "pit",
          context,
          out var descriptor)) {
        return false;
      }

      properties.Add(descriptor);
    }

    var parentHashKey = SingleOrDefault(properties, IsSupportBundleParentHashKey);
    var loadTimestamp = SingleOrDefault(properties, property => IsSupportBundleTechnical(property, "LoadTimestamp"));
    var drivingKeys = properties
        .Where(property => string.Equals(property.Role, "DrivingKey", StringComparison.Ordinal))
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.ProducedName, StringComparer.Ordinal)
        .ToArray();
    var snapshotReferences = properties
        .Where(property => string.Equals(property.Role, "SnapshotReference", StringComparison.Ordinal))
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.ProducedName, StringComparer.Ordinal)
        .ToArray();

    if (parentHashKey is null ||
        loadTimestamp is null ||
        snapshotReferences.Length == 0) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "authoritative explain metadata is missing the PIT parent hash key, load timestamp, or satellite snapshot reference binding.");
      return false;
    }

    if (!IsSupportBundleStringValue(parentHashKey, "HashKey") ||
        !IsSupportBundleTimestampValue(loadTimestamp, "LoadTimestamp")) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "PIT parent hash-key or load timestamp bindings are not provider-neutral string and timestamp values.");
      return false;
    }

    foreach (var drivingKey in drivingKeys) {
      if (IsTechnicalProjectionName(drivingKey.MetadataName) ||
          !IsSupportBundleStringValue(drivingKey, "DrivingKey")) {
        ReportUnsupportedReadModelShape(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.DynamicQueryShapeRequired,
            "PIT",
            sourceKind,
            sourceFingerprint,
            metadataName,
            producedTableName,
            "PIT driving-key column '" + drivingKey.ProducedName +
            "' is not a provider-neutral string driving-key binding or collides with a technical projection name.");
        return false;
      }
    }

    foreach (var snapshotReference in snapshotReferences) {
      if (!IsSupportBundleTimestampValue(snapshotReference, "SatelliteSnapshotReference")) {
        ReportUnsupportedReadModelShape(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
            "PIT",
            sourceKind,
            sourceFingerprint,
            metadataName,
            producedTableName,
            "PIT snapshot-reference column '" + snapshotReference.ProducedName +
            "' is not a provider-neutral timestamp snapshot binding.");
        return false;
      }
    }

    var pitDrivingKeyColumnNames = drivingKeys.Select(key => key.ProducedName).ToArray();
    if (!TryValidateSupportBundlePitReadShape(
        readShape,
        parent,
        parentHashKey.ProducedName,
        loadTimestamp.ProducedName,
        pitDrivingKeyColumnNames,
        snapshotReferences,
        context,
        sourceKind,
        sourceFingerprint,
        metadataName,
        producedTableName,
        out var orderedSnapshots)) {
      return false;
    }

    var rowProperties = new List<RowProperty>
    {
        new("ParentHashKey", parentHashKey.ProducedName, "ParentHashKey", "string", IsNullable: false)
        {
          ProjectionKind = RowProjectionKind.PitParentHashKey,
          SourceName = "ParentHashKey",
        },
    };

    foreach (var drivingKey in drivingKeys) {
      rowProperties.Add(new RowProperty(
          ResolvePropertyName(drivingKey.ProducedName),
          drivingKey.ProducedName,
          drivingKey.MetadataName,
          "string",
          IsNullable: false) {
        ProjectionKind = RowProjectionKind.PitDrivingKey,
        SourceName = drivingKey.MetadataName,
      });
    }

    rowProperties.Add(new RowProperty(
        "LoadTimestamp",
        loadTimestamp.ProducedName,
        "LoadTimestamp",
        "global::System.DateTimeOffset",
        IsNullable: false) {
      ProjectionKind = RowProjectionKind.PitLoadTimestamp,
      SourceName = "LoadTimestamp",
    });

    foreach (var snapshotReference in orderedSnapshots) {
      rowProperties.Add(new RowProperty(
          ResolvePropertyName(snapshotReference.Property.ProducedName),
          snapshotReference.Property.ProducedName,
          "SnapshotLoadTimestamp",
          "global::System.DateTimeOffset",
          IsNullable: true) {
        ProjectionKind = RowProjectionKind.PitSnapshotReference,
        SourceName = snapshotReference.Satellite.MetadataName,
      });
    }

    rowProperties = ResolveRowPropertyNames(rowProperties);
    declaration = new PitReadModelDeclaration(
        sourceKind,
        sourceFingerprint,
        Location.None,
        parent,
        metadataName,
        producedTableName,
        NormalizePublicIdentifier(producedTableName),
        readShape.ReferencedSatellites
            .Select(satellite => new PitSatelliteReference(
                satellite.MetadataName,
                satellite.DrivingKeyColumnNames.Count > 0))
            .ToArray(),
        rowProperties);
    return true;
  }

  private static bool TryCreateSupportBundlePitReadShape(
      JsonElement diagnostics,
      string metadataName,
      string producedTableName,
      out SupportBundlePitReadShape readShape,
      out string failure) {
    readShape = null!;
    failure = string.Empty;

    if (!diagnostics.TryGetProperty("readShape", out var readShapeElement) ||
        readShapeElement.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(readShapeElement, "kind", out var readShapeKind) ||
        !string.Equals(readShapeKind, "PitAsOf", StringComparison.Ordinal) ||
        !readShapeElement.TryGetProperty("pit", out var pitElement) ||
        pitElement.ValueKind != JsonValueKind.Object) {
      failure = "authoritative support-bundle diagnostics do not carry request-bound readShape.pit facts for this PIT helper.";
      return false;
    }

    if (!pitElement.TryGetProperty("pit", out var pitIdentity) ||
        pitIdentity.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(pitIdentity, "metadataName", out var readShapeMetadataName) ||
        !TryGetJsonString(pitIdentity, "tableName", out var readShapeTableName) ||
        !pitElement.TryGetProperty("parentReference", out var parentReferenceElement) ||
        parentReferenceElement.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(parentReferenceElement, "kind", out var parentKind) ||
        !TryGetJsonString(parentReferenceElement, "name", out var parentName) ||
        parentKind is not ("Hub" or "Link") ||
        !pitElement.TryGetProperty("referencedSatellites", out var referencedSatellitesElement) ||
        referencedSatellitesElement.ValueKind != JsonValueKind.Array) {
      failure = "request-bound readShape.pit facts are missing PIT identity, parent reference, or referenced satellite bindings.";
      return false;
    }

    if (!string.Equals(readShapeMetadataName, metadataName, StringComparison.Ordinal) ||
        !string.Equals(readShapeTableName, producedTableName, StringComparison.Ordinal)) {
      failure = "request-bound readShape.pit identity does not match the PIT entity identity.";
      return false;
    }

    var referencedSatellites = new List<SupportBundlePitReadShapeSatellite>();
    foreach (var satelliteElement in referencedSatellitesElement.EnumerateArray()) {
      if (satelliteElement.ValueKind != JsonValueKind.Object ||
          !TryGetJsonString(satelliteElement, "metadataName", out var satelliteName) ||
          !TryGetJsonString(satelliteElement, "snapshotReferenceColumnName", out var snapshotReferenceColumnName) ||
          !TryGetOptionalStringArray(satelliteElement, "drivingKeyColumnNames", out var drivingKeyColumnNames)) {
        failure = "request-bound readShape.pit referenced satellite facts are missing metadata name, snapshot-reference column, or driving-key columns.";
        return false;
      }

      referencedSatellites.Add(new SupportBundlePitReadShapeSatellite(
          satelliteName,
          snapshotReferenceColumnName,
          drivingKeyColumnNames));
    }

    if (!TryGetReadShapeColumnSets(pitElement, "filterColumns", out var filterColumns) ||
        !TryGetReadShapeColumnSets(pitElement, "projectedColumns", out var projectedColumns) ||
        !TryGetReadShapeColumnSets(pitElement, "rowIdentityColumns", out var rowIdentityColumns)) {
      failure = "request-bound readShape.pit facts are missing filter, projected-column, or row-identity column groups.";
      return false;
    }

    readShape = new SupportBundlePitReadShape(
        new ParentReference(parentKind, parentName),
        referencedSatellites,
        filterColumns,
        projectedColumns,
        rowIdentityColumns);
    return true;
  }

  private static bool TryValidateSupportBundlePitReadShape(
      SupportBundlePitReadShape readShape,
      ParentReference parent,
      string parentHashKeyColumnName,
      string loadTimestampColumnName,
      IReadOnlyList<string> pitDrivingKeyColumnNames,
      IReadOnlyList<SupportBundleProperty> snapshotReferences,
      SourceProductionContext context,
      string sourceKind,
      string sourceFingerprint,
      string metadataName,
      string producedTableName,
      out IReadOnlyList<SupportBundlePitSnapshotReference> orderedSnapshots) {
    orderedSnapshots = Array.Empty<SupportBundlePitSnapshotReference>();

    if (readShape.ReferencedSatellites.Count == 0) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit facts do not include any referenced satellite bindings.");
      return false;
    }

    if (!ColumnSetEquals(readShape.FilterColumns, "parentHashKeyFilter", [parentHashKeyColumnName]) ||
        !ColumnSetEquals(readShape.FilterColumns, "asOfCutoff", [loadTimestampColumnName]) ||
        !ColumnSetEquals(readShape.ProjectedColumns, "pitTechnicalProjection", [parentHashKeyColumnName, loadTimestampColumnName])) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit filter or technical projection facts do not expose the required parent hash-key and as-of cutoff columns.");
      return false;
    }

    var duplicateSatellite = readShape.ReferencedSatellites
        .GroupBy(satellite => satellite.MetadataName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();
    if (duplicateSatellite is not null) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit references satellite '" + duplicateSatellite + "' more than once.");
      return false;
    }

    if (snapshotReferences.Count != readShape.ReferencedSatellites.Count) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit referenced satellite count does not match the PIT entity snapshot-reference count.");
      return false;
    }

    var duplicateSnapshotReference = snapshotReferences
        .GroupBy(snapshot => snapshot.ProducedName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();
    if (duplicateSnapshotReference is not null) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "PIT entity snapshot-reference column '" + duplicateSnapshotReference + "' is not unique.");
      return false;
    }

    var snapshotsByProducedName = snapshotReferences.ToDictionary(
        snapshot => snapshot.ProducedName,
        snapshot => snapshot,
        StringComparer.Ordinal);
    var snapshotReferenceColumnNames = readShape.ReferencedSatellites
        .Select(satellite => satellite.SnapshotReferenceColumnName)
        .ToArray();
    if (!ColumnSetEquals(readShape.ProjectedColumns, "snapshotReferenceProjection", snapshotReferenceColumnNames)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit projected columns do not match the referenced satellite snapshot-reference columns.");
      return false;
    }

    var orderedSnapshotList = new List<SupportBundlePitSnapshotReference>();
    foreach (var satellite in readShape.ReferencedSatellites) {
      if (!snapshotsByProducedName.TryGetValue(satellite.SnapshotReferenceColumnName, out var snapshotReference) ||
          !string.Equals(snapshotReference.MetadataName, satellite.MetadataName, StringComparison.Ordinal)) {
        ReportUnsupportedReadModelShape(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
            "PIT",
            sourceKind,
            sourceFingerprint,
            metadataName,
            producedTableName,
            "request-bound readShape.pit snapshot-reference facts do not match the PIT entity snapshot properties.");
        return false;
      }

      orderedSnapshotList.Add(new SupportBundlePitSnapshotReference(satellite, snapshotReference));
    }

    var multiActiveSatellites = readShape.ReferencedSatellites
        .Where(satellite => satellite.DrivingKeyColumnNames.Count > 0)
        .ToArray();
    var rowIdentityColumns = new[]
    {
        parentHashKeyColumnName,
    }
        .Concat(pitDrivingKeyColumnNames)
        .Append(loadTimestampColumnName)
        .ToArray();
    if (!ColumnSetEquals(readShape.RowIdentityColumns, "pitRowIdentity", rowIdentityColumns)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit row identity columns do not match the PIT helper row identity boundary.");
      return false;
    }

    if (multiActiveSatellites.Length == 0) {
      if (pitDrivingKeyColumnNames.Count != 0) {
        ReportUnsupportedReadModelShape(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
            "PIT",
            sourceKind,
            sourceFingerprint,
            metadataName,
            producedTableName,
            "PIT driving-key columns were present even though readShape.pit did not prove a multi-active satellite driving-key family.");
        return false;
      }

      if (readShape.ProjectedColumns.TryGetValue("pitDrivingKeyProjection", out var projectedDrivingKeyColumns) &&
          projectedDrivingKeyColumns.Count > 0) {
        ReportUnsupportedReadModelShape(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
            "PIT",
            sourceKind,
            sourceFingerprint,
            metadataName,
            producedTableName,
            "request-bound readShape.pit projected driving-key columns were present for an ordinary PIT shape.");
        return false;
      }

      orderedSnapshots = orderedSnapshotList;
      return true;
    }

    if (parent.Kind == "Link") {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "link-parent PIT helpers are limited to unique non-multi-active satellites on one declared link parent.");
      return false;
    }

    if (pitDrivingKeyColumnNames.Count == 0) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedPitShape,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "request-bound readShape.pit facts prove multi-active satellite driving keys but the PIT table driving-key projection is missing.");
      return false;
    }

    var canonicalDrivingKeyColumns = multiActiveSatellites[0].DrivingKeyColumnNames;
    if (!ColumnSetsEqual(canonicalDrivingKeyColumns, pitDrivingKeyColumnNames) ||
        !ColumnSetEquals(readShape.ProjectedColumns, "pitDrivingKeyProjection", pitDrivingKeyColumnNames)) {
      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.DynamicQueryShapeRequired,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "PIT driving-key tuple projection does not match the canonical multi-active satellite driving-key family.");
      return false;
    }

    foreach (var satellite in multiActiveSatellites.Skip(1)) {
      if (ColumnSetsEqual(canonicalDrivingKeyColumns, satellite.DrivingKeyColumnNames)) {
        continue;
      }

      ReportUnsupportedReadModelShape(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.DynamicQueryShapeRequired,
          "PIT",
          sourceKind,
          sourceFingerprint,
          metadataName,
          producedTableName,
          "PIT driving-key tuple projection requires dynamic runtime query behavior because referenced multi-active satellites do not share one canonical driving-key family.");
      return false;
    }

    orderedSnapshots = orderedSnapshotList;
    return true;
  }

  private static bool TryCreateSupportBundleSatellite(
      JsonElement entity,
      string sourcePath,
      string sourceKind,
      string sourceFingerprint,
      SourceProductionContext context,
      out SatelliteReadModelDeclaration declaration) {
    declaration = null!;
    if (!TryGetJsonString(entity, "tableName", out var producedTableName) ||
        !TryGetJsonString(entity, "metadataName", out var metadataName) ||
        !TryGetSupportBundleParentReference(entity, out var parent) ||
        !entity.TryGetProperty("properties", out var propertiesElement) ||
        propertiesElement.ValueKind != JsonValueKind.Array) {
      Report(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedSatelliteShape,
          Location.None,
          "Data Vault support-bundle source '" + sourcePath +
          "' contains a satellite entity whose produced name, metadata name, parent reference, or property descriptor cannot be resolved.");
      return false;
    }

    var properties = new List<SupportBundleProperty>();
    foreach (var property in propertiesElement.EnumerateArray()) {
      if (!TryCreateSupportBundleProperty(
          property,
          sourcePath,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedSatelliteShape,
          "satellite",
          context,
          out var descriptor)) {
        return false;
      }

      properties.Add(descriptor);
    }

    var parentHashKey = SingleOrDefault(properties, IsSupportBundleParentHashKey);
    var hashDiff = SingleOrDefault(properties, property => IsSupportBundleTechnical(property, "HashDiff"));
    var loadTimestamp = SingleOrDefault(properties, property => IsSupportBundleTechnical(property, "LoadTimestamp"));
    var recordSource = SingleOrDefault(properties, property => IsSupportBundleTechnical(property, "RecordSource"));
    var drivingKeys = properties
        .Where(property => string.Equals(property.Role, "DrivingKey", StringComparison.Ordinal))
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.ProducedName, StringComparer.Ordinal)
        .ToArray();
    var payloads = properties
        .Where(property => string.Equals(property.Role, "Payload", StringComparison.Ordinal))
        .OrderBy(property => property.Ordinal)
        .ThenBy(property => property.ProducedName, StringComparer.Ordinal)
        .ToArray();

    if (parentHashKey is null ||
        hashDiff is null ||
        loadTimestamp is null ||
        recordSource is null ||
        payloads.Length == 0) {
      ReportUnsupportedSatellite(
          context,
          Location.None,
          sourceKind,
          sourceFingerprint,
          metadataName,
          "authoritative EF/DVault explain metadata is missing a parent hash-key, hash diff, load timestamp, record source, or payload binding for produced entity '" + producedTableName + "'.");
      return false;
    }

    foreach (var drivingKey in drivingKeys) {
      if (IsTechnicalProjectionName(drivingKey.MetadataName) ||
          !IsSupportBundleStringValue(drivingKey, "DrivingKey")) {
        ReportUnsupportedSatellite(
            context,
            Location.None,
            sourceKind,
            sourceFingerprint,
            metadataName,
            "driving-key produced property '" + drivingKey.ProducedName +
            "' is not a provider-neutral string driving-key binding or collides with a technical projection name.");
        return false;
      }
    }

    foreach (var payload in payloads) {
      if (IsTechnicalProjectionName(payload.MetadataName) ||
          !IsSupportBundleStringValue(payload, "PayloadText")) {
        ReportUnsupportedSatellite(
            context,
            Location.None,
            sourceKind,
            sourceFingerprint,
            metadataName,
            "payload produced property '" + payload.ProducedName +
            "' is not a provider-neutral string payload binding or collides with a technical projection name.");
        return false;
      }
    }

    var rowProperties = new List<RowProperty>
    {
        new(ResolvePropertyName(parentHashKey.ProducedName), parentHashKey.ProducedName, "ParentHashKey", "string", IsNullable: false),
    };

    foreach (var drivingKey in drivingKeys) {
      rowProperties.Add(new RowProperty(
          ResolvePropertyName(drivingKey.ProducedName),
          drivingKey.ProducedName,
          drivingKey.MetadataName,
          "string",
          IsNullable: false));
    }

    rowProperties.Add(new RowProperty("HashDiff", hashDiff.ProducedName, "HashDiff", "string", IsNullable: false));
    rowProperties.Add(new RowProperty("LoadTimestamp", loadTimestamp.ProducedName, "LoadTimestamp", "global::System.DateTimeOffset", IsNullable: false));
    rowProperties.Add(new RowProperty("RecordSource", recordSource.ProducedName, "RecordSource", "string", IsNullable: false));

    foreach (var payload in payloads) {
      if (!payload.IsNullable.HasValue) {
        Report(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.PayloadNullabilityFallback,
            Location.None,
            "Typed satellite read model '" + metadataName + "' from " + sourceKind +
            " metadata source fingerprint '" + sourceFingerprint +
            "' could not prove nullability for payload '" + payload.MetadataName +
            "', so the generated property is nullable.");
      }

      rowProperties.Add(new RowProperty(
          ResolvePropertyName(payload.ProducedName),
          payload.ProducedName,
          payload.MetadataName,
          "string",
          payload.IsNullable ?? true));
    }

    rowProperties = ResolveRowPropertyNames(rowProperties);
    declaration = new SatelliteReadModelDeclaration(
        sourceKind,
        sourceFingerprint,
        Location.None,
        parent,
        metadataName,
        producedTableName,
        NormalizePublicIdentifier(producedTableName),
        drivingKeys.Select(key => key.MetadataName).ToArray(),
        payloads.Select(payload => payload.MetadataName).ToArray(),
        rowProperties);
    return true;
  }

  private static string GetSupportBundleEntityString(JsonElement entity, string propertyName, string fallback) {
    return TryGetOptionalJsonString(entity, propertyName, out var value) &&
        !string.IsNullOrWhiteSpace(value)
        ? value
        : fallback;
  }

  private static bool TryGetSupportBundleParentReference(JsonElement entity, out ParentReference parent) {
    parent = default;
    if (!entity.TryGetProperty("parentReference", out var parentElement) ||
        parentElement.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(parentElement, "kind", out var kind) ||
        !TryGetJsonString(parentElement, "name", out var name) ||
        kind is not ("Hub" or "Link")) {
      return false;
    }

    parent = new ParentReference(kind, name);
    return true;
  }

  private static bool HasSupportBundleTechnicalProperty(JsonElement entity, string technicalRole) {
    return HasSupportBundleProperty(
        entity,
        property => IsSupportBundlePropertyRole(property, "Technical") &&
            TryGetJsonString(property, "technicalRole", out var value) &&
            string.Equals(value, technicalRole, StringComparison.Ordinal));
  }

  private static bool HasSupportBundlePropertyRole(JsonElement entity, string role) {
    return HasSupportBundleProperty(
        entity,
        property => IsSupportBundlePropertyRole(property, role));
  }

  private static bool HasSupportBundleProperty(JsonElement entity, Func<JsonElement, bool> predicate) {
    return CountSupportBundleProperties(entity, predicate) > 0;
  }

  private static int CountSupportBundleProperties(JsonElement entity, Func<JsonElement, bool> predicate) {
    if (!entity.TryGetProperty("properties", out var propertiesElement) ||
        propertiesElement.ValueKind != JsonValueKind.Array) {
      return 0;
    }

    var count = 0;
    foreach (var property in propertiesElement.EnumerateArray()) {
      if (property.ValueKind == JsonValueKind.Object && predicate(property)) {
        count++;
      }
    }

    return count;
  }

  private static bool IsSupportBundlePropertyRole(JsonElement property, string role) {
    return TryGetJsonString(property, "role", out var value) &&
        string.Equals(value, role, StringComparison.Ordinal);
  }

  private static bool TryCreateSupportBundleProperty(
      JsonElement property,
      string sourcePath,
      DiagnosticDescriptor diagnosticDescriptor,
      string shapeKind,
      SourceProductionContext context,
      out SupportBundleProperty propertyDescriptor) {
    propertyDescriptor = null!;
    if (property.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(property, "name", out var producedName) ||
        !TryGetJsonString(property, "role", out var role) ||
        !TryGetJsonString(property, "metadataName", out var metadataName) ||
        !TryGetJsonInt32(property, "ordinal", out var ordinal) ||
        !TryGetJsonString(property, "logicalPropertyKind", out var logicalPropertyKind) ||
        !TryGetJsonString(property, "valueFormat", out var valueFormat)) {
      Report(
          context,
          diagnosticDescriptor,
          Location.None,
          "Data Vault support-bundle source '" + sourcePath +
          "' contains a " + shapeKind + " property descriptor without produced name, role, metadata name, ordinal, logical property kind, or provider value format.");
      return false;
    }

    TryGetOptionalJsonString(property, "technicalRole", out var technicalRole);
    TryGetOptionalJsonString(property, "clrTypeName", out var clrTypeName);
    TryGetOptionalJsonBoolean(property, "isNullable", out var isNullable);
    propertyDescriptor = new SupportBundleProperty(
        producedName,
        role,
        string.IsNullOrWhiteSpace(technicalRole) ? null : technicalRole,
        metadataName,
        ordinal,
        logicalPropertyKind,
        valueFormat,
        clrTypeName,
        isNullable);
    return true;
  }

  private static SupportBundleProperty? SingleOrDefault(
      IEnumerable<SupportBundleProperty> properties,
      Func<SupportBundleProperty, bool> predicate) {
    SupportBundleProperty? match = null;
    foreach (var property in properties) {
      if (!predicate(property)) {
        continue;
      }

      if (match is not null) {
        return null;
      }

      match = property;
    }

    return match;
  }

  private static bool IsSupportBundleParentHashKey(SupportBundleProperty property) {
    return IsSupportBundleTechnical(property, "HashKey");
  }

  private static bool IsSupportBundleTechnical(SupportBundleProperty property, string technicalRole) {
    return string.Equals(property.Role, "Technical", StringComparison.Ordinal) &&
        string.Equals(property.TechnicalRole, technicalRole, StringComparison.Ordinal);
  }

  private static bool IsSupportBundleStringValue(
      SupportBundleProperty property,
      string logicalPropertyKind) {
    return string.Equals(property.LogicalPropertyKind, logicalPropertyKind, StringComparison.Ordinal) &&
        string.Equals(property.ValueFormat, "Text", StringComparison.Ordinal) &&
        IsStringClrType(property.ClrTypeName);
  }

  private static bool IsSupportBundleTimestampValue(
      SupportBundleProperty property,
      string logicalPropertyKind) {
    return string.Equals(property.LogicalPropertyKind, logicalPropertyKind, StringComparison.Ordinal) &&
        string.Equals(property.ValueFormat, "Iso8601UtcText", StringComparison.Ordinal) &&
        IsDateTimeOffsetClrType(property.ClrTypeName);
  }

  private static bool IsSupportBundleInt32Value(
      SupportBundleProperty property,
      string logicalPropertyKind) {
    return string.Equals(property.LogicalPropertyKind, logicalPropertyKind, StringComparison.Ordinal) &&
        string.Equals(property.ValueFormat, "NativeInteger", StringComparison.Ordinal) &&
        IsInt32ClrType(property.ClrTypeName);
  }

  private static bool IsStringClrType(string value) {
    return string.IsNullOrWhiteSpace(value) ||
        string.Equals(value, "System.String", StringComparison.Ordinal) ||
        string.Equals(value, "string", StringComparison.Ordinal);
  }

  private static bool IsDateTimeOffsetClrType(string value) {
    return string.IsNullOrWhiteSpace(value) ||
        string.Equals(value, "System.DateTimeOffset", StringComparison.Ordinal) ||
        string.Equals(value, "DateTimeOffset", StringComparison.Ordinal);
  }

  private static bool IsInt32ClrType(string value) {
    return string.IsNullOrWhiteSpace(value) ||
        string.Equals(value, "System.Int32", StringComparison.Ordinal) ||
        string.Equals(value, "int", StringComparison.Ordinal);
  }

  private static bool IsBridgeParticipantReference(SupportBundleProperty property) {
    return string.Equals(property.Role, "ParticipantReference", StringComparison.Ordinal) &&
        string.Equals(property.TechnicalRole, "HashKey", StringComparison.Ordinal);
  }

  private static bool IsBridgeDepth(SupportBundleProperty property) {
    return string.Equals(property.Role, "BridgeDepth", StringComparison.Ordinal);
  }

  private static bool IsSupportedBridgeReadModelProperty(SupportBundleProperty property) {
    return IsBridgeParticipantReference(property) || IsBridgeDepth(property);
  }

  private static bool HasReadShapeSingleColumnSet(
      JsonElement element,
      string propertyName,
      string role,
      string columnName) {
    if (!element.TryGetProperty(propertyName, out var columnSet) ||
        columnSet.ValueKind != JsonValueKind.Object) {
      return false;
    }

    return ReadShapeColumnSetMatches(columnSet, role, [columnName]);
  }

  private static bool HasReadShapeColumnSet(
      JsonElement element,
      string propertyName,
      string role,
      IReadOnlyList<string> columnNames) {
    if (!element.TryGetProperty(propertyName, out var columnSets) ||
        columnSets.ValueKind != JsonValueKind.Array) {
      return false;
    }

    return columnSets
        .EnumerateArray()
        .Any(columnSet => columnSet.ValueKind == JsonValueKind.Object &&
            ReadShapeColumnSetMatches(columnSet, role, columnNames));
  }

  private static bool ReadShapeColumnSetMatches(
      JsonElement columnSet,
      string role,
      IReadOnlyList<string> columnNames) {
    if (!TryGetJsonString(columnSet, "role", out var actualRole) ||
        !string.Equals(actualRole, role, StringComparison.Ordinal) ||
        !columnSet.TryGetProperty("columnNames", out var actualColumns) ||
        actualColumns.ValueKind != JsonValueKind.Array) {
      return false;
    }

    var values = actualColumns
        .EnumerateArray()
        .Select(column => column.ValueKind == JsonValueKind.String ? column.GetString() : null)
        .ToArray();
    return values.Length == columnNames.Count &&
        values
            .Zip(columnNames, (actual, expected) => string.Equals(actual, expected, StringComparison.Ordinal))
            .All(matches => matches);
  }

  private static bool IsTechnicalProjectionName(string value) {
    return value is "ParentHashKey" or "HashDiff" or "LoadTimestamp" or "RecordSource";
  }

  private static List<RowProperty> ResolveRowPropertyNames(IReadOnlyList<RowProperty> rowProperties) {
    var usedNames = new HashSet<string>(StringComparer.Ordinal);
    var resolved = new List<RowProperty>(rowProperties.Count);
    foreach (var property in rowProperties) {
      var baseName = property.PropertyName;
      var propertyName = baseName;
      var suffix = 2;
      while (!usedNames.Add(propertyName)) {
        propertyName = baseName + suffix.ToString(System.Globalization.CultureInfo.InvariantCulture);
        suffix++;
      }

      resolved.Add(property with { PropertyName = propertyName });
    }

    return resolved;
  }

  private static string GenerateSource(string generatedNamespace, IReadModelDeclaration declaration) {
    return declaration switch {
      SatelliteReadModelDeclaration satellite => GenerateSatelliteSource(generatedNamespace, satellite),
      PitReadModelDeclaration pit => GeneratePitSource(generatedNamespace, pit),
      BridgeReadModelDeclaration bridge => GenerateBridgeSource(generatedNamespace, bridge),
      _ => throw new ArgumentOutOfRangeException(nameof(declaration), declaration, "Unsupported read-model declaration."),
    };
  }
  private static string GenerateSatelliteSource(string generatedNamespace, SatelliteReadModelDeclaration declaration) {
    var builder = new StringBuilder();
    builder.AppendLine("// <auto-generated />");
    builder.AppendLine("#nullable enable");
    builder.AppendLine();
    builder.Append("namespace ");
    builder.Append(generatedNamespace);
    builder.AppendLine(";");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("public sealed record ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine("(");
    for (var index = 0; index < declaration.RowProperties.Count; index++) {
      var property = declaration.RowProperties[index];
      builder.Append("    ");
      builder.Append(property.TypeName);
      if (property.IsNullable) {
        builder.Append('?');
      }

      builder.Append(' ');
      builder.Append(property.PropertyName);
      if (index + 1 < declaration.RowProperties.Count) {
        builder.Append(',');
      }

      builder.AppendLine();
    }

    builder.AppendLine(") {");
    AppendConstant(builder, "ProducedTableName", declaration.ProducedTableName);
    AppendConstant(builder, "MetadataSourceKind", declaration.SourceKind);
    AppendConstant(builder, "MetadataSourceFingerprint", declaration.SourceFingerprint);
    foreach (var property in declaration.RowProperties) {
      AppendConstant(builder, property.PropertyName + "ProducedColumnName", property.ProducedColumnName);
      AppendConstant(builder, property.PropertyName + "MappedName", property.MappedName);
    }

    builder.AppendLine("}");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("public static class ");
    builder.Append(declaration.ExtensionTypeName);
    builder.AppendLine(" {");
    builder.Append("  private static readonly global::DCoding.Data.DVault.Modeling.DataVaultSatelliteMetadata SatelliteMetadata = new(");
    builder.Append(ToLiteral(declaration.MetadataName));
    builder.Append(", ");
    AppendParentReference(builder, declaration.Parent);
    builder.AppendLine(",");
    AppendStringArray(builder, declaration.PayloadNames, indentation: "      ");
    if (declaration.DrivingKeyNames.Count > 0) {
      builder.AppendLine(",");
      AppendStringArray(builder, declaration.DrivingKeyNames, indentation: "      ");
    }

    builder.AppendLine(");");
    builder.AppendLine();
    AppendReadMethod(builder, declaration, "Current", includeAsOf: false);
    builder.AppendLine();
    AppendReadMethod(builder, declaration, "Latest", includeAsOf: false);
    builder.AppendLine();
    AppendReadMethod(builder, declaration, "AsOf", includeAsOf: true);
    builder.AppendLine();
    AppendProjectMethod(builder, declaration);
    builder.AppendLine("}");

    return builder.ToString();
  }

  private static string GenerateBridgeSource(string generatedNamespace, BridgeReadModelDeclaration declaration) {
    var builder = new StringBuilder();
    builder.AppendLine("// <auto-generated />");
    builder.AppendLine("#nullable enable");
    builder.AppendLine();
    builder.Append("namespace ");
    builder.Append(generatedNamespace);
    builder.AppendLine(";");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("public sealed record ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine("(");
    for (var index = 0; index < declaration.RowProperties.Count; index++) {
      var property = declaration.RowProperties[index];
      builder.Append("    ");
      builder.Append(property.TypeName);
      builder.Append(' ');
      builder.Append(property.PropertyName);
      if (index + 1 < declaration.RowProperties.Count) {
        builder.Append(',');
      }

      builder.AppendLine();
    }

    builder.AppendLine(") {");
    AppendConstant(builder, "ProducedTableName", declaration.ProducedTableName);
    AppendConstant(builder, "MetadataSourceKind", declaration.SourceKind);
    AppendConstant(builder, "MetadataSourceFingerprint", declaration.SourceFingerprint);
    foreach (var property in declaration.RowProperties) {
      AppendConstant(builder, property.PropertyName + "ProducedColumnName", property.ProducedColumnName);
      AppendConstant(builder, property.PropertyName + "MappedName", property.MappedName);
    }

    builder.AppendLine("}");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("public static class ");
    builder.Append(declaration.ExtensionTypeName);
    builder.AppendLine(" {");
    AppendBridgeMetadata(builder, declaration);
    builder.AppendLine();
    foreach (var endpoint in declaration.Endpoints) {
      AppendBridgeReadMethod(
          builder,
          declaration,
          endpoint,
          includeMaximumDepth: string.Equals(declaration.BridgeKind, "Hierarchy", StringComparison.Ordinal));
      builder.AppendLine();
    }

    AppendBridgeProjectMethod(builder, declaration);
    builder.AppendLine("}");

    return builder.ToString();
  }

  private static void AppendReadMethod(
      StringBuilder builder,
      SatelliteReadModelDeclaration declaration,
      string methodToken,
      bool includeAsOf) {
    builder.Append("  public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IReadOnlyList<");
    builder.Append(declaration.RowTypeName);
    builder.Append(">> Read");
    builder.Append(declaration.TypeNamePrefix);
    builder.Append(methodToken);
    builder.AppendLine("Async(");
    builder.AppendLine("      this global::DCoding.Data.DVault.IDataVaultReadService readService,");
    builder.AppendLine("      global::Microsoft.EntityFrameworkCore.DbContext dbContext,");
    builder.AppendLine("      global::System.Collections.Generic.IEnumerable<string> parentHashKeys,");
    if (includeAsOf) {
      builder.AppendLine("      global::System.DateTimeOffset asOf,");
    }

    builder.AppendLine("      global::System.Threading.CancellationToken cancellationToken = default) {");
    builder.Append("    return ");
    if (includeAsOf) {
      builder.AppendLine("global::DCoding.Data.DVault.DataVaultReadServiceCurrentSatelliteExtensions.ReadAsOfSatelliteAsync(");
      builder.AppendLine("        readService,");
      builder.AppendLine("        dbContext,");
      builder.AppendLine("        SatelliteMetadata,");
      builder.AppendLine("        parentHashKeys,");
      builder.AppendLine("        asOf,");
      builder.AppendLine("        Project,");
      builder.AppendLine("        cancellationToken);");
    }
    else if (string.Equals(methodToken, "Current", StringComparison.Ordinal)) {
      builder.AppendLine("global::DCoding.Data.DVault.DataVaultReadServiceCurrentSatelliteExtensions.ReadCurrentSatelliteAsync(");
      builder.AppendLine("        readService,");
      builder.AppendLine("        dbContext,");
      builder.AppendLine("        SatelliteMetadata,");
      builder.AppendLine("        parentHashKeys,");
      builder.AppendLine("        Project,");
      builder.AppendLine("        cancellationToken);");
    }
    else {
      builder.AppendLine("global::DCoding.Data.DVault.DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync(");
      builder.AppendLine("        readService,");
      builder.AppendLine("        dbContext,");
      builder.AppendLine("        new global::DCoding.Data.DVault.DataVaultLatestSatelliteReadRequest(SatelliteMetadata, parentHashKeys),");
      builder.AppendLine("        Project,");
      builder.AppendLine("        cancellationToken);");
    }

    builder.AppendLine("  }");
  }

  private static void AppendProjectMethod(StringBuilder builder, SatelliteReadModelDeclaration declaration) {
    builder.Append("  private static ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine(" Project(global::DCoding.Data.DVault.DataVaultSatelliteProjectionRow row) {");
    builder.Append("    return new ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine("(");
    for (var index = 0; index < declaration.RowProperties.Count; index++) {
      var property = declaration.RowProperties[index];
      builder.Append("        ");
      if (string.Equals(property.TypeName, "global::System.DateTimeOffset", StringComparison.Ordinal)) {
        builder.Append("row.RequiredDateTimeOffset(");
      }
      else if (property.IsNullable) {
        builder.Append("row.NullableString(");
      }
      else {
        builder.Append("row.RequiredString(");
      }

      builder.Append(ToLiteral(property.MappedName));
      builder.Append(index + 1 == declaration.RowProperties.Count ? "));" : "),");
      builder.AppendLine();
    }

    builder.AppendLine("  }");
  }

  private static string GeneratePitSource(string generatedNamespace, PitReadModelDeclaration declaration) {
    var builder = new StringBuilder();
    builder.AppendLine("// <auto-generated />");
    builder.AppendLine("#nullable enable");
    builder.AppendLine();
    builder.Append("namespace ");
    builder.Append(generatedNamespace);
    builder.AppendLine(";");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("public sealed record ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine("(");
    for (var index = 0; index < declaration.RowProperties.Count; index++) {
      var property = declaration.RowProperties[index];
      builder.Append("    ");
      builder.Append(property.TypeName);
      if (property.IsNullable) {
        builder.Append('?');
      }

      builder.Append(' ');
      builder.Append(property.PropertyName);
      if (index + 1 < declaration.RowProperties.Count) {
        builder.Append(',');
      }

      builder.AppendLine();
    }

    builder.AppendLine(") {");
    AppendConstant(builder, "ProducedTableName", declaration.ProducedTableName);
    AppendConstant(builder, "MetadataSourceKind", declaration.SourceKind);
    AppendConstant(builder, "MetadataSourceFingerprint", declaration.SourceFingerprint);
    foreach (var property in declaration.RowProperties) {
      AppendConstant(builder, property.PropertyName + "ProducedColumnName", property.ProducedColumnName);
      AppendConstant(builder, property.PropertyName + "MappedName", property.MappedName);
    }

    builder.AppendLine("}");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("public static class ");
    builder.Append(declaration.ExtensionTypeName);
    builder.AppendLine(" {");
    builder.Append("  private static readonly global::DCoding.Data.DVault.Modeling.DataVaultPitMetadata PitMetadata = new(");
    AppendParentReference(builder, declaration.Parent);
    builder.AppendLine(",");
    AppendPitSatelliteReferences(builder, declaration.Satellites, indentation: "      ");
    builder.AppendLine(");");
    builder.AppendLine();
    AppendPitReadMethod(builder, declaration);
    builder.AppendLine();
    AppendPitProjectMethod(builder, declaration);
    builder.AppendLine();
    AppendPitProjectionHelpers(builder);
    builder.AppendLine("}");

    return builder.ToString();
  }

  private static void AppendPitReadMethod(StringBuilder builder, PitReadModelDeclaration declaration) {
    builder.Append("  public static async global::System.Threading.Tasks.Task<global::System.Collections.Generic.IReadOnlyList<");
    builder.Append(declaration.RowTypeName);
    builder.Append(">> Read");
    builder.Append(declaration.TypeNamePrefix);
    builder.AppendLine("AsOfAsync(");
    builder.AppendLine("      this global::DCoding.Data.DVault.IDataVaultReadService readService,");
    builder.AppendLine("      global::Microsoft.EntityFrameworkCore.DbContext dbContext,");
    builder.AppendLine("      global::System.Collections.Generic.IEnumerable<string> parentHashKeys,");
    builder.AppendLine("      global::System.DateTimeOffset asOf,");
    builder.AppendLine("      global::System.Threading.CancellationToken cancellationToken = default) {");
    builder.AppendLine("    var rows = await readService.ReadPitRowsAsync(");
    builder.AppendLine("        dbContext,");
    builder.AppendLine("        new global::DCoding.Data.DVault.DataVaultPitAsOfReadRequest(PitMetadata, parentHashKeys, asOf),");
    builder.AppendLine("        cancellationToken).ConfigureAwait(false);");
    builder.Append("    var projections = new ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine("[rows.Count];");
    builder.AppendLine("    for (var index = 0; index < rows.Count; index++) {");
    builder.AppendLine("      projections[index] = Project(rows[index]);");
    builder.AppendLine("    }");
    builder.AppendLine();
    builder.AppendLine("    return projections;");
    builder.AppendLine("  }");
  }

  private static void AppendPitProjectMethod(StringBuilder builder, PitReadModelDeclaration declaration) {
    builder.Append("  private static ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine(" Project(global::DCoding.Data.DVault.DataVaultPitReadRecord row) {");

    builder.Append("    return new ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine("(");
    for (var index = 0; index < declaration.RowProperties.Count; index++) {
      var property = declaration.RowProperties[index];
      builder.Append("        ");
      switch (property.ProjectionKind) {
        case RowProjectionKind.PitParentHashKey:
          builder.Append("row.ParentHashKey");
          break;
        case RowProjectionKind.PitLoadTimestamp:
          builder.Append("row.LoadTimestamp");
          break;
        case RowProjectionKind.PitDrivingKey:
          builder.Append("RequiredDrivingKeyValue(row, ");
          builder.Append(ToLiteral(property.SourceName));
          builder.Append(')');
          break;
        case RowProjectionKind.PitSnapshotReference:
          builder.Append("GetSnapshotLoadTimestamp(row, ");
          builder.Append(ToLiteral(property.SourceName));
          builder.Append(')');
          break;
        default:
          throw new InvalidOperationException("Unsupported PIT projection kind.");
      }

      builder.Append(index + 1 == declaration.RowProperties.Count ? ");" : ",");
      builder.AppendLine();
    }

    builder.AppendLine("  }");
  }

  private static void AppendPitProjectionHelpers(StringBuilder builder) {
    builder.AppendLine("  private static string RequiredDrivingKeyValue(");
    builder.AppendLine("      global::DCoding.Data.DVault.DataVaultPitReadRecord row,");
    builder.AppendLine("      string name) {");
    builder.AppendLine("    if (row.DrivingKeyValues.TryGetValue(name, out var value)) {");
    builder.AppendLine("      return value;");
    builder.AppendLine("    }");
    builder.AppendLine();
    builder.AppendLine("    throw new global::System.InvalidOperationException(");
    builder.AppendLine("        \"DVault typed PIT projection failed: driving-key value '\" + name + \"' is not present in the PIT read record.\");");
    builder.AppendLine("  }");
    builder.AppendLine();
    builder.AppendLine("  private static global::System.DateTimeOffset? GetSnapshotLoadTimestamp(");
    builder.AppendLine("      global::DCoding.Data.DVault.DataVaultPitReadRecord row,");
    builder.AppendLine("      string satelliteName) {");
    builder.AppendLine("    return row.SatelliteSnapshotsByName.TryGetValue(satelliteName, out var snapshot)");
    builder.AppendLine("        ? snapshot.SnapshotLoadTimestamp");
    builder.AppendLine("        : null;");
    builder.AppendLine("  }");
  }

  private static void AppendBridgeMetadata(StringBuilder builder, BridgeReadModelDeclaration declaration) {
    builder.Append("  private static readonly global::DCoding.Data.DVault.Modeling.DataVaultBridgeMetadata BridgeMetadata = ");
    if (string.Equals(declaration.BridgeKind, "ManyToMany", StringComparison.Ordinal)) {
      var fromEndpoint = declaration.Endpoints.Single(endpoint => endpoint.Endpoint == "From");
      var toEndpoint = declaration.Endpoints.Single(endpoint => endpoint.Endpoint == "To");
      builder.AppendLine("global::DCoding.Data.DVault.Modeling.DataVaultBridgeMetadata.ManyToMany(");
      builder.Append("      ");
      builder.Append(ToLiteral(declaration.MetadataName));
      builder.AppendLine(",");
      AppendHubReferenceArgument(builder, fromEndpoint.HubName);
      builder.AppendLine(",");
      AppendLinkReferenceArgument(builder, declaration.MetadataName);
      builder.AppendLine(",");
      AppendHubReferenceArgument(builder, toEndpoint.HubName);
      builder.AppendLine(");");
      return;
    }

    var ancestorEndpoint = declaration.Endpoints.Single(endpoint => endpoint.Endpoint == "Ancestor");
    var descendantEndpoint = declaration.Endpoints.Single(endpoint => endpoint.Endpoint == "Descendant");
    var endpointOrder = declaration.Endpoints.ToArray();
    builder.AppendLine("global::DCoding.Data.DVault.Modeling.DataVaultBridgeMetadata.Hierarchy(");
    builder.Append("      ");
    builder.Append(ToLiteral(declaration.MetadataName));
    builder.AppendLine(",");
    AppendHubReferenceArgument(builder, ancestorEndpoint.HubName);
    builder.AppendLine(",");
    AppendLinkReferenceArgument(builder, declaration.MetadataName);
    builder.AppendLine(",");
    AppendHubReferenceArgument(builder, descendantEndpoint.HubName);
    builder.AppendLine(",");
    builder.Append("      ");
    builder.Append(Array.IndexOf(endpointOrder, ancestorEndpoint).ToString(System.Globalization.CultureInfo.InvariantCulture));
    builder.AppendLine(",");
    builder.Append("      ");
    builder.Append(Array.IndexOf(endpointOrder, descendantEndpoint).ToString(System.Globalization.CultureInfo.InvariantCulture));
    builder.AppendLine(");");
  }

  private static void AppendHubReferenceArgument(StringBuilder builder, string hubName) {
    builder.Append("      global::DCoding.Data.DVault.Modeling.DataVaultMetadataReference.Hub(");
    builder.Append(ToLiteral(hubName));
    builder.Append(')');
  }

  private static void AppendLinkReferenceArgument(StringBuilder builder, string linkName) {
    builder.Append("      global::DCoding.Data.DVault.Modeling.DataVaultMetadataReference.Link(");
    builder.Append(ToLiteral(linkName));
    builder.Append(')');
  }

  private static void AppendBridgeReadMethod(
      StringBuilder builder,
      BridgeReadModelDeclaration declaration,
      BridgeEndpointDeclaration endpoint,
      bool includeMaximumDepth) {
    builder.Append("  public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IReadOnlyList<");
    builder.Append(declaration.RowTypeName);
    builder.Append(">> Read");
    builder.Append(declaration.TypeNamePrefix);
    builder.Append(endpoint.Endpoint);
    builder.AppendLine("Async(");
    builder.AppendLine("      this global::DCoding.Data.DVault.IDataVaultReadService readService,");
    builder.AppendLine("      global::Microsoft.EntityFrameworkCore.DbContext dbContext,");
    builder.AppendLine("      global::System.Collections.Generic.IEnumerable<string> endpointHashKeys,");
    if (includeMaximumDepth) {
      builder.AppendLine("      int maximumDepth,");
    }

    builder.AppendLine("      global::System.Threading.CancellationToken cancellationToken = default) {");
    builder.AppendLine("    return global::DCoding.Data.DVault.DataVaultReadServiceBridgeExtensions.ReadBridgeAsync(");
    builder.AppendLine("        readService,");
    builder.AppendLine("        dbContext,");
    builder.Append("        new global::DCoding.Data.DVault.DataVaultBridgeReadRequest(BridgeMetadata, global::DCoding.Data.DVault.DataVaultBridgeTraversalEndpoint.");
    builder.Append(endpoint.Endpoint);
    builder.Append(", endpointHashKeys");
    if (includeMaximumDepth) {
      builder.Append(", maximumDepth");
    }

    builder.AppendLine("),");
    builder.AppendLine("        Project,");
    builder.AppendLine("        cancellationToken);");
    builder.AppendLine("  }");
  }

  private static void AppendBridgeProjectMethod(StringBuilder builder, BridgeReadModelDeclaration declaration) {
    builder.Append("  private static ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine(" Project(global::DCoding.Data.DVault.DataVaultBridgeProjectionRow row) {");
    builder.Append("    return new ");
    builder.Append(declaration.RowTypeName);
    builder.AppendLine("(");
    for (var index = 0; index < declaration.RowProperties.Count; index++) {
      var property = declaration.RowProperties[index];
      builder.Append("        ");
      if (string.Equals(property.TypeName, "int", StringComparison.Ordinal)) {
        builder.Append("row.RequiredInt32(");
      }
      else {
        builder.Append("row.RequiredString(");
      }

      builder.Append(ToLiteral(property.MappedName));
      builder.Append(index + 1 == declaration.RowProperties.Count ? "));" : "),");
      builder.AppendLine();
    }

    builder.AppendLine("  }");
  }
  private static void AppendGeneratedCodeAttribute(StringBuilder builder) {
    builder.AppendLine("[global::System.CodeDom.Compiler.GeneratedCode(\"DCoding.Data.DVault.Analyzers\", \"1.0.0\")]");
  }

  private static void AppendConstant(StringBuilder builder, string name, string value) {
    builder.Append("  public const string ");
    builder.Append(name);
    builder.Append(" = ");
    builder.Append(ToLiteral(value));
    builder.AppendLine(";");
  }

  private static void AppendParentReference(StringBuilder builder, ParentReference parent) {
    builder.Append("global::DCoding.Data.DVault.Modeling.DataVaultMetadataReference.");
    builder.Append(parent.Kind);
    builder.Append('(');
    builder.Append(ToLiteral(parent.Name));
    builder.Append(')');
  }

  private static void AppendStringArray(StringBuilder builder, IReadOnlyList<string> values, string indentation) {
    builder.Append(indentation);
    builder.Append("new string[] {");
    for (var index = 0; index < values.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(ToLiteral(values[index]));
    }

    builder.Append('}');
  }

  private static void AppendPitSatelliteReferences(
      StringBuilder builder,
      IReadOnlyList<PitSatelliteReference> satellites,
      string indentation) {
    builder.Append(indentation);
    builder.AppendLine("new global::DCoding.Data.DVault.Modeling.DataVaultPitSatelliteReferenceMetadata[] {");
    for (var index = 0; index < satellites.Count; index++) {
      var satellite = satellites[index];
      builder.Append(indentation);
      builder.Append("    new global::DCoding.Data.DVault.Modeling.DataVaultPitSatelliteReferenceMetadata(");
      builder.Append(ToLiteral(satellite.MetadataName));
      if (satellite.IsMultiActive) {
        builder.Append(", isMultiActive: true");
      }

      builder.Append(')');
      if (index + 1 < satellites.Count) {
        builder.Append(',');
      }

      builder.AppendLine();
    }

    builder.Append(indentation);
    builder.Append('}');
  }

  private static bool TryGetJsonString(JsonElement element, string propertyName, out string value) {
    value = string.Empty;
    if (!element.TryGetProperty(propertyName, out var property) ||
        property.ValueKind != JsonValueKind.String ||
        string.IsNullOrWhiteSpace(property.GetString())) {
      return false;
    }

    value = property.GetString()!;
    return true;
  }

  private static bool TryGetOptionalJsonString(JsonElement element, string propertyName, out string value) {
    value = string.Empty;
    if (!element.TryGetProperty(propertyName, out var property) ||
        property.ValueKind == JsonValueKind.Null) {
      return true;
    }

    if (property.ValueKind != JsonValueKind.String) {
      return false;
    }

    value = property.GetString() ?? string.Empty;
    return true;
  }

  private static bool TryGetJsonInt32(JsonElement element, string propertyName, out int value) {
    value = 0;
    if (!element.TryGetProperty(propertyName, out var property) ||
        property.ValueKind != JsonValueKind.Number ||
        !property.TryGetInt32(out value)) {
      return false;
    }

    return true;
  }

  private static bool TryGetOptionalJsonBoolean(JsonElement element, string propertyName, out bool? value) {
    value = null;
    if (!element.TryGetProperty(propertyName, out var property) ||
        property.ValueKind == JsonValueKind.Null) {
      return true;
    }

    if (property.ValueKind == JsonValueKind.True) {
      value = true;
      return true;
    }

    if (property.ValueKind == JsonValueKind.False) {
      value = false;
      return true;
    }

    return false;
  }

  private static bool TryGetOptionalStringArray(
      JsonElement element,
      string propertyName,
      out IReadOnlyList<string> values) {
    values = Array.Empty<string>();
    if (!element.TryGetProperty(propertyName, out var property) ||
        property.ValueKind == JsonValueKind.Null) {
      return true;
    }

    if (property.ValueKind != JsonValueKind.Array) {
      return false;
    }

    var result = new List<string>();
    foreach (var item in property.EnumerateArray()) {
      if (item.ValueKind != JsonValueKind.String ||
          string.IsNullOrWhiteSpace(item.GetString())) {
        return false;
      }

      result.Add(item.GetString()!);
    }

    values = result;
    return true;
  }

  private static bool TryGetReadShapeColumnSets(
      JsonElement element,
      string propertyName,
      out IReadOnlyDictionary<string, IReadOnlyList<string>> columnSets) {
    columnSets = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal);
    if (!element.TryGetProperty(propertyName, out var property) ||
        property.ValueKind != JsonValueKind.Array) {
      return false;
    }

    var result = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal);
    foreach (var item in property.EnumerateArray()) {
      if (item.ValueKind != JsonValueKind.Object ||
          !TryGetJsonString(item, "role", out var role) ||
          !TryGetOptionalStringArray(item, "columnNames", out var columnNames) ||
          !result.TryAdd(role, columnNames)) {
        return false;
      }
    }

    columnSets = result;
    return true;
  }

  private static bool ColumnSetEquals(
      IReadOnlyDictionary<string, IReadOnlyList<string>> columnSets,
      string role,
      IReadOnlyList<string> expected) {
    return columnSets.TryGetValue(role, out var actual) &&
        ColumnSetsEqual(actual, expected);
  }

  private static bool ColumnSetsEqual(
      IReadOnlyList<string> first,
      IReadOnlyList<string> second) {
    return first.Count == second.Count &&
        first.SequenceEqual(second, StringComparer.Ordinal);
  }

  private static string ResolveGeneratedNamespace(AnalyzerConfigOptionsProvider optionsProvider) {
    if (optionsProvider.GlobalOptions.TryGetValue(RootNamespaceProperty, out var rootNamespace) &&
        !string.IsNullOrWhiteSpace(rootNamespace)) {
      return rootNamespace + ".DVault.GeneratedReadModels";
    }

    return "DVault.GeneratedReadModels";
  }

  private static bool IsTypedReadModelGenerationEnabled(AnalyzerConfigOptionsProvider optionsProvider) {
    return optionsProvider.GlobalOptions.TryGetValue(EnableTypedReadModelsProperty, out var value) &&
        bool.TryParse(value, out var isEnabled) &&
        isEnabled;
  }

  private static string ResolveExpectedFingerprint(AnalyzerConfigOptionsProvider optionsProvider) {
    if (optionsProvider.GlobalOptions.TryGetValue(ExpectedFingerprintProperty, out var value)) {
      return value;
    }

    return optionsProvider.GlobalOptions.TryGetValue(LegacyExpectedFingerprintProperty, out value)
        ? value
        : string.Empty;
  }

  private static void ReportUnsupportedReadModelShape(
      SourceProductionContext context,
      DiagnosticDescriptor descriptor,
      string shapeKind,
      string sourceKind,
      string sourceFingerprint,
      string metadataName,
      string producedTableName,
      string reason) {
    Report(
        context,
        descriptor,
        Location.None,
        "Typed " + shapeKind + " read model '" + metadataName + "' produced entity '" +
        producedTableName + "' from " + sourceKind + " metadata source fingerprint '" +
        sourceFingerprint + "' is unsupported or skipped: " + reason);
  }

  private static void ReportUnsupportedSatellite(
      SourceProductionContext context,
      Location location,
      string sourceKind,
      string sourceFingerprint,
      string satelliteName,
      string reason) {
    Report(
        context,
        DataVaultTypedReadModelDiagnosticCatalog.UnsupportedSatelliteShape,
        location,
        "Typed satellite read model '" + satelliteName + "' from " + sourceKind +
        " metadata source fingerprint '" + sourceFingerprint + "' is unsupported: " + reason);
  }

  private static void Report(
      SourceProductionContext context,
      DiagnosticDescriptor descriptor,
      Location location,
      string message) {
    context.ReportDiagnostic(Diagnostic.Create(descriptor, location, message));
  }

  private static string NormalizePublicIdentifier(string value) {
    var normalized = NormalizePascalCase(value);
    if (normalized.Length == 0) {
      normalized = "Value";
    }

    if (normalized[0] is >= '0' and <= '9' || SyntaxFacts.GetKeywordKind(normalized) != SyntaxKind.None ||
        SyntaxFacts.GetContextualKeywordKind(normalized) != SyntaxKind.None) {
      normalized = "Dvault" + normalized;
    }

    return normalized;
  }

  private static string ResolvePropertyName(string producedColumnName) {
    return NormalizePublicIdentifier(producedColumnName);
  }

  private static string NormalizePascalCase(string value) {
    var tokens = SplitIdentifierTokens(value);
    if (tokens.Count == 0) {
      return string.Empty;
    }

    var builder = new StringBuilder();
    for (var index = 0; index < tokens.Count; index++) {
      var token = tokens[index];
      if (token.Length == 0) {
        continue;
      }

      token = token.ToLowerInvariant();
      builder.Append(ToPascalToken(token));
    }

    return builder.ToString();
  }

  private static List<string> SplitIdentifierTokens(string value) {
    var tokens = new List<string>();
    if (string.IsNullOrWhiteSpace(value)) {
      return tokens;
    }

    var currentToken = new StringBuilder();
    for (var index = 0; index < value.Length; index++) {
      var current = value[index];
      if (!IsAsciiLetterOrDigit(current)) {
        AddToken(tokens, currentToken);
        continue;
      }

      if (currentToken.Length > 0) {
        var previous = value[index - 1];
        var next = index + 1 < value.Length ? value[index + 1] : '\0';
        if (StartsNewToken(previous, current, next)) {
          AddToken(tokens, currentToken);
        }
      }

      currentToken.Append(current);
    }

    AddToken(tokens, currentToken);
    return tokens;
  }

  private static void AddToken(ICollection<string> tokens, StringBuilder currentToken) {
    if (currentToken.Length == 0) {
      return;
    }

    tokens.Add(currentToken.ToString());
    currentToken.Clear();
  }

  private static bool StartsNewToken(char previous, char current, char next) {
    if (!IsAsciiLetterOrDigit(previous)) {
      return false;
    }

    if (IsAsciiDigit(previous) && IsAsciiLetter(current)) {
      return true;
    }

    if (IsAsciiLower(previous) && IsAsciiUpper(current)) {
      return true;
    }

    return IsAsciiUpper(previous) && IsAsciiUpper(current) && IsAsciiLower(next);
  }

  private static string ToPascalToken(string token) {
    if (token.Length == 1) {
      return token.ToUpperInvariant();
    }

    return char.ToUpperInvariant(token[0]) + token[1..].ToLowerInvariant();
  }

  private static bool IsAsciiLetterOrDigit(char value) {
    return IsAsciiLetter(value) || IsAsciiDigit(value);
  }

  private static bool IsAsciiLetter(char value) {
    return IsAsciiUpper(value) || IsAsciiLower(value);
  }

  private static bool IsAsciiUpper(char value) {
    return value is >= 'A' and <= 'Z';
  }

  private static bool IsAsciiLower(char value) {
    return value is >= 'a' and <= 'z';
  }

  private static bool IsAsciiDigit(char value) {
    return value is >= '0' and <= '9';
  }

  private static string ToLiteral(string value) {
    var builder = new StringBuilder(value.Length + 2);
    builder.Append('"');
    foreach (var character in value) {
      switch (character) {
        case '\\':
          builder.Append("\\\\");
          break;
        case '"':
          builder.Append("\\\"");
          break;
        case '\r':
          builder.Append("\\r");
          break;
        case '\n':
          builder.Append("\\n");
          break;
        case '\t':
          builder.Append("\\t");
          break;
        default:
          builder.Append(character);
          break;
      }
    }

    builder.Append('"');
    return builder.ToString();
  }

  private readonly record struct ParentReference(string Kind, string Name);

  private enum RowProjectionKind {
    SatelliteMappedValue,
    PitParentHashKey,
    PitDrivingKey,
    PitLoadTimestamp,
    PitSnapshotReference,
  }

  private sealed record SupportBundleProperty(
      string ProducedName,
      string Role,
      string? TechnicalRole,
      string MetadataName,
      int Ordinal,
      string LogicalPropertyKind,
      string ValueFormat,
      string ClrTypeName,
      bool? IsNullable);

  private sealed record RowProperty(
      string PropertyName,
      string ProducedColumnName,
      string MappedName,
      string TypeName,
      bool IsNullable) {
    public RowProjectionKind ProjectionKind { get; init; } = RowProjectionKind.SatelliteMappedValue;

    public string SourceName { get; init; } = MappedName;
  }

  private interface IReadModelDeclaration {
    string SourceKind { get; }

    string SourceFingerprint { get; }

    Location Location { get; }

    string ShapeKind { get; }

    string MetadataName { get; }

    string ProducedTableName { get; }

    string TypeNamePrefix { get; }

    IReadOnlyList<RowProperty> RowProperties { get; }

    string RowTypeName { get; }

    string ExtensionTypeName { get; }
  }

  private sealed record BridgeEndpointDeclaration(
      string Endpoint,
      string EndpointName,
      string ColumnName,
      string HubName);

  private sealed record SatelliteReadModelDeclaration(
      string SourceKind,
      string SourceFingerprint,
      Location Location,
      ParentReference Parent,
      string MetadataName,
      string ProducedTableName,
      string TypeNamePrefix,
      IReadOnlyList<string> DrivingKeyNames,
      IReadOnlyList<string> PayloadNames,
      IReadOnlyList<RowProperty> RowProperties) : IReadModelDeclaration {
    public string ShapeKind { get; } = "satellite";

    public string RowTypeName { get; } = TypeNamePrefix + "ReadModel";

    public string ExtensionTypeName { get; } = TypeNamePrefix + "ReadExtensions";
  }

  private sealed record BridgeReadModelDeclaration(
      string SourceKind,
      string SourceFingerprint,
      Location Location,
      string MetadataName,
      string ProducedTableName,
      string TypeNamePrefix,
      string BridgeKind,
      IReadOnlyList<BridgeEndpointDeclaration> Endpoints,
      IReadOnlyList<RowProperty> RowProperties) : IReadModelDeclaration {
    public string ShapeKind { get; } = "bridge";

    public string RowTypeName { get; } = TypeNamePrefix + "ReadModel";

    public string ExtensionTypeName { get; } = TypeNamePrefix + "ReadExtensions";
  }

  private sealed record PitReadModelDeclaration(
      string SourceKind,
      string SourceFingerprint,
      Location Location,
      ParentReference Parent,
      string MetadataName,
      string ProducedTableName,
      string TypeNamePrefix,
      IReadOnlyList<PitSatelliteReference> Satellites,
      IReadOnlyList<RowProperty> RowProperties) : IReadModelDeclaration {
    public string ShapeKind { get; } = "pit";

    public string RowTypeName { get; } = TypeNamePrefix + "ReadModel";

    public string ExtensionTypeName { get; } = TypeNamePrefix + "ReadExtensions";
  }

  private sealed record PitSatelliteReference(string MetadataName, bool IsMultiActive);

  private sealed record SupportBundlePitReadShape(
      ParentReference Parent,
      IReadOnlyList<SupportBundlePitReadShapeSatellite> ReferencedSatellites,
      IReadOnlyDictionary<string, IReadOnlyList<string>> FilterColumns,
      IReadOnlyDictionary<string, IReadOnlyList<string>> ProjectedColumns,
      IReadOnlyDictionary<string, IReadOnlyList<string>> RowIdentityColumns);

  private sealed record SupportBundlePitReadShapeSatellite(
      string MetadataName,
      string SnapshotReferenceColumnName,
      IReadOnlyList<string> DrivingKeyColumnNames);

  private sealed record SupportBundlePitSnapshotReference(
      SupportBundlePitReadShapeSatellite Satellite,
      SupportBundleProperty Property);
}
