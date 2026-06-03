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
  private const string EfCoreInfrastructureNamespaceName = "Microsoft.EntityFrameworkCore.Infrastructure";
  private const string EfCoreMetadataNamespaceName = "Microsoft.EntityFrameworkCore.Metadata";
  private const string DependencyInjectionNamespaceName = "Microsoft.Extensions.DependencyInjection";
  private const string DbContextTypeName = "DbContext";
  private const string DbContextOptionsBuilderTypeName = "DbContextOptionsBuilder";
  private const string DbSetTypeName = "DbSet";
  private const string ModelCacheKeyFactoryTypeName = "IModelCacheKeyFactory";
  private const string DesignTimeModelTypeName = "IDesignTimeModel";
  private const string ModelRuntimeInitializerTypeName = "IModelRuntimeInitializer";
  private const string ModelTypeName = "IModel";
  private const string GenericCollectionsNamespaceName = "System.Collections.Generic";
  private const string DictionaryTypeName = "Dictionary";
  private const string DataVaultNamespaceName = "DCoding.Data.DVault";
  private const string MetadataInterceptorMethodName = "UseDataVaultSaveChangesMetadataInterceptor";
  private const string ApplyDataVaultMetadataMethodName = "ApplyDataVaultMetadata";
  private const string UseModelMethodName = "UseModel";
  private const string AddDbContextMethodName = "AddDbContext";
  private const string AddDbContextPoolMethodName = "AddDbContextPool";
  private const string ReplaceServiceMethodName = "ReplaceService";
  private const string HasDefaultSchemaMethodName = "HasDefaultSchema";
  private const string SharedTypeEntityMethodName = "SharedTypeEntity";
  private const string ToTableMethodName = "ToTable";
  private const string GetServiceMethodName = "GetService";
  private const string InitializeMethodName = "Initialize";
  private const string ModelPropertyName = "Model";

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

  private static readonly ImmutableHashSet<string> ProviderOptionsMethodNames = ImmutableHashSet.Create(
      StringComparer.Ordinal,
      "UseMySql",
      "UseNpgsql",
      "UseOracle",
      "UseSqlite",
      "UseSqlServer");

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
      EfCoreMisuseDiagnosticCatalog.MissingModelCacheDiscriminator,
      EfCoreMisuseDiagnosticCatalog.UnsafeCompiledModelSelection,
      EfCoreMisuseDiagnosticCatalog.UnsafeDbContextPooling,
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

    AnalyzeModelCacheDiscriminator(context, typeSymbol);
  }

  private static void AnalyzeInvocation(OperationAnalysisContext context) {
    var invocation = (IInvocationOperation)context.Operation;
    var methodName = invocation.TargetMethod.Name;
    if (DirectWriteMethodNames.Contains(methodName) &&
        invocation.Instance?.Type is { } receiverType &&
        IsGeneratedSharedTypeDbSet(receiverType) &&
        !IsInsideVisibleMetadataInterceptorOptIn(context.Compilation, invocation, context.CancellationToken) &&
        TryGetSourceVisibleGeneratedTableName(invocation.Instance, out var tableName)) {
      context.ReportDiagnostic(Diagnostic.Create(
          EfCoreMisuseDiagnosticCatalog.DirectGeneratedTableWrite,
          invocation.Syntax.GetLocation(),
          methodName,
          tableName));
    }

    if (IsUseModelInvocation(invocation) &&
        invocation.Syntax is InvocationExpressionSyntax useModelInvocationSyntax &&
        TryGetContextTypeFromOptionsBuilderOrRegistration(
            context.Compilation.GetSemanticModel(useModelInvocationSyntax.SyntaxTree),
            useModelInvocationSyntax,
            invocation,
            context.CancellationToken,
            out var useModelContextType)) {
      var shape = GetContextLifecycleShape(useModelContextType, context.Compilation, context.CancellationToken);
      if (shape.HasVariableDataVaultShape &&
          !IsVisibleDesignModelRuntimeModelLane(
              context.Compilation,
              invocation,
              useModelContextType,
              context.CancellationToken)) {
        context.ReportDiagnostic(Diagnostic.Create(
            EfCoreMisuseDiagnosticCatalog.UnsafeCompiledModelSelection,
            invocation.Syntax.GetLocation(),
            useModelContextType.Name,
            shape.GetVaryingMemberDisplayList()));
      }
    }

    if (IsAddDbContextPoolInvocation(invocation, out var pooledContextType)) {
      var shape = GetContextLifecycleShape(pooledContextType, context.Compilation, context.CancellationToken);
      var semanticModel = invocation.Syntax is InvocationExpressionSyntax addPoolInvocationSyntax
          ? context.Compilation.GetSemanticModel(addPoolInvocationSyntax.SyntaxTree)
          : null;
      var registrationShape = semanticModel is not null && invocation.Syntax is InvocationExpressionSyntax addPoolRegistrationSyntax
          ? GetRegistrationLifecycleShape(
              semanticModel,
              addPoolRegistrationSyntax,
              invocation,
              context.CancellationToken)
          : ContextLifecycleShape.None;
      var diagnosticShape = shape.HasVariableDataVaultShape ? shape : registrationShape;
      if (diagnosticShape.HasVariableDataVaultShape) {
        context.ReportDiagnostic(Diagnostic.Create(
            EfCoreMisuseDiagnosticCatalog.UnsafeDbContextPooling,
            invocation.Syntax.GetLocation(),
            pooledContextType.Name,
            diagnosticShape.GetVaryingMemberDisplayList()));
      }
    }
  }

  private static void AnalyzeModelCacheDiscriminator(
      SymbolAnalysisContext context,
      INamedTypeSymbol typeSymbol) {
    var shape = GetContextLifecycleShape(typeSymbol, context.Compilation, context.CancellationToken);
    if (!shape.HasVariableDataVaultShape) {
      return;
    }

    var cacheKeyCoverage = GetVisibleCacheKeyCoverage(
        context.Compilation,
        typeSymbol,
        shape.VaryingMembers,
        context.CancellationToken);
    if (cacheKeyCoverage is CacheKeyCoverage.Sufficient or CacheKeyCoverage.Opaque) {
      return;
    }

    context.ReportDiagnostic(Diagnostic.Create(
        EfCoreMisuseDiagnosticCatalog.MissingModelCacheDiscriminator,
        shape.ReportLocation,
        typeSymbol.Name,
        shape.GetVaryingMemberDisplayList()));
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

  private static ContextLifecycleShape GetContextLifecycleShape(
      INamedTypeSymbol typeSymbol,
      Compilation compilation,
      CancellationToken cancellationToken) {
    if (!IsDbContextTypeOrDerived(typeSymbol)) {
      return ContextLifecycleShape.None;
    }

    var varyingMembers = ImmutableArray.CreateBuilder<ISymbol>();
    Location? reportLocation = null;
    var hasDataVaultProjection = false;

    foreach (var methodSymbol in typeSymbol.GetMembers("OnModelCreating").OfType<IMethodSymbol>()) {
      foreach (var syntaxReference in methodSymbol.DeclaringSyntaxReferences) {
        if (syntaxReference.GetSyntax(cancellationToken) is not MethodDeclarationSyntax methodSyntax) {
          continue;
        }

        var semanticModel = compilation.GetSemanticModel(methodSyntax.SyntaxTree);
        var projectionInvocations = GetDataVaultModelProjectionInvocations(
            semanticModel,
            methodSyntax,
            cancellationToken);
        if (projectionInvocations.IsEmpty) {
          continue;
        }

        hasDataVaultProjection = true;

        foreach (var projectionInvocation in projectionInvocations) {
          AddVaryingContextMemberReferences(
              semanticModel,
              projectionInvocation.Syntax,
              typeSymbol,
              varyingMembers,
              ref reportLocation,
              cancellationToken);

          foreach (var controlExpression in GetProjectionControlExpressions(projectionInvocation.Syntax)) {
            AddVaryingContextMemberReferences(
                semanticModel,
                controlExpression,
                typeSymbol,
                varyingMembers,
                ref reportLocation,
                cancellationToken);
          }
        }

        foreach (var invocationSyntax in methodSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>()) {
          if (semanticModel.GetOperation(invocationSyntax, cancellationToken) is not IInvocationOperation invocation ||
              !IsDataVaultShapeAffectingInvocation(
                  semanticModel,
                  invocationSyntax,
                  invocation,
                  cancellationToken)) {
            continue;
          }

          AddVaryingContextMemberReferences(
              semanticModel,
              invocationSyntax,
              typeSymbol,
              varyingMembers,
              ref reportLocation,
              cancellationToken);
        }
      }
    }

    if (!hasDataVaultProjection || varyingMembers.Count == 0) {
      return ContextLifecycleShape.None;
    }

    return new ContextLifecycleShape(varyingMembers.ToImmutable(), reportLocation);
  }

  private static ContextLifecycleShape GetRegistrationLifecycleShape(
      SemanticModel semanticModel,
      InvocationExpressionSyntax registrationInvocationSyntax,
      IInvocationOperation registrationInvocation,
      CancellationToken cancellationToken) {
    if (!IsDbContextRegistrationInvocation(registrationInvocation, out var contextType) ||
        !HasVisibleDataVaultProjection(contextType, semanticModel.Compilation, cancellationToken)) {
      return ContextLifecycleShape.None;
    }

    var varyingSymbols = ImmutableArray.CreateBuilder<ISymbol>();
    Location? reportLocation = null;
    var visibleSourceScope = GetVisibleSourceScope(registrationInvocationSyntax);
    if (visibleSourceScope is null) {
      return ContextLifecycleShape.None;
    }

    foreach (var argumentSyntax in registrationInvocationSyntax.ArgumentList.Arguments) {
      if (argumentSyntax.Expression is not LambdaExpressionSyntax lambdaSyntax) {
        continue;
      }

      foreach (var providerInvocationSyntax in lambdaSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>()) {
        if (semanticModel.GetOperation(providerInvocationSyntax, cancellationToken) is not IInvocationOperation providerInvocation ||
            !IsProviderOptionsInvocation(providerInvocation)) {
          continue;
        }

        foreach (var controlExpression in GetRegistrationControlExpressions(providerInvocationSyntax, lambdaSyntax)) {
          AddVaryingSourceReferences(
              semanticModel,
              controlExpression,
              visibleSourceScope,
              lambdaSyntax,
              varyingSymbols,
              ref reportLocation,
              cancellationToken,
              ImmutableArray<ISymbol>.Empty);
        }
      }
    }

    return varyingSymbols.Count == 0
        ? ContextLifecycleShape.None
        : new ContextLifecycleShape(varyingSymbols.ToImmutable(), reportLocation);
  }

  private static IEnumerable<ExpressionSyntax> GetRegistrationControlExpressions(
      InvocationExpressionSyntax invocationSyntax,
      LambdaExpressionSyntax lambdaSyntax) {
    foreach (var ancestor in invocationSyntax.Ancestors()) {
      if (ReferenceEquals(ancestor, lambdaSyntax)) {
        yield break;
      }

      switch (ancestor) {
        case IfStatementSyntax ifStatement:
          yield return ifStatement.Condition;
          break;
        case ConditionalExpressionSyntax conditionalExpression:
          yield return conditionalExpression.Condition;
          break;
        case SwitchStatementSyntax switchStatement:
          yield return switchStatement.Expression;
          break;
        case SwitchExpressionSyntax switchExpression:
          yield return switchExpression.GoverningExpression;
          break;
      }
    }
  }

  private static void AddVaryingSourceReferences(
      SemanticModel semanticModel,
      SyntaxNode scope,
      SyntaxNode visibleSourceScope,
      LambdaExpressionSyntax lambdaSyntax,
      ImmutableArray<ISymbol>.Builder varyingSymbols,
      ref Location? reportLocation,
      CancellationToken cancellationToken,
      ImmutableArray<ISymbol> visitedSymbols) {
    foreach (var node in scope.DescendantNodesAndSelf()) {
      if (node is not IdentifierNameSyntax and not MemberAccessExpressionSyntax) {
        continue;
      }

      var symbol = semanticModel.GetSymbolInfo(node, cancellationToken).Symbol;
      if (symbol is ILocalSymbol localSymbol) {
        if (ContainsSymbol(visitedSymbols, localSymbol)) {
          continue;
        }

        if (TryGetLocalInitializerSyntax(
            semanticModel,
            localSymbol,
            visibleSourceScope,
            cancellationToken,
            out var initializerSyntax)) {
          if (IsFixedSourceVisibleExpression(semanticModel, initializerSyntax, cancellationToken) ||
              ContainsOpaqueSourceExpansion(semanticModel, initializerSyntax, cancellationToken)) {
            continue;
          }

          var beforeCount = varyingSymbols.Count;
          AddVaryingSourceReferences(
              semanticModel,
              initializerSyntax,
              visibleSourceScope,
              lambdaSyntax,
              varyingSymbols,
              ref reportLocation,
              cancellationToken,
              visitedSymbols.Add(localSymbol));
          if (beforeCount != varyingSymbols.Count) {
            continue;
          }
        }

        continue;
      }

      if (symbol is IParameterSymbol parameterSymbol &&
          !IsDbContextOptionsBuilderType(parameterSymbol.Type) &&
          !IsParameterDeclaredInLambda(parameterSymbol, lambdaSyntax, cancellationToken)) {
        AddVaryingSourceReference(parameterSymbol, node.GetLocation(), varyingSymbols, ref reportLocation);
      }
    }
  }

  private static void AddVaryingSourceReference(
      ISymbol symbol,
      Location location,
      ImmutableArray<ISymbol>.Builder varyingSymbols,
      ref Location? reportLocation) {
    if (ContainsSymbol(varyingSymbols, symbol)) {
      return;
    }

    varyingSymbols.Add(symbol);
    reportLocation ??= location;
  }

  private static bool ContainsOpaqueSourceExpansion(
      SemanticModel semanticModel,
      SyntaxNode scope,
      CancellationToken cancellationToken) {
    foreach (var invocationSyntax in scope.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>()) {
      if (semanticModel.GetOperation(invocationSyntax, cancellationToken) is IInvocationOperation invocation &&
          IsProviderOptionsInvocation(invocation)) {
        continue;
      }

      return true;
    }

    return scope.DescendantNodesAndSelf().OfType<ObjectCreationExpressionSyntax>().Any();
  }

  private static bool IsParameterDeclaredInLambda(
      IParameterSymbol parameterSymbol,
      LambdaExpressionSyntax lambdaSyntax,
      CancellationToken cancellationToken) {
    foreach (var syntaxReference in parameterSymbol.DeclaringSyntaxReferences) {
      if (syntaxReference.GetSyntax(cancellationToken).FirstAncestorOrSelf<LambdaExpressionSyntax>() is { } parameterLambda &&
          ReferenceEquals(parameterLambda, lambdaSyntax)) {
        return true;
      }
    }

    return false;
  }

  private static ImmutableArray<IInvocationOperation> GetDataVaultModelProjectionInvocations(
      SemanticModel semanticModel,
      MethodDeclarationSyntax methodSyntax,
      CancellationToken cancellationToken) {
    var invocations = ImmutableArray.CreateBuilder<IInvocationOperation>();
    foreach (var invocationSyntax in methodSyntax.DescendantNodes().OfType<InvocationExpressionSyntax>()) {
      if (semanticModel.GetOperation(invocationSyntax, cancellationToken) is IInvocationOperation invocation &&
          IsApplyDataVaultMetadataInvocation(invocation)) {
        invocations.Add(invocation);
      }
    }

    return invocations.ToImmutable();
  }

  private static bool HasVisibleDataVaultProjection(
      INamedTypeSymbol typeSymbol,
      Compilation compilation,
      CancellationToken cancellationToken) {
    if (!IsDbContextTypeOrDerived(typeSymbol)) {
      return false;
    }

    foreach (var methodSymbol in typeSymbol.GetMembers("OnModelCreating").OfType<IMethodSymbol>()) {
      foreach (var syntaxReference in methodSymbol.DeclaringSyntaxReferences) {
        if (syntaxReference.GetSyntax(cancellationToken) is not MethodDeclarationSyntax methodSyntax) {
          continue;
        }

        var semanticModel = compilation.GetSemanticModel(methodSyntax.SyntaxTree);
        if (!GetDataVaultModelProjectionInvocations(semanticModel, methodSyntax, cancellationToken).IsEmpty) {
          return true;
        }
      }
    }

    return false;
  }

  private static IEnumerable<ExpressionSyntax> GetProjectionControlExpressions(SyntaxNode projectionSyntax) {
    foreach (var ancestor in projectionSyntax.Ancestors()) {
      switch (ancestor) {
        case IfStatementSyntax ifStatement:
          yield return ifStatement.Condition;
          break;
        case ConditionalExpressionSyntax conditionalExpression:
          yield return conditionalExpression.Condition;
          break;
        case SwitchStatementSyntax switchStatement:
          yield return switchStatement.Expression;
          break;
        case SwitchExpressionSyntax switchExpression:
          yield return switchExpression.GoverningExpression;
          break;
      }
    }
  }

  private static bool IsDataVaultShapeAffectingInvocation(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocationSyntax,
      IInvocationOperation invocation,
      CancellationToken cancellationToken) {
    if (IsApplyDataVaultMetadataInvocation(invocation)) {
      return true;
    }

    if (string.Equals(invocation.TargetMethod.Name, HasDefaultSchemaMethodName, StringComparison.Ordinal) &&
        invocation.Instance?.Type is INamedTypeSymbol modelBuilderType &&
        IsNamedType(modelBuilderType, EfCoreNamespaceName, "ModelBuilder", arity: 0)) {
      return true;
    }

    return string.Equals(invocation.TargetMethod.Name, ToTableMethodName, StringComparison.Ordinal) &&
        IsInsideGeneratedSharedTypeEntityConfiguration(semanticModel, invocationSyntax, cancellationToken);
  }

  private static bool IsInsideGeneratedSharedTypeEntityConfiguration(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocationSyntax,
      CancellationToken cancellationToken) {
    foreach (var ancestorInvocationSyntax in invocationSyntax.Ancestors().OfType<InvocationExpressionSyntax>()) {
      if (semanticModel.GetOperation(ancestorInvocationSyntax, cancellationToken) is not IInvocationOperation ancestorInvocation ||
          !string.Equals(ancestorInvocation.TargetMethod.Name, SharedTypeEntityMethodName, StringComparison.Ordinal)) {
        continue;
      }

      if (ancestorInvocation.TargetMethod.TypeArguments.Length == 1 &&
          IsDictionaryStringObject(ancestorInvocation.TargetMethod.TypeArguments[0]) &&
          TryGetConstantStringArgument(ancestorInvocation, out var tableName) &&
          IsGeneratedDataVaultTableName(tableName)) {
        return true;
      }
    }

    return false;
  }

  private static void AddVaryingContextMemberReferences(
      SemanticModel semanticModel,
      SyntaxNode scope,
      INamedTypeSymbol contextType,
      ImmutableArray<ISymbol>.Builder varyingMembers,
      ref Location? reportLocation,
      CancellationToken cancellationToken) {
    AddVaryingContextMemberReferences(
        semanticModel,
        scope,
        contextType,
        varyingMembers,
        ref reportLocation,
        cancellationToken,
        GetVisibleSourceScope(scope) ?? scope,
        ImmutableArray<ISymbol>.Empty);
  }

  private static void AddVaryingContextMemberReferences(
      SemanticModel semanticModel,
      SyntaxNode scope,
      INamedTypeSymbol contextType,
      ImmutableArray<ISymbol>.Builder varyingMembers,
      ref Location? reportLocation,
      CancellationToken cancellationToken,
      SyntaxNode visibleSourceScope,
      ImmutableArray<ISymbol> visitedSymbols) {
    foreach (var memberReference in GetContextInstanceMemberReferences(
        semanticModel,
        scope,
        visibleSourceScope,
        contextType,
        cancellationToken,
        visitedSymbols)) {
      if (IsFixedSourceVisibleStateMember(semanticModel, memberReference.Symbol, cancellationToken) ||
          ContainsSymbol(varyingMembers, memberReference.Symbol)) {
        continue;
      }

      varyingMembers.Add(memberReference.Symbol);
      reportLocation ??= memberReference.Location;
    }
  }

  private static bool IsFixedSourceVisibleStateMember(
      SemanticModel semanticModel,
      ISymbol symbol,
      CancellationToken cancellationToken) {
    if (symbol is IPropertySymbol { SetMethod: not null } ||
        symbol is IFieldSymbol { IsReadOnly: false }) {
      return false;
    }

    foreach (var syntaxReference in symbol.DeclaringSyntaxReferences) {
      var syntax = syntaxReference.GetSyntax(cancellationToken);
      if (syntax is PropertyDeclarationSyntax propertySyntax &&
          IsFixedSourceVisibleProperty(semanticModel, propertySyntax, cancellationToken)) {
        return true;
      }

      if (syntax is VariableDeclaratorSyntax { Initializer.Value: { } initializer } &&
          IsFixedSourceVisibleExpression(semanticModel, initializer, cancellationToken)) {
        return true;
      }
    }

    return false;
  }

  private static bool IsFixedSourceVisibleProperty(
      SemanticModel semanticModel,
      PropertyDeclarationSyntax propertySyntax,
      CancellationToken cancellationToken) {
    if (propertySyntax.Initializer?.Value is { } initializer &&
        IsFixedSourceVisibleExpression(semanticModel, initializer, cancellationToken)) {
      return true;
    }

    if (propertySyntax.ExpressionBody?.Expression is { } expressionBody &&
        IsFixedSourceVisibleExpression(semanticModel, expressionBody, cancellationToken)) {
      return true;
    }

    var getAccessor = propertySyntax.AccessorList?.Accessors.FirstOrDefault(
        accessor => accessor.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.GetAccessorDeclaration));
    if (getAccessor?.ExpressionBody?.Expression is { } accessorExpression &&
        IsFixedSourceVisibleExpression(semanticModel, accessorExpression, cancellationToken)) {
      return true;
    }

    if (getAccessor?.Body is null) {
      return false;
    }

    var returnExpressions = getAccessor.Body.Statements
        .OfType<ReturnStatementSyntax>()
        .Select(returnStatement => returnStatement.Expression)
        .ToArray();

    return returnExpressions.Length > 0 &&
        returnExpressions.All(returnExpression =>
            returnExpression is not null &&
            IsFixedSourceVisibleExpression(semanticModel, returnExpression, cancellationToken));
  }

  private static bool IsFixedSourceVisibleExpression(
      SemanticModel semanticModel,
      ExpressionSyntax expression,
      CancellationToken cancellationToken) {
    var expressionSemanticModel = semanticModel.Compilation.GetSemanticModel(expression.SyntaxTree);
    return IsFixedSourceVisibleOperation(
        expressionSemanticModel.GetOperation(expression, cancellationToken),
        expressionSemanticModel,
        expression,
        cancellationToken);
  }

  private static bool IsFixedSourceVisibleOperation(
      IOperation? operation,
      SemanticModel semanticModel,
      SyntaxNode scope,
      CancellationToken cancellationToken) {
    operation = Unwrap(operation);
    if (operation is null) {
      return false;
    }

    if (operation.ConstantValue.HasValue) {
      return true;
    }

    if (operation is ILocalReferenceOperation localReference &&
        TryGetLocalInitializerOperation(
            semanticModel,
            localReference.Local,
            GetVisibleSourceScope(scope) ?? scope,
            cancellationToken,
            out var initializerOperation)) {
      return IsFixedSourceVisibleOperation(
          initializerOperation,
          semanticModel,
          scope,
          cancellationToken);
    }

    return operation is IMemberReferenceOperation { Member.IsStatic: true };
  }

  private static IEnumerable<MemberReference> GetContextInstanceMemberReferences(
      SemanticModel semanticModel,
      SyntaxNode scope,
      SyntaxNode visibleSourceScope,
      INamedTypeSymbol contextType,
      CancellationToken cancellationToken,
      ImmutableArray<ISymbol> visitedSymbols) {
    foreach (var node in scope.DescendantNodesAndSelf()) {
      if (node is not IdentifierNameSyntax and not MemberAccessExpressionSyntax) {
        continue;
      }

      var symbol = semanticModel.GetSymbolInfo(node, cancellationToken).Symbol;
      if (IsContextInstanceStateMember(symbol, contextType)) {
        yield return new MemberReference(symbol!, node.GetLocation());
        continue;
      }

      if (symbol is ILocalSymbol localSymbol &&
          !ContainsSymbol(visitedSymbols, localSymbol) &&
          TryGetLocalInitializerSyntax(
              semanticModel,
              localSymbol,
              visibleSourceScope,
              cancellationToken,
              out var initializerSyntax)) {
        foreach (var memberReference in GetContextInstanceMemberReferences(
            semanticModel,
            initializerSyntax,
            visibleSourceScope,
            contextType,
            cancellationToken,
            visitedSymbols.Add(localSymbol))) {
          yield return memberReference;
        }
      }
    }
  }

  private static bool IsContextInstanceStateMember(ISymbol? symbol, INamedTypeSymbol contextType) {
    return symbol is IPropertySymbol or IFieldSymbol &&
        !symbol.IsStatic &&
        !symbol.IsImplicitlyDeclared &&
        SymbolEqualityComparer.Default.Equals(symbol.ContainingType, contextType);
  }

  private static bool IsApplyDataVaultMetadataInvocation(IInvocationOperation invocation) {
    var originalDefinition = invocation.TargetMethod.ReducedFrom?.OriginalDefinition ??
        invocation.TargetMethod.OriginalDefinition;

    return string.Equals(originalDefinition.Name, ApplyDataVaultMetadataMethodName, StringComparison.Ordinal) &&
        string.Equals(
            originalDefinition.ContainingNamespace.ToDisplayString(),
            DataVaultNamespaceName,
            StringComparison.Ordinal);
  }

  private static CacheKeyCoverage GetVisibleCacheKeyCoverage(
      Compilation compilation,
      INamedTypeSymbol contextType,
      ImmutableArray<ISymbol> varyingMembers,
      CancellationToken cancellationToken) {
    var replacements = GetVisibleModelCacheKeyFactoryReplacements(
        compilation,
        contextType,
        cancellationToken);
    if (replacements.IsEmpty) {
      return CacheKeyCoverage.MissingReplacement;
    }

    var hasOmittedKey = false;
    foreach (var replacement in replacements) {
      var coverage = GetFactoryKeyCoverage(replacement, contextType, varyingMembers, compilation, cancellationToken);
      if (coverage == CacheKeyCoverage.Sufficient) {
        return CacheKeyCoverage.Sufficient;
      }

      if (coverage == CacheKeyCoverage.Opaque) {
        return CacheKeyCoverage.Opaque;
      }

      hasOmittedKey = true;
    }

    return hasOmittedKey ? CacheKeyCoverage.OmitsVaryingMembers : CacheKeyCoverage.Opaque;
  }

  private static ImmutableArray<INamedTypeSymbol> GetVisibleModelCacheKeyFactoryReplacements(
      Compilation compilation,
      INamedTypeSymbol contextType,
      CancellationToken cancellationToken) {
    var replacements = ImmutableArray.CreateBuilder<INamedTypeSymbol>();

    foreach (var syntaxTree in compilation.SyntaxTrees) {
      cancellationToken.ThrowIfCancellationRequested();
      var semanticModel = compilation.GetSemanticModel(syntaxTree);
      var root = syntaxTree.GetRoot(cancellationToken);

      foreach (var invocationSyntax in root.DescendantNodes().OfType<InvocationExpressionSyntax>()) {
        if (semanticModel.GetOperation(invocationSyntax, cancellationToken) is not IInvocationOperation invocation ||
            !IsModelCacheKeyFactoryReplacement(invocation, out var replacementFactoryType) ||
            !TryGetContextTypeFromCacheKeyReplacementInvocation(
                semanticModel,
                invocationSyntax,
                invocation,
                cancellationToken,
                out var replacementContextType) ||
            !SymbolEqualityComparer.Default.Equals(replacementContextType, contextType) ||
            ContainsNamedType(replacements, replacementFactoryType)) {
          continue;
        }

        replacements.Add(replacementFactoryType);
      }
    }

    return replacements.ToImmutable();
  }

  private static bool IsModelCacheKeyFactoryReplacement(
      IInvocationOperation invocation,
      out INamedTypeSymbol replacementFactoryType) {
    replacementFactoryType = null!;
    var originalDefinition = invocation.TargetMethod.ReducedFrom?.OriginalDefinition ??
        invocation.TargetMethod.OriginalDefinition;

    if (!string.Equals(originalDefinition.Name, ReplaceServiceMethodName, StringComparison.Ordinal) ||
        invocation.TargetMethod.TypeArguments.Length != 2 ||
        !IsModelCacheKeyFactoryType(invocation.TargetMethod.TypeArguments[0]) ||
        invocation.TargetMethod.TypeArguments[1] is not INamedTypeSymbol factoryType ||
        !ImplementsModelCacheKeyFactory(factoryType)) {
      return false;
    }

    replacementFactoryType = factoryType;
    return true;
  }

  private static bool TryGetContextTypeFromCacheKeyReplacementInvocation(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocationSyntax,
      IInvocationOperation invocation,
      CancellationToken cancellationToken,
      out INamedTypeSymbol contextType) {
    if (TryGetContextTypeFromOptionsBuilderReceiver(invocation, out contextType)) {
      return true;
    }

    if (TryGetContextTypeFromEnclosingDbContextRegistration(
        semanticModel,
        invocationSyntax,
        cancellationToken,
        out contextType)) {
      return true;
    }

    if (TryGetContainingDbContextType(semanticModel, invocationSyntax, cancellationToken, out contextType)) {
      return true;
    }

    contextType = null!;
    return false;
  }

  private static bool TryGetContextTypeFromEnclosingDbContextRegistration(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocationSyntax,
      CancellationToken cancellationToken,
      out INamedTypeSymbol contextType) {
    foreach (var lambdaSyntax in invocationSyntax.Ancestors().OfType<LambdaExpressionSyntax>()) {
      if (lambdaSyntax.Parent is not ArgumentSyntax argumentSyntax ||
          !ReferenceEquals(argumentSyntax.Expression, lambdaSyntax) ||
          argumentSyntax.Parent is not ArgumentListSyntax { Parent: InvocationExpressionSyntax registrationSyntax } ||
          semanticModel.GetOperation(registrationSyntax, cancellationToken) is not IInvocationOperation registrationInvocation ||
          !IsDbContextRegistrationInvocation(registrationInvocation, out contextType)) {
        continue;
      }

      return true;
    }

    contextType = null!;
    return false;
  }

  private static bool TryGetContainingDbContextType(
      SemanticModel semanticModel,
      SyntaxNode node,
      CancellationToken cancellationToken,
      out INamedTypeSymbol contextType) {
    var typeSyntax = node.FirstAncestorOrSelf<TypeDeclarationSyntax>();
    if (typeSyntax is not null &&
        semanticModel.GetDeclaredSymbol(typeSyntax, cancellationToken) is INamedTypeSymbol containingType &&
        IsDbContextTypeOrDerived(containingType)) {
      contextType = containingType;
      return true;
    }

    contextType = null!;
    return false;
  }

  private static bool IsVisibleDesignModelRuntimeModelLane(
      Compilation compilation,
      IInvocationOperation useModelInvocation,
      INamedTypeSymbol contextType,
      CancellationToken cancellationToken) {
    if (useModelInvocation.Arguments.FirstOrDefault(argument => argument.Parameter?.Ordinal == 0) is not { } modelArgument) {
      return false;
    }

    var semanticModel = compilation.GetSemanticModel(useModelInvocation.Syntax.SyntaxTree);
    var scope = GetVisibleSourceScope(useModelInvocation.Syntax);
    if (scope is null) {
      return false;
    }

    return IsRuntimeModelInitializerResult(
        semanticModel,
        Unwrap(modelArgument.Value),
        scope,
        cancellationToken,
        ImmutableArray<ISymbol>.Empty,
        out var designContextOperation) &&
        IsFixedDesignModelContextExpression(
            semanticModel,
            designContextOperation,
            scope,
            contextType,
            cancellationToken,
            ImmutableArray<ISymbol>.Empty);
  }

  private static SyntaxNode? GetVisibleSourceScope(SyntaxNode syntax) {
    return syntax.FirstAncestorOrSelf<BaseMethodDeclarationSyntax>() ??
        (SyntaxNode?)syntax.FirstAncestorOrSelf<AccessorDeclarationSyntax>() ??
        syntax.FirstAncestorOrSelf<AnonymousFunctionExpressionSyntax>();
  }

  private static bool IsRuntimeModelInitializerResult(
      SemanticModel semanticModel,
      IOperation? operation,
      SyntaxNode scope,
      CancellationToken cancellationToken,
      ImmutableArray<ISymbol> visitedSymbols,
      out IOperation? designContextOperation) {
    designContextOperation = null;
    operation = Unwrap(operation);
    if (operation is null) {
      return false;
    }

    if (operation is ILocalReferenceOperation localReference) {
      if (ContainsSymbol(visitedSymbols, localReference.Local) ||
          TryGetLocalInitializerOperation(
              semanticModel,
              localReference.Local,
              scope,
              cancellationToken,
              out var initializerOperation) is false) {
        return false;
      }

      return IsRuntimeModelInitializerResult(
          semanticModel,
          initializerOperation,
          scope,
          cancellationToken,
          visitedSymbols.Add(localReference.Local),
          out designContextOperation);
    }

    if (operation is not IInvocationOperation initializeInvocation ||
        !IsModelRuntimeInitializerInvocation(initializeInvocation) ||
        initializeInvocation.Arguments.FirstOrDefault(argument => argument.Parameter?.Ordinal == 0) is not { } modelArgument) {
      return false;
    }

    return IsDesignModelExpression(
        semanticModel,
        Unwrap(modelArgument.Value),
        scope,
        cancellationToken,
        visitedSymbols,
        out designContextOperation);
  }

  private static bool IsDesignModelExpression(
      SemanticModel semanticModel,
      IOperation? operation,
      SyntaxNode scope,
      CancellationToken cancellationToken,
      ImmutableArray<ISymbol> visitedSymbols,
      out IOperation? designContextOperation) {
    designContextOperation = null;
    operation = Unwrap(operation);
    if (operation is null) {
      return false;
    }

    if (operation is ILocalReferenceOperation localReference) {
      if (ContainsSymbol(visitedSymbols, localReference.Local) ||
          TryGetLocalInitializerOperation(
              semanticModel,
              localReference.Local,
              scope,
              cancellationToken,
              out var initializerOperation) is false) {
        return false;
      }

      return IsDesignModelExpression(
          semanticModel,
          initializerOperation,
          scope,
          cancellationToken,
          visitedSymbols.Add(localReference.Local),
          out designContextOperation);
    }

    return operation is IPropertyReferenceOperation propertyReference &&
        string.Equals(propertyReference.Property.Name, ModelPropertyName, StringComparison.Ordinal) &&
        IsDesignTimeModelType(propertyReference.Property.ContainingType) &&
        IsDesignTimeModelServiceExpression(propertyReference.Instance, out designContextOperation);
  }

  private static bool IsFixedDesignModelContextExpression(
      SemanticModel semanticModel,
      IOperation? operation,
      SyntaxNode scope,
      INamedTypeSymbol contextType,
      CancellationToken cancellationToken,
      ImmutableArray<ISymbol> visitedSymbols) {
    operation = Unwrap(operation);
    if (operation is null) {
      return false;
    }

    if (operation is ILocalReferenceOperation localReference) {
      if (ContainsSymbol(visitedSymbols, localReference.Local) ||
          TryGetLocalInitializerOperation(
              semanticModel,
              localReference.Local,
              scope,
              cancellationToken,
              out var initializerOperation) is false) {
        return false;
      }

      return IsFixedDesignModelContextExpression(
          semanticModel,
          initializerOperation,
          scope,
          contextType,
          cancellationToken,
          visitedSymbols.Add(localReference.Local));
    }

    if (operation is not IObjectCreationOperation objectCreation ||
        objectCreation.Type is not INamedTypeSymbol createdType ||
        !SymbolEqualityComparer.Default.Equals(createdType, contextType)) {
      return false;
    }

    foreach (var argument in objectCreation.Arguments) {
      if (argument.Parameter?.Type is { } parameterType &&
          IsDbContextOptionsType(parameterType)) {
        continue;
      }

      if (!IsFixedModelShapeArgument(
          semanticModel,
          argument.Value,
          scope,
          cancellationToken,
          ImmutableArray<ISymbol>.Empty)) {
        return false;
      }
    }

    return true;
  }

  private static bool IsFixedModelShapeArgument(
      SemanticModel semanticModel,
      IOperation? operation,
      SyntaxNode scope,
      CancellationToken cancellationToken,
      ImmutableArray<ISymbol> visitedSymbols) {
    operation = Unwrap(operation);
    if (operation is null) {
      return false;
    }

    if (operation.ConstantValue.HasValue) {
      return true;
    }

    if (operation is ILocalReferenceOperation localReference) {
      if (ContainsSymbol(visitedSymbols, localReference.Local) ||
          TryGetLocalInitializerOperation(
              semanticModel,
              localReference.Local,
              scope,
              cancellationToken,
              out var initializerOperation) is false) {
        return false;
      }

      return IsFixedModelShapeArgument(
          semanticModel,
          initializerOperation,
          scope,
          cancellationToken,
          visitedSymbols.Add(localReference.Local));
    }

    if (operation is IMemberReferenceOperation memberReference &&
        memberReference.Member.IsStatic) {
      return true;
    }

    return false;
  }

  private static bool TryGetLocalInitializerOperation(
      SemanticModel semanticModel,
      ILocalSymbol localSymbol,
      SyntaxNode scope,
      CancellationToken cancellationToken,
      out IOperation? initializerOperation) {
    if (TryGetLocalInitializerSyntax(
        semanticModel,
        localSymbol,
        scope,
        cancellationToken,
        out var initializerSyntax)) {
      initializerOperation = semanticModel.GetOperation(initializerSyntax, cancellationToken);
      return true;
    }

    initializerOperation = null;
    return false;
  }

  private static bool TryGetLocalInitializerSyntax(
      SemanticModel semanticModel,
      ILocalSymbol localSymbol,
      SyntaxNode scope,
      CancellationToken cancellationToken,
      out ExpressionSyntax initializerSyntax) {
    foreach (var declaratorSyntax in scope.DescendantNodes().OfType<VariableDeclaratorSyntax>()) {
      if (declaratorSyntax.Initializer?.Value is null ||
          semanticModel.GetDeclaredSymbol(declaratorSyntax, cancellationToken) is not ILocalSymbol declaredLocal ||
          !SymbolEqualityComparer.Default.Equals(declaredLocal, localSymbol)) {
        continue;
      }

      initializerSyntax = declaratorSyntax.Initializer.Value;
      return true;
    }

    initializerSyntax = null!;
    return false;
  }

  private static bool IsModelRuntimeInitializerInvocation(IInvocationOperation invocation) {
    return string.Equals(invocation.TargetMethod.Name, InitializeMethodName, StringComparison.Ordinal) &&
        invocation.Instance?.Type is not null &&
        IsModelRuntimeInitializerType(invocation.Instance.Type);
  }

  private static bool IsDesignTimeModelServiceExpression(
      IOperation? operation,
      out IOperation? designContextOperation) {
    designContextOperation = null;
    operation = Unwrap(operation);

    if (operation?.Type is not null &&
        IsDesignTimeModelType(operation.Type) &&
        operation is IInvocationOperation invocation &&
        string.Equals(invocation.TargetMethod.Name, GetServiceMethodName, StringComparison.Ordinal)) {
      designContextOperation = invocation.Instance ??
          invocation.Arguments.FirstOrDefault(argument => argument.Parameter?.Ordinal == 0)?.Value;
      return true;
    }

    return false;
  }

  private static CacheKeyCoverage GetFactoryKeyCoverage(
      INamedTypeSymbol factoryType,
      INamedTypeSymbol contextType,
      ImmutableArray<ISymbol> varyingMembers,
      Compilation compilation,
      CancellationToken cancellationToken) {
    var inspectedReturn = false;

    foreach (var createMethod in factoryType.GetMembers("Create").OfType<IMethodSymbol>()) {
      foreach (var syntaxReference in createMethod.DeclaringSyntaxReferences) {
        if (syntaxReference.GetSyntax(cancellationToken) is not MethodDeclarationSyntax methodSyntax) {
          continue;
        }

        var semanticModel = compilation.GetSemanticModel(methodSyntax.SyntaxTree);
        foreach (var returnStatement in methodSyntax.DescendantNodes().OfType<ReturnStatementSyntax>()) {
          if (returnStatement.Expression is null) {
            continue;
          }

          inspectedReturn = true;
          if (IsOpaqueFactoryKeyExpression(
              semanticModel,
              returnStatement.Expression,
              cancellationToken)) {
            return CacheKeyCoverage.Opaque;
          }

          var referencedMembers = GetContextInstanceMemberReferences(
              semanticModel,
              returnStatement.Expression,
              methodSyntax,
              contextType,
              cancellationToken,
              ImmutableArray<ISymbol>.Empty)
              .Select(reference => reference.Symbol)
              .ToImmutableArray();
          if (ContainsAllSymbols(referencedMembers, varyingMembers)) {
            return CacheKeyCoverage.Sufficient;
          }
        }
      }
    }

    return inspectedReturn ? CacheKeyCoverage.OmitsVaryingMembers : CacheKeyCoverage.Opaque;
  }

  private static bool IsOpaqueFactoryKeyExpression(
      SemanticModel semanticModel,
      ExpressionSyntax expression,
      CancellationToken cancellationToken) {
    foreach (var invocationSyntax in expression.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>()) {
      var symbol = semanticModel.GetSymbolInfo(invocationSyntax, cancellationToken).Symbol as IMethodSymbol;
      if (symbol is not null &&
          !IsTransparentFactoryKeyInvocation(symbol)) {
        return true;
      }
    }

    return expression.DescendantNodesAndSelf().OfType<ObjectCreationExpressionSyntax>().Any();
  }

  private static bool IsTransparentFactoryKeyInvocation(IMethodSymbol methodSymbol) {
    return string.Equals(methodSymbol.Name, nameof(object.GetType), StringComparison.Ordinal) &&
        methodSymbol.Parameters.Length == 0;
  }

  private static bool IsModelCacheKeyFactoryType(ITypeSymbol typeSymbol) {
    return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
        IsNamedType(namedTypeSymbol, EfCoreInfrastructureNamespaceName, ModelCacheKeyFactoryTypeName, arity: 0);
  }

  private static bool IsDesignTimeModelType(ITypeSymbol typeSymbol) {
    return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
        IsNamedType(namedTypeSymbol, EfCoreInfrastructureNamespaceName, DesignTimeModelTypeName, arity: 0);
  }

  private static bool IsModelRuntimeInitializerType(ITypeSymbol typeSymbol) {
    return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
        IsNamedType(namedTypeSymbol, EfCoreInfrastructureNamespaceName, ModelRuntimeInitializerTypeName, arity: 0);
  }

  private static bool IsModelType(ITypeSymbol typeSymbol) {
    return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
        IsNamedType(namedTypeSymbol, EfCoreMetadataNamespaceName, ModelTypeName, arity: 0);
  }

  private static bool ImplementsModelCacheKeyFactory(INamedTypeSymbol typeSymbol) {
    foreach (var interfaceType in typeSymbol.AllInterfaces) {
      if (IsNamedType(interfaceType, EfCoreInfrastructureNamespaceName, ModelCacheKeyFactoryTypeName, arity: 0)) {
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

  private static bool TryGetContextTypeFromOptionsBuilderReceiver(
      IInvocationOperation invocation,
      out INamedTypeSymbol contextType) {
    var receiverType = Unwrap(invocation.Instance)?.Type as INamedTypeSymbol;
    if (receiverType is not null &&
        IsNamedType(receiverType.OriginalDefinition, EfCoreNamespaceName, DbContextOptionsBuilderTypeName, arity: 1) &&
        receiverType.TypeArguments.Length == 1 &&
        receiverType.TypeArguments[0] is INamedTypeSymbol receiverContextType &&
        IsDbContextTypeOrDerived(receiverContextType)) {
      contextType = receiverContextType;
      return true;
    }

    contextType = null!;
    return false;
  }

  private static bool IsDbContextOptionsType(ITypeSymbol typeSymbol) {
    if (typeSymbol is not INamedTypeSymbol namedTypeSymbol) {
      return false;
    }

    return IsNamedType(namedTypeSymbol, EfCoreNamespaceName, "DbContextOptions", arity: 0) ||
        IsNamedType(namedTypeSymbol.OriginalDefinition, EfCoreNamespaceName, "DbContextOptions", arity: 1);
  }

  private static bool IsDbContextOptionsBuilderType(ITypeSymbol typeSymbol) {
    if (typeSymbol is not INamedTypeSymbol namedTypeSymbol) {
      return false;
    }

    return IsNamedType(namedTypeSymbol, EfCoreNamespaceName, DbContextOptionsBuilderTypeName, arity: 0) ||
        IsNamedType(namedTypeSymbol.OriginalDefinition, EfCoreNamespaceName, DbContextOptionsBuilderTypeName, arity: 1);
  }

  private static bool IsProviderOptionsInvocation(IInvocationOperation invocation) {
    if (!ProviderOptionsMethodNames.Contains(invocation.TargetMethod.Name)) {
      return false;
    }

    var receiverType = Unwrap(invocation.Instance)?.Type;
    if (receiverType is not null &&
        IsDbContextOptionsBuilderType(receiverType)) {
      return true;
    }

    return invocation.TargetMethod.Parameters.Length > 0 &&
        IsDbContextOptionsBuilderType(invocation.TargetMethod.Parameters[0].Type);
  }

  private static bool TryGetContextTypeFromOptionsBuilderOrRegistration(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocationSyntax,
      IInvocationOperation invocation,
      CancellationToken cancellationToken,
      out INamedTypeSymbol contextType) {
    if (TryGetContextTypeFromOptionsBuilderReceiver(invocation, out contextType)) {
      return true;
    }

    return TryGetContextTypeFromEnclosingDbContextRegistration(
        semanticModel,
        invocationSyntax,
        cancellationToken,
        out contextType);
  }

  private static bool IsUseModelInvocation(IInvocationOperation invocation) {
    var originalDefinition = invocation.TargetMethod.ReducedFrom?.OriginalDefinition ??
        invocation.TargetMethod.OriginalDefinition;

    return string.Equals(originalDefinition.Name, UseModelMethodName, StringComparison.Ordinal) &&
        string.Equals(
            originalDefinition.ContainingNamespace.ToDisplayString(),
            EfCoreNamespaceName,
            StringComparison.Ordinal) &&
        originalDefinition.Parameters.Length == 1 &&
        IsModelType(originalDefinition.Parameters[0].Type);
  }

  private static bool IsAddDbContextPoolInvocation(
      IInvocationOperation invocation,
      out INamedTypeSymbol contextType) {
    var originalDefinition = invocation.TargetMethod.ReducedFrom?.OriginalDefinition ??
        invocation.TargetMethod.OriginalDefinition;

    if (IsDbContextRegistrationInvocation(invocation, out contextType) &&
        string.Equals(originalDefinition.Name, AddDbContextPoolMethodName, StringComparison.Ordinal)) {
      return true;
    }

    contextType = null!;
    return false;
  }

  private static bool IsDbContextRegistrationInvocation(
      IInvocationOperation invocation,
      out INamedTypeSymbol contextType) {
    var originalDefinition = invocation.TargetMethod.ReducedFrom?.OriginalDefinition ??
        invocation.TargetMethod.OriginalDefinition;

    if ((string.Equals(originalDefinition.Name, AddDbContextMethodName, StringComparison.Ordinal) ||
        string.Equals(originalDefinition.Name, AddDbContextPoolMethodName, StringComparison.Ordinal)) &&
        string.Equals(
            originalDefinition.ContainingNamespace.ToDisplayString(),
            DependencyInjectionNamespaceName,
            StringComparison.Ordinal) &&
        invocation.TargetMethod.TypeArguments.Length > 0 &&
        invocation.TargetMethod.TypeArguments[0] is INamedTypeSymbol pooledContextType &&
        IsDbContextTypeOrDerived(pooledContextType)) {
      contextType = pooledContextType;
      return true;
    }

    contextType = null!;
    return false;
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

  private static bool ContainsSymbol(ImmutableArray<ISymbol>.Builder symbols, ISymbol candidate) {
    foreach (var symbol in symbols) {
      if (SymbolEqualityComparer.Default.Equals(symbol, candidate)) {
        return true;
      }
    }

    return false;
  }

  private static bool ContainsSymbol(ImmutableArray<ISymbol> symbols, ISymbol candidate) {
    foreach (var symbol in symbols) {
      if (SymbolEqualityComparer.Default.Equals(symbol, candidate)) {
        return true;
      }
    }

    return false;
  }

  private static bool ContainsAllSymbols(
      ImmutableArray<ISymbol> availableSymbols,
      ImmutableArray<ISymbol> requiredSymbols) {
    foreach (var requiredSymbol in requiredSymbols) {
      var found = false;
      foreach (var availableSymbol in availableSymbols) {
        if (SymbolEqualityComparer.Default.Equals(availableSymbol, requiredSymbol)) {
          found = true;
          break;
        }
      }

      if (!found) {
        return false;
      }
    }

    return true;
  }

  private static bool ContainsNamedType(
      ImmutableArray<INamedTypeSymbol>.Builder symbols,
      INamedTypeSymbol candidate) {
    foreach (var symbol in symbols) {
      if (SymbolEqualityComparer.Default.Equals(symbol, candidate)) {
        return true;
      }
    }

    return false;
  }

  private enum CacheKeyCoverage {
    MissingReplacement,
    OmitsVaryingMembers,
    Sufficient,
    Opaque,
  }

  private readonly record struct MemberReference(ISymbol Symbol, Location Location);

  private readonly record struct ContextLifecycleShape(
      ImmutableArray<ISymbol> VaryingMembers,
      Location? ReportLocation) {
    public static ContextLifecycleShape None { get; } = new(ImmutableArray<ISymbol>.Empty, null);

    public bool HasVariableDataVaultShape => !VaryingMembers.IsEmpty && ReportLocation is not null;

    public string GetVaryingMemberDisplayList() {
      return string.Join(
          ", ",
          VaryingMembers
              .Select(member => member.Name)
              .Distinct(StringComparer.Ordinal)
              .OrderBy(name => name, StringComparer.Ordinal));
    }
  }
}
