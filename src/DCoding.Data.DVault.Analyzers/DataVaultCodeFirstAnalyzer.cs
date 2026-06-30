using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace DCoding.Data.DVault.Analyzers;

/// <summary>
/// Reports low-noise diagnostics for DVault Code-First fluent selector declarations.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DataVaultCodeFirstAnalyzer : DiagnosticAnalyzer {
  private const string NamespaceName = "DCoding.Data.DVault";
  private const string HubBuilderTypeName = "DataVaultCodeFirstHubBuilder";
  private const string SatelliteBuilderTypeName = "DataVaultCodeFirstSatelliteBuilder";
  private const string BusinessKeyVerb = "BusinessKey";
  private const string PayloadVerb = "Payload";
  private const string DrivingKeyVerb = "DrivingKey";

  /// <inheritdoc />
  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
      CodeFirstDiagnosticCatalog.UnsupportedSelector,
      CodeFirstDiagnosticCatalog.DuplicateMember);

  /// <inheritdoc />
  public override void Initialize(AnalysisContext context) {
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    context.EnableConcurrentExecution();
    context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    context.RegisterSyntaxNodeAction(
        AnalyzeBuilderLambdaScope,
        SyntaxKind.SimpleLambdaExpression,
        SyntaxKind.ParenthesizedLambdaExpression);
  }

  private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context) {
    var invocation = (InvocationExpressionSyntax)context.Node;
    if (!TryGetCodeFirstVerb(context.SemanticModel, invocation, context.CancellationToken, out var verb)) {
      return;
    }

    var selectorArgument = invocation.ArgumentList.Arguments.FirstOrDefault();
    if (selectorArgument is null) {
      return;
    }

    var selectorExpression = UnwrapParenthesesAndNullableSuppression(selectorArgument.Expression);
    if (!TryGetSelectorLambda(selectorExpression, out _, out _) ||
        TryGetDirectReadableScalarMemberName(
            context.SemanticModel,
            selectorExpression,
            context.CancellationToken,
            out _)) {
      return;
    }

    context.ReportDiagnostic(Diagnostic.Create(
        CodeFirstDiagnosticCatalog.UnsupportedSelector,
        selectorExpression.GetLocation(),
        verb));
  }

  private static void AnalyzeBuilderLambdaScope(SyntaxNodeAnalysisContext context) {
    var lambda = (LambdaExpressionSyntax)context.Node;
    if (!TryGetSingleLambdaParameter(lambda, out var parameter)) {
      return;
    }

    if (context.SemanticModel.GetDeclaredSymbol(parameter, context.CancellationToken) is not IParameterSymbol parameterSymbol ||
        parameterSymbol.Type is not INamedTypeSymbol parameterType) {
      return;
    }

    var scopeKind = GetBuilderScopeKind(parameterType);
    if (scopeKind == BuilderScopeKind.None) {
      return;
    }

    var declarations = new Dictionary<MemberDeclarationKey, InvocationExpressionSyntax>();
    foreach (var invocation in lambda.Body
        .DescendantNodesAndSelf()
        .OfType<InvocationExpressionSyntax>()
        .OrderBy(invocation => invocation.ArgumentList.Arguments.FirstOrDefault()?.SpanStart ?? invocation.SpanStart)) {
      if (!TryGetCodeFirstVerb(context.SemanticModel, invocation, context.CancellationToken, out var verb) ||
          !VerbBelongsToScope(scopeKind, verb) ||
          !TryGetInvocationRootReceiverSymbol(
              context.SemanticModel,
              invocation,
              context.CancellationToken,
              out var receiverSymbol) ||
          !SymbolEqualityComparer.Default.Equals(receiverSymbol, parameterSymbol)) {
        continue;
      }

      var selectorArgument = invocation.ArgumentList.Arguments.FirstOrDefault();
      if (selectorArgument is null ||
          !TryGetDirectReadableScalarMemberName(
              context.SemanticModel,
              selectorArgument.Expression,
              context.CancellationToken,
              out var memberName)) {
        continue;
      }

      var key = new MemberDeclarationKey(verb, memberName);
      if (declarations.ContainsKey(key)) {
        context.ReportDiagnostic(Diagnostic.Create(
            CodeFirstDiagnosticCatalog.DuplicateMember,
            selectorArgument.Expression.GetLocation(),
            verb,
            memberName));
        continue;
      }

      declarations.Add(key, invocation);
    }
  }

  private static bool TryGetCodeFirstVerb(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocation,
      CancellationToken cancellationToken,
      out string verb) {
    var operation = semanticModel.GetOperation(invocation, cancellationToken) as IInvocationOperation;
    return TryGetCodeFirstVerb(operation?.TargetMethod, out verb);
  }

  private static bool TryGetCodeFirstVerb(IMethodSymbol? methodSymbol, out string verb) {
    verb = string.Empty;
    if (methodSymbol is null) {
      return false;
    }

    var originalDefinition = methodSymbol.OriginalDefinition;
    var containingType = originalDefinition.ContainingType;
    if (containingType is null ||
        !string.Equals(containingType.ContainingNamespace.ToDisplayString(), NamespaceName, StringComparison.Ordinal)) {
      return false;
    }

    if (string.Equals(originalDefinition.Name, BusinessKeyVerb, StringComparison.Ordinal) &&
        string.Equals(containingType.Name, HubBuilderTypeName, StringComparison.Ordinal) &&
        containingType.Arity == 1) {
      verb = BusinessKeyVerb;
      return true;
    }

    if ((string.Equals(originalDefinition.Name, PayloadVerb, StringComparison.Ordinal) ||
        string.Equals(originalDefinition.Name, DrivingKeyVerb, StringComparison.Ordinal)) &&
        string.Equals(containingType.Name, SatelliteBuilderTypeName, StringComparison.Ordinal) &&
        containingType.Arity == 1) {
      verb = originalDefinition.Name;
      return true;
    }

    return false;
  }

  private static bool TryGetSingleLambdaParameter(
      LambdaExpressionSyntax lambda,
      out ParameterSyntax parameter) {
    switch (lambda) {
      case SimpleLambdaExpressionSyntax simpleLambda:
        parameter = simpleLambda.Parameter;
        return true;
      case ParenthesizedLambdaExpressionSyntax parenthesizedLambda when parenthesizedLambda.ParameterList.Parameters.Count == 1:
        parameter = parenthesizedLambda.ParameterList.Parameters[0];
        return true;
      default:
        parameter = null!;
        return false;
    }
  }

  private static bool TryGetDirectReadableScalarMemberName(
      SemanticModel semanticModel,
      ExpressionSyntax selectorExpression,
      CancellationToken cancellationToken,
      out string memberName) {
    memberName = string.Empty;
    selectorExpression = UnwrapParenthesesAndNullableSuppression(selectorExpression);

    if (!TryGetSelectorLambda(selectorExpression, out var parameter, out var bodyExpression)) {
      return false;
    }

    bodyExpression = UnwrapParenthesesAndNullableSuppression(bodyExpression);
    if (bodyExpression is not MemberAccessExpressionSyntax memberAccess) {
      return false;
    }

    if (semanticModel.GetDeclaredSymbol(parameter, cancellationToken) is not IParameterSymbol parameterSymbol) {
      return false;
    }

    var receiverExpression = UnwrapParenthesesAndNullableSuppression(memberAccess.Expression);
    var receiverSymbol = semanticModel.GetSymbolInfo(receiverExpression, cancellationToken).Symbol;
    if (!SymbolEqualityComparer.Default.Equals(receiverSymbol, parameterSymbol)) {
      return false;
    }

    var selectedSymbol = semanticModel.GetSymbolInfo(memberAccess, cancellationToken).Symbol;
    switch (selectedSymbol) {
      case IPropertySymbol propertySymbol
          when propertySymbol.GetMethod is not null &&
              !propertySymbol.IsIndexer &&
              propertySymbol.Parameters.Length == 0 &&
              IsReadableScalarType(propertySymbol.Type):
        memberName = propertySymbol.Name;
        return true;
      case IFieldSymbol fieldSymbol when IsReadableScalarType(fieldSymbol.Type):
        memberName = fieldSymbol.Name;
        return true;
      default:
        return false;
    }
  }

  private static bool TryGetSelectorLambda(
      ExpressionSyntax selectorExpression,
      out ParameterSyntax parameter,
      out ExpressionSyntax bodyExpression) {
    switch (selectorExpression) {
      case SimpleLambdaExpressionSyntax simpleLambda when simpleLambda.Body is ExpressionSyntax simpleBody:
        parameter = simpleLambda.Parameter;
        bodyExpression = simpleBody;
        return true;
      case ParenthesizedLambdaExpressionSyntax parenthesizedLambda
          when parenthesizedLambda.ParameterList.Parameters.Count == 1 &&
              parenthesizedLambda.Body is ExpressionSyntax parenthesizedBody:
        parameter = parenthesizedLambda.ParameterList.Parameters[0];
        bodyExpression = parenthesizedBody;
        return true;
      default:
        parameter = null!;
        bodyExpression = null!;
        return false;
    }
  }

  private static bool TryGetInvocationRootReceiverSymbol(
      SemanticModel semanticModel,
      InvocationExpressionSyntax invocation,
      CancellationToken cancellationToken,
      out ISymbol? receiverSymbol) {
    receiverSymbol = null;
    if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess) {
      return false;
    }

    var receiverExpression = UnwrapParenthesesAndNullableSuppression(memberAccess.Expression);
    while (receiverExpression is InvocationExpressionSyntax receiverInvocation &&
        receiverInvocation.Expression is MemberAccessExpressionSyntax receiverMemberAccess) {
      receiverExpression = UnwrapParenthesesAndNullableSuppression(receiverMemberAccess.Expression);
    }

    receiverSymbol = semanticModel.GetSymbolInfo(receiverExpression, cancellationToken).Symbol;
    return receiverSymbol is not null;
  }

  private static BuilderScopeKind GetBuilderScopeKind(INamedTypeSymbol typeSymbol) {
    if (!string.Equals(typeSymbol.ContainingNamespace.ToDisplayString(), NamespaceName, StringComparison.Ordinal)) {
      return BuilderScopeKind.None;
    }

    if (string.Equals(typeSymbol.Name, HubBuilderTypeName, StringComparison.Ordinal) &&
        typeSymbol.Arity == 1) {
      return BuilderScopeKind.Hub;
    }

    if (string.Equals(typeSymbol.Name, SatelliteBuilderTypeName, StringComparison.Ordinal) &&
        typeSymbol.Arity == 1) {
      return BuilderScopeKind.Satellite;
    }

    return BuilderScopeKind.None;
  }

  private static bool VerbBelongsToScope(BuilderScopeKind scopeKind, string verb) {
    return scopeKind switch {
      BuilderScopeKind.Hub => string.Equals(verb, BusinessKeyVerb, StringComparison.Ordinal),
      BuilderScopeKind.Satellite => string.Equals(verb, PayloadVerb, StringComparison.Ordinal) ||
          string.Equals(verb, DrivingKeyVerb, StringComparison.Ordinal),
      _ => false,
    };
  }

  private static ExpressionSyntax UnwrapParenthesesAndNullableSuppression(ExpressionSyntax expression) {
    while (true) {
      switch (expression) {
        case ParenthesizedExpressionSyntax parenthesizedExpression:
          expression = parenthesizedExpression.Expression;
          continue;
        case PostfixUnaryExpressionSyntax postfixUnaryExpression
            when postfixUnaryExpression.IsKind(SyntaxKind.SuppressNullableWarningExpression):
          expression = postfixUnaryExpression.Operand;
          continue;
        default:
          return expression;
      }
    }
  }

  private static bool IsReadableScalarType(ITypeSymbol typeSymbol) {
    typeSymbol = UnwrapNullable(typeSymbol);
    return typeSymbol.SpecialType == SpecialType.System_String || typeSymbol.IsValueType;
  }

  private static ITypeSymbol UnwrapNullable(ITypeSymbol typeSymbol) {
    return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
        namedTypeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T &&
        namedTypeSymbol.TypeArguments.Length == 1
        ? namedTypeSymbol.TypeArguments[0]
        : typeSymbol;
  }

  private readonly record struct MemberDeclarationKey(string Verb, string MemberName);

  private enum BuilderScopeKind {
    None,
    Hub,
    Satellite,
  }
}
