using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace DCoding.Data.DVault.Analyzers;

/// <summary>
/// Generates typed latest/current/as-of satellite read models from deterministic DVault metadata declarations.
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

    var declarations = new List<SatelliteReadModelDeclaration>();
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
    var validDeclarations = new List<SatelliteReadModelDeclaration>();

    foreach (var declaration in declarations) {
      if (!string.IsNullOrWhiteSpace(expectedFingerprint) &&
          !string.Equals(expectedFingerprint, declaration.SourceFingerprint, StringComparison.Ordinal)) {
        Report(
            context,
            DataVaultTypedReadModelDiagnosticCatalog.MetadataSourceFingerprintDrift,
            declaration.Location,
            "Typed satellite read model '" + declaration.MetadataName + "' from " + declaration.SourceKind +
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
            "Typed satellite read model '" + declaration.MetadataName + "' from " + declaration.SourceKind +
            " metadata source fingerprint '" + declaration.SourceFingerprint +
            "' produced generated type prefix '" + declaration.TypeNamePrefix +
            "', which collides with another satellite read model in the same compilation.");
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

  private static IReadOnlyList<SatelliteReadModelDeclaration> CreateSupportBundleDeclarations(
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

    var declarations = new List<SatelliteReadModelDeclaration>();
    foreach (var entity in entities.EnumerateArray()) {
      context.CancellationToken.ThrowIfCancellationRequested();
      if (entity.ValueKind != JsonValueKind.Object ||
          !TryGetJsonString(entity, "tableKind", out var tableKind) ||
          !string.Equals(tableKind, "Satellite", StringComparison.Ordinal)) {
        continue;
      }

      if (TryCreateSupportBundleSatellite(
          entity,
          additionalText.Path,
          sourceKind,
          sourceFingerprint,
          context,
          out var declaration)) {
        declarations.Add(declaration);
      }
    }

    return declarations;
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
      if (!TryCreateSupportBundleProperty(property, sourcePath, context, out var descriptor)) {
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

  private static bool TryCreateSupportBundleProperty(
      JsonElement property,
      string sourcePath,
      SourceProductionContext context,
      out SupportBundleProperty descriptor) {
    descriptor = null!;
    if (property.ValueKind != JsonValueKind.Object ||
        !TryGetJsonString(property, "name", out var producedName) ||
        !TryGetJsonString(property, "role", out var role) ||
        !TryGetJsonString(property, "metadataName", out var metadataName) ||
        !TryGetJsonInt32(property, "ordinal", out var ordinal) ||
        !TryGetJsonString(property, "logicalPropertyKind", out var logicalPropertyKind) ||
        !TryGetJsonString(property, "valueFormat", out var valueFormat)) {
      Report(
          context,
          DataVaultTypedReadModelDiagnosticCatalog.UnsupportedSatelliteShape,
          Location.None,
          "Data Vault support-bundle source '" + sourcePath +
          "' contains a satellite property descriptor without produced name, role, metadata name, ordinal, logical property kind, or provider value format.");
      return false;
    }

    TryGetOptionalJsonString(property, "technicalRole", out var technicalRole);
    TryGetOptionalJsonString(property, "clrTypeName", out var clrTypeName);
    TryGetOptionalJsonBoolean(property, "isNullable", out var isNullable);
    descriptor = new SupportBundleProperty(
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

  private static bool IsStringClrType(string value) {
    return string.IsNullOrWhiteSpace(value) ||
        string.Equals(value, "System.String", StringComparison.Ordinal) ||
        string.Equals(value, "string", StringComparison.Ordinal);
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

  private static string GenerateSource(string generatedNamespace, SatelliteReadModelDeclaration declaration) {
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
      if (property.IsNullable && string.Equals(property.TypeName, "string", StringComparison.Ordinal)) {
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
      bool IsNullable);

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
      IReadOnlyList<RowProperty> RowProperties) {
    public string RowTypeName { get; } = TypeNamePrefix + "ReadModel";

    public string ExtensionTypeName { get; } = TypeNamePrefix + "ReadExtensions";
  }
}
