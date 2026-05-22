using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace DCoding.Data.DVault.Analyzers;

/// <summary>
/// Reports high-confidence diagnostics for consumer-side EF Core access patterns that bypass DVault write boundaries.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DataVaultEfCoreMisuseAnalyzer : DiagnosticAnalyzer {
  private const string EfCoreNamespaceName = "Microsoft.EntityFrameworkCore";
  private const string DbContextTypeName = "DbContext";
  private const string DbSetTypeName = "DbSet";
  private const string GenericCollectionsNamespaceName = "System.Collections.Generic";
  private const string DictionaryTypeName = "Dictionary";
  private const string DataVaultNamespaceName = "DCoding.Data.DVault";
  private const string MetadataInterceptorMethodName = "UseDataVaultSaveChangesMetadataInterceptor";

  private static readonly ImmutableHashSet<string> DirectWriteMethodNames = ImmutableHashSet.Create(
      StringComparer.Ordinal,
      "Add",
      "AddAsync",
      "AddRange",
      "AddRangeAsync",
      "Attach",
      "AttachRange",
      "Remove",
      "RemoveRange",
      "Update",
      "UpdateRange");

  private static readonly ImmutableArray<string> GeneratedTableNamePrefixes =
  [
      "Hub",
      "Link",
      "Sat",
      "Pit",
      "Bridge",
  ];

  /// <inheritdoc />
  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
  [
      EfCoreMisuseDiagnosticCatalog.GeneratedDbSetExposure,
      EfCoreMisuseDiagnosticCatalog.DirectGeneratedTableWrite,
  ];

  /// <inheritdoc />
  public override void Initialize(AnalysisContext context) {
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    context.EnableConcurrentExecution();
    context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
    context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
  }

  private static void AnalyzeNamedType(SymbolAnalysisContext context) {
    var typeSymbol = (INamedTypeSymbol)context.Symbol;
    if (!IsDbContextTypeOrDerived(typeSymbol)) {
      return;
    }

    foreach (var member in typeSymbol.GetMembers()) {
      switch (member) {
        case IPropertySymbol propertySymbol
            when IsExposedMember(propertySymbol) &&
                IsGeneratedSharedTypeDbSet(propertySymbol.Type) &&
                TryGetSourceVisibleGeneratedTableName(
                    context.Compilation,
                    propertySymbol,
                    context.CancellationToken,
                    out var propertyTableName):
          ReportGeneratedDbSetExposure(context, propertySymbol, propertyTableName);
          break;
        case IFieldSymbol fieldSymbol
            when IsExposedMember(fieldSymbol) &&
                IsGeneratedSharedTypeDbSet(fieldSymbol.Type) &&
                TryGetSourceVisibleGeneratedTableName(
                    context.Compilation,
                    fieldSymbol,
                    context.CancellationToken,
                    out var fieldTableName):
          ReportGeneratedDbSetExposure(context, fieldSymbol, fieldTableName);
          break;
      }
    }
  }

  private static void AnalyzeInvocation(OperationAnalysisContext context) {
    var invocation = (IInvocationOperation)context.Operation;
    var methodName = invocation.TargetMethod.Name;
    if (!DirectWriteMethodNames.Contains(methodName) ||
        invocation.Instance?.Type is not { } receiverType ||
        !IsGeneratedSharedTypeDbSet(receiverType) ||
        IsInsideVisibleMetadataInterceptorOptIn(context.Compilation, invocation, context.CancellationToken) ||
        !TryGetSourceVisibleGeneratedTableName(invocation.Instance, out var tableName)) {
      return;
    }

    context.ReportDiagnostic(Diagnostic.Create(
        EfCoreMisuseDiagnosticCatalog.DirectGeneratedTableWrite,
        invocation.Syntax.GetLocation(),
        methodName,
        tableName));
  }

  private static void ReportGeneratedDbSetExposure(
      SymbolAnalysisContext context,
      ISymbol memberSymbol,
      string tableName) {
    var location = memberSymbol.Locations.FirstOrDefault(location => location.IsInSource);
    if (location is null) {
      return;
    }

    context.ReportDiagnostic(Diagnostic.Create(
        EfCoreMisuseDiagnosticCatalog.GeneratedDbSetExposure,
        location,
        memberSymbol.Name,
        tableName));
  }

  private static bool IsExposedMember(ISymbol memberSymbol) {
    return !memberSymbol.IsStatic &&
        !memberSymbol.IsImplicitlyDeclared &&
        memberSymbol.DeclaredAccessibility is not Accessibility.Private;
  }

  private static bool IsDbContextTypeOrDerived(INamedTypeSymbol typeSymbol) {
    for (var current = typeSymbol; current is not null; current = current.BaseType) {
      if (IsNamedType(current, EfCoreNamespaceName, DbContextTypeName, arity: 0)) {
        return true;
      }
    }

    return false;
  }

  private static bool IsGeneratedSharedTypeDbSet(ITypeSymbol typeSymbol) {
    return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
        IsNamedType(namedTypeSymbol.OriginalDefinition, EfCoreNamespaceName, DbSetTypeName, arity: 1) &&
        namedTypeSymbol.TypeArguments.Length == 1 &&
        IsDictionaryStringObject(namedTypeSymbol.TypeArguments[0]);
  }

  private static bool IsDictionaryStringObject(ITypeSymbol typeSymbol) {
    return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
        IsNamedType(namedTypeSymbol.OriginalDefinition, GenericCollectionsNamespaceName, DictionaryTypeName, arity: 2) &&
        namedTypeSymbol.TypeArguments.Length == 2 &&
        namedTypeSymbol.TypeArguments[0].SpecialType == SpecialType.System_String &&
        namedTypeSymbol.TypeArguments[1].SpecialType == SpecialType.System_Object;
  }

  private static bool IsNamedType(
      INamedTypeSymbol typeSymbol,
      string namespaceName,
      string typeName,
      int arity) {
    return string.Equals(typeSymbol.ContainingNamespace.ToDisplayString(), namespaceName, StringComparison.Ordinal) &&
        string.Equals(typeSymbol.Name, typeName, StringComparison.Ordinal) &&
        typeSymbol.Arity == arity;
  }

  private static bool TryGetSourceVisibleGeneratedTableName(
      Compilation compilation,
      ISymbol memberSymbol,
      CancellationToken cancellationToken,
      out string tableName) {
    foreach (var syntaxReference in memberSymbol.DeclaringSyntaxReferences) {
      var syntax = syntaxReference.GetSyntax(cancellationToken);
      var semanticModel = compilation.GetSemanticModel(syntax.SyntaxTree);
      foreach (var invocationSyntax in syntax.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>()) {
        if (semanticModel.GetOperation(invocationSyntax, cancellationToken) is IInvocationOperation invocation &&
            TryGetGeneratedTableNameFromSetInvocation(invocation, out tableName)) {
          return true;
        }
      }
    }

    tableName = string.Empty;
    return false;
  }

  private static bool TryGetSourceVisibleGeneratedTableName(IOperation? receiverOperation, out string tableName) {
    receiverOperation = Unwrap(receiverOperation);
    if (receiverOperation is IInvocationOperation invocation &&
        TryGetGeneratedTableNameFromSetInvocation(invocation, out tableName)) {
      return true;
    }

    tableName = string.Empty;
    return false;
  }

  private static bool TryGetGeneratedTableNameFromSetInvocation(
      IInvocationOperation invocation,
      out string tableName) {
    if (!IsDbContextDictionarySetInvocation(invocation) ||
        !TryGetConstantStringArgument(invocation, out tableName) ||
        !IsGeneratedDataVaultTableName(tableName)) {
      tableName = string.Empty;
      return false;
    }

    return true;
  }

  private static bool IsDbContextDictionarySetInvocation(IInvocationOperation invocation) {
    return string.Equals(invocation.TargetMethod.Name, "Set", StringComparison.Ordinal) &&
        invocation.TargetMethod.TypeArguments.Length == 1 &&
        IsDictionaryStringObject(invocation.TargetMethod.TypeArguments[0]) &&
        invocation.TargetMethod.Parameters.Length == 1 &&
        invocation.Instance?.Type is INamedTypeSymbol receiverType &&
        IsDbContextTypeOrDerived(receiverType);
  }

  private static bool TryGetConstantStringArgument(IInvocationOperation invocation, out string value) {
    foreach (var argument in invocation.Arguments) {
      if (argument.Parameter?.Ordinal == 0 &&
          argument.Value.ConstantValue is { HasValue: true, Value: string argumentValue }) {
        value = argumentValue;
        return true;
      }
    }

    value = string.Empty;
    return false;
  }

  private static bool IsGeneratedDataVaultTableName(string tableName) {
    foreach (var prefix in GeneratedTableNamePrefixes) {
      if (tableName.Length > prefix.Length &&
          tableName.StartsWith(prefix, StringComparison.Ordinal) &&
          char.IsUpper(tableName[prefix.Length])) {
        return true;
      }
    }

    return false;
  }

  private static bool IsInsideVisibleMetadataInterceptorOptIn(
      Compilation compilation,
      IInvocationOperation invocation,
      CancellationToken cancellationToken) {
    var scope = invocation.Syntax.FirstAncestorOrSelf<BaseMethodDeclarationSyntax>() ??
        (SyntaxNode?)invocation.Syntax.FirstAncestorOrSelf<AccessorDeclarationSyntax>() ??
        invocation.Syntax.FirstAncestorOrSelf<AnonymousFunctionExpressionSyntax>();
    if (scope is null) {
      return false;
    }

    var semanticModel = compilation.GetSemanticModel(invocation.Syntax.SyntaxTree);
    foreach (var invocationSyntax in scope.DescendantNodes().OfType<InvocationExpressionSyntax>()) {
      if (IsMetadataInterceptorInvocation(semanticModel, invocationSyntax, cancellationToken)) {
        return true;
      }
    }

    return false;
  }

  private static bool IsMetadataInterceptorInvocation(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocationSyntax,
      CancellationToken cancellationToken) {
    var symbol = semanticModel.GetSymbolInfo(invocationSyntax, cancellationToken).Symbol as IMethodSymbol;
    var originalDefinition = symbol?.ReducedFrom?.OriginalDefinition ?? symbol?.OriginalDefinition;

    return originalDefinition is not null &&
        string.Equals(originalDefinition.Name, MetadataInterceptorMethodName, StringComparison.Ordinal) &&
        string.Equals(
            originalDefinition.ContainingNamespace.ToDisplayString(),
            DataVaultNamespaceName,
            StringComparison.Ordinal);
  }

  private static IOperation? Unwrap(IOperation? operation) {
    while (operation is IConversionOperation conversionOperation) {
      operation = conversionOperation.Operand;
    }

    return operation;
  }
}
