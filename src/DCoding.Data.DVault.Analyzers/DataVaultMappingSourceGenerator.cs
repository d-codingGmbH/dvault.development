using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DCoding.Data.DVault.Analyzers;

/// <summary>
/// Generates registry-backed DVault typed row mappers from compile-time mapping declarations.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class DataVaultMappingSourceGenerator : IIncrementalGenerator {
  private const string AttributeNamespace = "DCoding.Data.DVault";
  private const string HubMappingAttributeName = AttributeNamespace + ".DataVaultHubMappingAttribute";
  private const string LinkMappingAttributeName = AttributeNamespace + ".DataVaultLinkMappingAttribute";
  private const string HubSatelliteMappingAttributeName = AttributeNamespace + ".DataVaultHubSatelliteMappingAttribute";
  private const string BusinessKeyBindingAttributeName = AttributeNamespace + ".DataVaultBusinessKeyBindingAttribute";
  private const string LinkParticipantBindingAttributeName = AttributeNamespace + ".DataVaultLinkParticipantBindingAttribute";
  private const string LinkDependentChildKeyBindingAttributeName = AttributeNamespace + ".DataVaultLinkDependentChildKeyBindingAttribute";
  private const string SatelliteParentHashKeyBindingAttributeName = AttributeNamespace + ".DataVaultSatelliteParentHashKeyBindingAttribute";
  private const string SatelliteDrivingKeyBindingAttributeName = AttributeNamespace + ".DataVaultSatelliteDrivingKeyBindingAttribute";
  private const string SatellitePayloadBindingAttributeName = AttributeNamespace + ".DataVaultSatellitePayloadBindingAttribute";
  private const string SatelliteHashDiffBindingAttributeName = AttributeNamespace + ".DataVaultSatelliteHashDiffBindingAttribute";

  private static readonly SymbolDisplayFormat FullyQualifiedTypeFormat = new(
      globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
      typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
      genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
      miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);

  /// <inheritdoc />
  public void Initialize(IncrementalGeneratorInitializationContext context) {
    var candidateTypes = context.SyntaxProvider.CreateSyntaxProvider(
        static (node, _) => node is TypeDeclarationSyntax typeDeclaration && typeDeclaration.AttributeLists.Count > 0,
        static (syntaxContext, cancellationToken) =>
            (INamedTypeSymbol?)syntaxContext.SemanticModel.GetDeclaredSymbol((TypeDeclarationSyntax)syntaxContext.Node, cancellationToken))
        .Where(static symbol => symbol is not null);

    context.RegisterSourceOutput(candidateTypes.Collect(), static (context, sourceTypes) => Execute(sourceTypes, context));
  }

  private static void Execute(ImmutableArray<INamedTypeSymbol?> sourceTypes, SourceProductionContext context) {
    var seenTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
    foreach (var sourceType in sourceTypes) {
      context.CancellationToken.ThrowIfCancellationRequested();
      if (sourceType is null || !seenTypes.Add(sourceType)) {
        continue;
      }

      var declaration = CreateDeclaration(sourceType, context);
      if (declaration is null) {
        continue;
      }

      var generatedSource = GenerateSource(declaration);
      context.AddSource(declaration.HintName, SourceText.From(generatedSource, Encoding.UTF8));
    }
  }

  private static MappingDeclaration? CreateDeclaration(INamedTypeSymbol sourceType, SourceProductionContext context) {
    var attributes = sourceType.GetAttributes();
    var hubMapping = attributes.FirstOrDefault(IsHubMappingAttribute);
    var linkMapping = attributes.FirstOrDefault(IsLinkMappingAttribute);
    var satelliteMapping = attributes.FirstOrDefault(IsHubSatelliteMappingAttribute);
    var mappingCount = CountPresent(hubMapping) + CountPresent(linkMapping) + CountPresent(satelliteMapping);
    var hasAnyMappingAttribute = mappingCount > 0 || attributes.Any(IsGeneratedMappingBindingAttribute);

    if (!hasAnyMappingAttribute) {
      return null;
    }

    if (sourceType.TypeParameters.Length > 0) {
      Report(
          context,
          DataVaultMappingDiagnosticCatalog.AmbiguousMappingDeclaration,
          sourceType,
          hubMapping ?? linkMapping ?? satelliteMapping,
          "Data Vault generated mapping source type '" + sourceType.ToDisplayString() + "' must be non-generic.");
      return null;
    }

    if (mappingCount != 1) {
      Report(
          context,
          DataVaultMappingDiagnosticCatalog.AmbiguousMappingDeclaration,
          sourceType,
          hubMapping ?? linkMapping ?? satelliteMapping,
          "Data Vault generated mapping source type '" + sourceType.ToDisplayString() + "' must declare exactly one of DataVaultHubMappingAttribute, DataVaultLinkMappingAttribute, or DataVaultHubSatelliteMappingAttribute.");
      return null;
    }

    if (hubMapping is not null) {
      return CreateHubDeclaration(sourceType, hubMapping, attributes, context);
    }

    if (linkMapping is not null) {
      return CreateLinkDeclaration(sourceType, linkMapping, attributes, context);
    }

    return CreateSatelliteDeclaration(sourceType, satelliteMapping!, attributes, context);
  }

  private static MappingDeclaration? CreateHubDeclaration(
      INamedTypeSymbol sourceType,
      AttributeData mappingAttribute,
      ImmutableArray<AttributeData> attributes,
      SourceProductionContext context) {
    if (!TryGetStringArgument(mappingAttribute, 0, out var hubName)) {
      return null;
    }

    if (!TryValidateLogicalName(sourceType, mappingAttribute, hubName, "hub target name", context)) {
      return null;
    }

    var rawBindings = GetOrderedBindings(attributes, BusinessKeyBindingAttributeName, "business-key");
    if (!TryResolveOrderedBindings(sourceType, rawBindings, "hub business-key", requireAtLeast: 1, checkDuplicateNames: true, context, out var bindings)) {
      return null;
    }

    return MappingDeclaration.Hub(sourceType, hubName, bindings);
  }

  private static MappingDeclaration? CreateLinkDeclaration(
      INamedTypeSymbol sourceType,
      AttributeData mappingAttribute,
      ImmutableArray<AttributeData> attributes,
      SourceProductionContext context) {
    if (!TryGetStringArgument(mappingAttribute, 0, out var linkName)) {
      return null;
    }

    if (!TryValidateLogicalName(sourceType, mappingAttribute, linkName, "link target name", context)) {
      return null;
    }

    var rawBindings = GetOrderedBindings(attributes, LinkParticipantBindingAttributeName, "link produced participant");
    if (!TryResolveOrderedBindings(sourceType, rawBindings, "link produced participant", requireAtLeast: 2, checkDuplicateNames: false, context, out var bindings)) {
      return null;
    }

    var rawDependentChildKeyBindings = GetOrderedBindings(
        attributes,
        LinkDependentChildKeyBindingAttributeName,
        "link dependent child key");
    if (!TryResolveOrderedBindings(
        sourceType,
        rawDependentChildKeyBindings,
        "link dependent child key",
        requireAtLeast: 0,
        checkDuplicateNames: true,
        context,
        out var dependentChildKeyBindings)) {
      return null;
    }

    var repeatedParticipant = bindings
        .GroupBy(binding => binding.LogicalName, StringComparer.Ordinal)
        .FirstOrDefault(group => group.Count() > 1);
    if (repeatedParticipant is not null) {
      var attribute = repeatedParticipant.Skip(1).First().Attribute;
      Report(
          context,
          DataVaultMappingDiagnosticCatalog.RepeatedLinkParticipant,
          sourceType,
          attribute,
          "Generated link mapping '" + linkName + "' declares produced participant name '" + repeatedParticipant.Key + "' more than once. Repeated same-hub generated links must use distinct explicit participant role names by StringComparer.Ordinal.");
      return null;
    }

    var overlappingDependentChildKey = dependentChildKeyBindings
        .FirstOrDefault(binding => bindings.Any(participant =>
            string.Equals(participant.LogicalName, binding.LogicalName, StringComparison.Ordinal)));
    if (overlappingDependentChildKey is not null) {
      Report(
          context,
          DataVaultMappingDiagnosticCatalog.DuplicateBindingName,
          sourceType,
          overlappingDependentChildKey.Attribute,
          "Generated link mapping '" + linkName + "' declares dependent child key name '" + overlappingDependentChildKey.LogicalName + "' that overlaps a produced participant name.");
      return null;
    }

    return MappingDeclaration.Link(sourceType, linkName, bindings, dependentChildKeyBindings);
  }

  private static MappingDeclaration? CreateSatelliteDeclaration(
      INamedTypeSymbol sourceType,
      AttributeData mappingAttribute,
      ImmutableArray<AttributeData> attributes,
      SourceProductionContext context) {
    if (!TryGetStringArgument(mappingAttribute, 0, out var parentHubName) ||
        !TryGetStringArgument(mappingAttribute, 1, out var satelliteName)) {
      return null;
    }

    if (!TryValidateLogicalName(sourceType, mappingAttribute, parentHubName, "hub-parent satellite parent hub name", context) ||
        !TryValidateLogicalName(sourceType, mappingAttribute, satelliteName, "hub-parent satellite target name", context)) {
      return null;
    }

    if (!TryResolveSingleBinding(
        sourceType,
        attributes,
        SatelliteParentHashKeyBindingAttributeName,
        "satellite parent hash-key",
        context,
        out var parentHashKeyBinding)) {
      return null;
    }

    if (!TryResolveSingleBinding(
        sourceType,
        attributes,
        SatelliteHashDiffBindingAttributeName,
        "satellite hash-diff",
        context,
        out var hashDiffBinding)) {
      return null;
    }

    var rawDrivingKeyBindings = GetOrderedBindings(attributes, SatelliteDrivingKeyBindingAttributeName, "satellite driving-key");
    if (!TryResolveOrderedBindings(sourceType, rawDrivingKeyBindings, "satellite driving-key", requireAtLeast: 0, checkDuplicateNames: true, context, out var drivingKeyBindings)) {
      return null;
    }

    var rawPayloadBindings = GetOrderedBindings(attributes, SatellitePayloadBindingAttributeName, "satellite payload");
    if (!TryResolveOrderedBindings(sourceType, rawPayloadBindings, "satellite payload", requireAtLeast: 1, checkDuplicateNames: true, context, out var payloadBindings)) {
      return null;
    }

    return MappingDeclaration.Satellite(
        sourceType,
        parentHubName,
        satelliteName,
        parentHashKeyBinding,
        drivingKeyBindings,
        payloadBindings,
        hashDiffBinding);
  }

  private static bool TryValidateLogicalName(
      INamedTypeSymbol sourceType,
      AttributeData attribute,
      string logicalName,
      string nameKind,
      SourceProductionContext context) {
    if (!string.IsNullOrWhiteSpace(logicalName)) {
      return true;
    }

    Report(
        context,
        DataVaultMappingDiagnosticCatalog.InvalidBinding,
        sourceType,
        attribute,
        "Generated mapping " + nameKind + " must not be blank.");
    return false;
  }

  private static IReadOnlyList<RawOrderedBinding> GetOrderedBindings(
      ImmutableArray<AttributeData> attributes,
      string attributeName,
      string bindingKind) {
    var bindings = new List<RawOrderedBinding>();
    foreach (var attribute in attributes.Where(attribute => IsAttribute(attribute, attributeName))) {
      if (!TryGetIntArgument(attribute, 0, out var order) ||
          !TryGetStringArgument(attribute, 1, out var logicalName) ||
          !TryGetStringArgument(attribute, 2, out var sourceMemberName)) {
        continue;
      }

      bindings.Add(new RawOrderedBinding(order, logicalName, sourceMemberName, bindingKind, attribute));
    }

    return bindings
        .OrderBy(binding => binding.Order)
        .ThenBy(binding => binding.LogicalName, StringComparer.Ordinal)
        .ThenBy(binding => binding.SourceMemberName, StringComparer.Ordinal)
        .ToArray();
  }

  private static bool TryResolveOrderedBindings(
      INamedTypeSymbol sourceType,
      IReadOnlyList<RawOrderedBinding> rawBindings,
      string bindingKind,
      int requireAtLeast,
      bool checkDuplicateNames,
      SourceProductionContext context,
      out IReadOnlyList<ResolvedOrderedBinding> resolvedBindings) {
    var hasErrors = false;
    var resolved = new List<ResolvedOrderedBinding>();
    resolvedBindings = resolved;

    if (rawBindings.Count < requireAtLeast) {
      Report(
          context,
          DataVaultMappingDiagnosticCatalog.MissingRequiredBinding,
          sourceType,
          rawBindings.FirstOrDefault()?.Attribute,
          "Generated " + bindingKind + " mapping for source type '" + sourceType.ToDisplayString() + "' requires at least " + requireAtLeast.ToString(System.Globalization.CultureInfo.InvariantCulture) + " explicit " + bindingKind + " binding" + (requireAtLeast == 1 ? "." : "s."));
      return false;
    }

    foreach (var rawBinding in rawBindings) {
      if (rawBinding.Order < 0) {
        Report(
            context,
            DataVaultMappingDiagnosticCatalog.InvalidBinding,
            sourceType,
            rawBinding.Attribute,
            "Generated " + bindingKind + " binding order must be zero or greater.");
        hasErrors = true;
      }

      if (string.IsNullOrWhiteSpace(rawBinding.LogicalName)) {
        Report(
            context,
            DataVaultMappingDiagnosticCatalog.InvalidBinding,
            sourceType,
            rawBinding.Attribute,
            "Generated " + bindingKind + " binding logical name must not be blank.");
        hasErrors = true;
      }

      if (!TryResolveStringSourceMember(sourceType, rawBinding.SourceMemberName, out var sourceMember)) {
        ReportInvalidSourceMember(context, sourceType, rawBinding.Attribute, bindingKind, rawBinding.SourceMemberName);
        hasErrors = true;
        continue;
      }

      resolved.Add(new ResolvedOrderedBinding(rawBinding.Order, rawBinding.LogicalName, sourceMember.Name, rawBinding.Attribute));
    }

    foreach (var duplicateOrder in rawBindings.GroupBy(binding => binding.Order).Where(group => group.Count() > 1)) {
      Report(
          context,
          DataVaultMappingDiagnosticCatalog.DuplicateBindingOrder,
          sourceType,
          duplicateOrder.Skip(1).First().Attribute,
          "Generated " + bindingKind + " binding order " + duplicateOrder.Key.ToString(System.Globalization.CultureInfo.InvariantCulture) + " is declared more than once.");
      hasErrors = true;
    }

    if (checkDuplicateNames) {
      foreach (var duplicateName in rawBindings.GroupBy(binding => binding.LogicalName, StringComparer.Ordinal).Where(group => group.Count() > 1)) {
        Report(
            context,
            DataVaultMappingDiagnosticCatalog.DuplicateBindingName,
            sourceType,
            duplicateName.Skip(1).First().Attribute,
            "Generated " + bindingKind + " binding name '" + duplicateName.Key + "' is declared more than once.");
        hasErrors = true;
      }
    }

    resolvedBindings = resolved
        .OrderBy(binding => binding.Order)
        .ToArray();
    return !hasErrors;
  }

  private static bool TryResolveSingleBinding(
      INamedTypeSymbol sourceType,
      ImmutableArray<AttributeData> attributes,
      string attributeName,
      string bindingKind,
      SourceProductionContext context,
      out ResolvedSingleBinding binding) {
    binding = default;
    var matchingAttributes = attributes.Where(attribute => IsAttribute(attribute, attributeName)).ToArray();
    if (matchingAttributes.Length == 0) {
      Report(
          context,
          DataVaultMappingDiagnosticCatalog.MissingRequiredBinding,
          sourceType,
          null,
          "Generated hub-parent satellite mapping for source type '" + sourceType.ToDisplayString() + "' requires an explicit " + bindingKind + " binding.");
      return false;
    }

    var attribute = matchingAttributes[0];
    if (!TryGetStringArgument(attribute, 0, out var sourceMemberName)) {
      return false;
    }

    if (!TryResolveStringSourceMember(sourceType, sourceMemberName, out var sourceMember)) {
      ReportInvalidSourceMember(context, sourceType, attribute, bindingKind, sourceMemberName);
      return false;
    }

    binding = new ResolvedSingleBinding(sourceMember.Name, attribute);
    return true;
  }

  private static void ReportInvalidSourceMember(
      SourceProductionContext context,
      INamedTypeSymbol sourceType,
      AttributeData attribute,
      string bindingKind,
      string sourceMemberName) {
    Report(
        context,
        DataVaultMappingDiagnosticCatalog.InvalidBinding,
        sourceType,
        attribute,
        "Generated " + bindingKind + " binding source member '" + sourceMemberName + "' must name a non-static accessible string property or field on source type '" + sourceType.ToDisplayString() + "'.");
  }

  private static bool TryResolveStringSourceMember(
      INamedTypeSymbol sourceType,
      string sourceMemberName,
      out ISymbol sourceMember) {
    sourceMember = null!;
    if (string.IsNullOrWhiteSpace(sourceMemberName)) {
      return false;
    }

    foreach (var candidate in sourceType.GetMembers(sourceMemberName)) {
      switch (candidate) {
        case IPropertySymbol property
            when !property.IsStatic &&
                property.GetMethod is not null &&
                !property.IsIndexer &&
                property.Parameters.Length == 0 &&
                IsAccessibleToGeneratedCode(property) &&
                property.Type.SpecialType == SpecialType.System_String:
          sourceMember = property;
          return true;
        case IFieldSymbol field
            when !field.IsStatic &&
                IsAccessibleToGeneratedCode(field) &&
                field.Type.SpecialType == SpecialType.System_String:
          sourceMember = field;
          return true;
      }
    }

    return false;
  }

  private static bool IsAccessibleToGeneratedCode(ISymbol symbol) {
    return symbol.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal or Accessibility.ProtectedOrInternal;
  }

  private static string GenerateSource(MappingDeclaration declaration) {
    var builder = new StringBuilder();
    builder.AppendLine("// <auto-generated />");
    builder.AppendLine("#nullable enable");
    builder.AppendLine();

    if (!declaration.SourceType.ContainingNamespace.IsGlobalNamespace) {
      builder.Append("namespace ");
      builder.Append(declaration.SourceType.ContainingNamespace.ToDisplayString());
      builder.AppendLine(";");
      builder.AppendLine();
    }

    switch (declaration.Kind) {
      case MappingKind.Hub:
        AppendHubSource(builder, declaration);
        break;
      case MappingKind.Link:
        AppendLinkSource(builder, declaration);
        break;
      case MappingKind.Satellite:
        AppendSatelliteSource(builder, declaration);
        break;
    }

    return builder.ToString();
  }

  private static void AppendHubSource(StringBuilder builder, MappingDeclaration declaration) {
    AppendGeneratedCodeAttribute(builder);
    builder.Append("internal static class ");
    builder.AppendLine(declaration.HelperTypeName + " {");
    AppendMetadataConstant(builder, "HubName", declaration.PrimaryName);
    AppendNameArray(builder, "BusinessKeyNames", declaration.OrderedBindings.Select(binding => binding.LogicalName));
    builder.Append("  public static global::DCoding.Data.DVault.IDataVaultHubMapper<");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.Append("> CreateMapper() => new ");
    builder.Append(declaration.MapperTypeName);
    builder.AppendLine("();");
    builder.AppendLine("}");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("internal sealed class ");
    builder.Append(declaration.MapperTypeName);
    builder.Append(" : global::DCoding.Data.DVault.IDataVaultHubMapper<");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.AppendLine("> {");
    builder.Append("  public global::DCoding.Data.DVault.DataVaultRegistryHubSaveOperation Map(");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.AppendLine(" source) {");
    builder.AppendLine("    global::System.ArgumentNullException.ThrowIfNull(source);");
    builder.AppendLine();
    builder.AppendLine("    return new global::DCoding.Data.DVault.DataVaultRegistryHubSaveOperation(");
    builder.Append("        ");
    builder.Append(ToLiteral(declaration.PrimaryName));
    builder.AppendLine(",");
    AppendKeyValuePairArray(builder, declaration.OrderedBindings, closeWithSemicolon: true);
    builder.AppendLine("  }");
    builder.AppendLine("}");
  }

  private static void AppendLinkSource(StringBuilder builder, MappingDeclaration declaration) {
    AppendGeneratedCodeAttribute(builder);
    builder.Append("internal static class ");
    builder.AppendLine(declaration.HelperTypeName + " {");
    AppendMetadataConstant(builder, "LinkName", declaration.PrimaryName);
    AppendNameArray(builder, "ProducedParticipantNames", declaration.OrderedBindings.Select(binding => binding.LogicalName));
    AppendNameArray(builder, "DependentChildKeyNames", declaration.DependentChildKeyBindings.Select(binding => binding.LogicalName));
    builder.AppendLine("  public static global::System.Collections.Generic.IReadOnlyList<string> ParticipantHubNames => ProducedParticipantNames;");
    builder.Append("  public static global::DCoding.Data.DVault.IDataVaultLinkMapper<");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.Append("> CreateMapper() => new ");
    builder.Append(declaration.MapperTypeName);
    builder.AppendLine("();");
    builder.AppendLine("}");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("internal sealed class ");
    builder.Append(declaration.MapperTypeName);
    builder.Append(" : global::DCoding.Data.DVault.IDataVaultLinkMapper<");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.AppendLine("> {");
    builder.Append("  public global::DCoding.Data.DVault.DataVaultRegistryLinkSaveOperation Map(");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.AppendLine(" source) {");
    builder.AppendLine("    global::System.ArgumentNullException.ThrowIfNull(source);");
    builder.AppendLine();
    builder.AppendLine("    return new global::DCoding.Data.DVault.DataVaultRegistryLinkSaveOperation(");
    builder.Append("        ");
    builder.Append(ToLiteral(declaration.PrimaryName));
    builder.AppendLine(",");
    AppendKeyValuePairArray(builder, declaration.OrderedBindings, closeWithSemicolon: false);
    builder.AppendLine(",");
    AppendKeyValuePairArray(builder, declaration.DependentChildKeyBindings, closeWithSemicolon: true);
    builder.AppendLine("  }");
    builder.AppendLine("}");
  }

  private static void AppendSatelliteSource(StringBuilder builder, MappingDeclaration declaration) {
    AppendGeneratedCodeAttribute(builder);
    builder.Append("internal static class ");
    builder.AppendLine(declaration.HelperTypeName + " {");
    AppendMetadataConstant(builder, "ParentHubName", declaration.PrimaryName);
    AppendMetadataConstant(builder, "SatelliteName", declaration.SecondaryName);
    AppendNameArray(builder, "DrivingKeyNames", declaration.DrivingKeyBindings.Select(binding => binding.LogicalName));
    AppendNameArray(builder, "PayloadNames", declaration.OrderedBindings.Select(binding => binding.LogicalName));
    builder.Append("  public static global::DCoding.Data.DVault.IDataVaultSatelliteMapper<");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.Append("> CreateMapper() => new ");
    builder.Append(declaration.MapperTypeName);
    builder.AppendLine("();");
    builder.AppendLine("}");
    builder.AppendLine();

    AppendGeneratedCodeAttribute(builder);
    builder.Append("internal sealed class ");
    builder.Append(declaration.MapperTypeName);
    builder.Append(" : global::DCoding.Data.DVault.IDataVaultSatelliteMapper<");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.AppendLine("> {");
    builder.Append("  public global::DCoding.Data.DVault.DataVaultRegistrySatelliteSaveOperation Map(");
    builder.Append(GetSourceTypeDisplayName(declaration.SourceType));
    builder.AppendLine(" source) {");
    builder.AppendLine("    global::System.ArgumentNullException.ThrowIfNull(source);");
    builder.AppendLine();
    builder.AppendLine("    return new global::DCoding.Data.DVault.DataVaultRegistrySatelliteSaveOperation(");
    builder.Append("        global::DCoding.Data.DVault.Modeling.DataVaultMetadataReference.Hub(");
    builder.Append(ToLiteral(declaration.PrimaryName));
    builder.AppendLine("),");
    builder.Append("        ");
    builder.Append(ToLiteral(declaration.SecondaryName));
    builder.AppendLine(",");
    builder.Append("        source.");
    builder.AppendLine(EscapeIdentifier(declaration.ParentHashKeyBinding.MemberName) + ",");

    if (declaration.DrivingKeyBindings.Count > 0) {
      AppendKeyValuePairArray(builder, declaration.DrivingKeyBindings, closeWithSemicolon: false);
      builder.AppendLine(",");
    }

    AppendKeyValuePairArray(builder, declaration.OrderedBindings, closeWithSemicolon: false);
    builder.AppendLine(",");
    builder.Append("        source.");
    builder.Append(EscapeIdentifier(declaration.HashDiffBinding.MemberName));
    builder.AppendLine(");");
    builder.AppendLine("  }");
    builder.AppendLine("}");
  }

  private static void AppendGeneratedCodeAttribute(StringBuilder builder) {
    builder.AppendLine("[global::System.CodeDom.Compiler.GeneratedCode(\"DCoding.Data.DVault.Analyzers\", \"1.0.0\")]");
  }

  private static void AppendMetadataConstant(StringBuilder builder, string constantName, string value) {
    builder.Append("  public const string ");
    builder.Append(constantName);
    builder.Append(" = ");
    builder.Append(ToLiteral(value));
    builder.AppendLine(";");
  }

  private static void AppendNameArray(StringBuilder builder, string propertyName, IEnumerable<string> names) {
    builder.Append("  public static global::System.Collections.Generic.IReadOnlyList<string> ");
    builder.Append(propertyName);
    builder.AppendLine(" { get; } = new string[] {");
    foreach (var name in names) {
      builder.Append("      ");
      builder.Append(ToLiteral(name));
      builder.AppendLine(",");
    }

    builder.AppendLine("  };");
  }

  private static void AppendKeyValuePairArray(
      StringBuilder builder,
      IReadOnlyList<ResolvedOrderedBinding> bindings,
      bool closeWithSemicolon) {
    builder.AppendLine("        new global::System.Collections.Generic.KeyValuePair<string, string>[] {");
    foreach (var binding in bindings) {
      builder.Append("            new(");
      builder.Append(ToLiteral(binding.LogicalName));
      builder.Append(", source.");
      builder.Append(EscapeIdentifier(binding.MemberName));
      builder.AppendLine("),");
    }

    builder.Append("        }");
    builder.AppendLine(closeWithSemicolon ? ");" : string.Empty);
  }

  private static string GetSourceTypeDisplayName(INamedTypeSymbol sourceType) {
    return sourceType.ToDisplayString(FullyQualifiedTypeFormat);
  }

  private static string CreateHelperTypeName(INamedTypeSymbol sourceType, string suffix) {
    var containingTypes = new Stack<string>();
    var current = sourceType;
    while (current is not null) {
      containingTypes.Push(SanitizeIdentifier(current.Name));
      current = current.ContainingType;
    }

    return string.Concat(containingTypes) + suffix;
  }

  private static string SanitizeIdentifier(string value) {
    var builder = new StringBuilder(value.Length);
    foreach (var character in value) {
      builder.Append(SyntaxFacts.IsIdentifierPartCharacter(character) ? character : '_');
    }

    if (builder.Length == 0 || !SyntaxFacts.IsIdentifierStartCharacter(builder[0])) {
      builder.Insert(0, '_');
    }

    return EscapeIdentifier(builder.ToString());
  }

  private static string EscapeIdentifier(string value) {
    return SyntaxFacts.GetKeywordKind(value) == SyntaxKind.None &&
        SyntaxFacts.GetContextualKeywordKind(value) == SyntaxKind.None
        ? value
        : "@" + value;
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

  private static bool TryGetStringArgument(AttributeData attribute, int index, out string value) {
    value = string.Empty;
    if (attribute.ConstructorArguments.Length <= index ||
        attribute.ConstructorArguments[index].Value is not string text) {
      return false;
    }

    value = text;
    return true;
  }

  private static bool TryGetIntArgument(AttributeData attribute, int index, out int value) {
    value = 0;
    if (attribute.ConstructorArguments.Length <= index ||
        attribute.ConstructorArguments[index].Value is not int number) {
      return false;
    }

    value = number;
    return true;
  }

  private static void Report(
      SourceProductionContext context,
      DiagnosticDescriptor descriptor,
      INamedTypeSymbol sourceType,
      AttributeData? attribute,
      string message) {
    context.ReportDiagnostic(Diagnostic.Create(descriptor, GetLocation(sourceType, attribute, context.CancellationToken), message));
  }

  private static Location GetLocation(INamedTypeSymbol sourceType, AttributeData? attribute, CancellationToken cancellationToken) {
    return attribute?.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation() ??
        sourceType.Locations.FirstOrDefault() ??
        Location.None;
  }

  private static int CountPresent(AttributeData? attribute) {
    return attribute is null ? 0 : 1;
  }

  private static bool IsHubMappingAttribute(AttributeData attribute) {
    return IsAttribute(attribute, HubMappingAttributeName);
  }

  private static bool IsLinkMappingAttribute(AttributeData attribute) {
    return IsAttribute(attribute, LinkMappingAttributeName);
  }

  private static bool IsHubSatelliteMappingAttribute(AttributeData attribute) {
    return IsAttribute(attribute, HubSatelliteMappingAttributeName);
  }

  private static bool IsGeneratedMappingBindingAttribute(AttributeData attribute) {
    return IsAttribute(attribute, BusinessKeyBindingAttributeName) ||
        IsAttribute(attribute, LinkParticipantBindingAttributeName) ||
        IsAttribute(attribute, LinkDependentChildKeyBindingAttributeName) ||
        IsAttribute(attribute, SatelliteParentHashKeyBindingAttributeName) ||
        IsAttribute(attribute, SatelliteDrivingKeyBindingAttributeName) ||
        IsAttribute(attribute, SatellitePayloadBindingAttributeName) ||
        IsAttribute(attribute, SatelliteHashDiffBindingAttributeName);
  }

  private static bool IsAttribute(AttributeData attribute, string metadataName) {
    var attributeClass = attribute.AttributeClass;
    if (attributeClass is null) {
      return false;
    }

    var namespaceName = attributeClass.ContainingNamespace.IsGlobalNamespace
        ? string.Empty
        : attributeClass.ContainingNamespace.ToDisplayString() + ".";
    return string.Equals(namespaceName + attributeClass.Name, metadataName, StringComparison.Ordinal);
  }

  private sealed record MappingDeclaration(
      MappingKind Kind,
      INamedTypeSymbol SourceType,
      string PrimaryName,
      string SecondaryName,
      ResolvedSingleBinding ParentHashKeyBinding,
      IReadOnlyList<ResolvedOrderedBinding> DrivingKeyBindings,
      IReadOnlyList<ResolvedOrderedBinding> OrderedBindings,
      IReadOnlyList<ResolvedOrderedBinding> DependentChildKeyBindings,
      ResolvedSingleBinding HashDiffBinding) {
    public string HelperTypeName { get; } = CreateHelperTypeName(SourceType, Kind switch {
      MappingKind.Hub => "DataVaultHubMapping",
      MappingKind.Link => "DataVaultLinkMapping",
      _ => "DataVaultHubSatelliteMapping",
    });

    public string MapperTypeName { get; } = CreateHelperTypeName(SourceType, Kind switch {
      MappingKind.Hub => "DataVaultHubMapper",
      MappingKind.Link => "DataVaultLinkMapper",
      _ => "DataVaultHubSatelliteMapper",
    });

    public string HintName { get; } = CreateHelperTypeName(SourceType, Kind switch {
      MappingKind.Hub => "DataVaultHubMapping.g.cs",
      MappingKind.Link => "DataVaultLinkMapping.g.cs",
      _ => "DataVaultHubSatelliteMapping.g.cs",
    });

    public static MappingDeclaration Hub(
        INamedTypeSymbol sourceType,
        string hubName,
        IReadOnlyList<ResolvedOrderedBinding> businessKeyBindings) {
      return new MappingDeclaration(
          MappingKind.Hub,
          sourceType,
          hubName,
          string.Empty,
          default,
          [],
          businessKeyBindings,
          [],
          default);
    }

    public static MappingDeclaration Link(
        INamedTypeSymbol sourceType,
        string linkName,
        IReadOnlyList<ResolvedOrderedBinding> participantBindings,
        IReadOnlyList<ResolvedOrderedBinding> dependentChildKeyBindings) {
      return new MappingDeclaration(
          MappingKind.Link,
          sourceType,
          linkName,
          string.Empty,
          default,
          [],
          participantBindings,
          dependentChildKeyBindings,
          default);
    }

    public static MappingDeclaration Satellite(
        INamedTypeSymbol sourceType,
        string parentHubName,
        string satelliteName,
        ResolvedSingleBinding parentHashKeyBinding,
        IReadOnlyList<ResolvedOrderedBinding> drivingKeyBindings,
        IReadOnlyList<ResolvedOrderedBinding> payloadBindings,
        ResolvedSingleBinding hashDiffBinding) {
      return new MappingDeclaration(
          MappingKind.Satellite,
          sourceType,
          parentHubName,
          satelliteName,
          parentHashKeyBinding,
          drivingKeyBindings,
          payloadBindings,
          [],
          hashDiffBinding);
    }
  }

  private sealed record RawOrderedBinding(
      int Order,
      string LogicalName,
      string SourceMemberName,
      string BindingKind,
      AttributeData Attribute);

  private sealed record ResolvedOrderedBinding(
      int Order,
      string LogicalName,
      string MemberName,
      AttributeData Attribute);

  private readonly record struct ResolvedSingleBinding(string MemberName, AttributeData Attribute);

  private enum MappingKind {
    Hub,
    Link,
    Satellite,
  }
}
