using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace DCoding.Data.DVault.Analyzers;

/// <summary>
/// Provides bounded mechanical code fixes for DVault Code-First analyzer diagnostics.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DataVaultCodeFirstCodeFixProvider))]
[Shared]
internal sealed class DataVaultCodeFirstCodeFixProvider : CodeFixProvider {
  private const string BusinessKeyVerb = "BusinessKey";
  private const string PayloadVerb = "Payload";
  private const string DrivingKeyVerb = "DrivingKey";
  private const string ExpandAnonymousSelectorTitle = "Expand selector into repeated direct-member calls";
  private const string RemoveDuplicateDeclarationTitle = "Remove duplicate Code-First member declaration";

  /// <inheritdoc />
  public override ImmutableArray<string> FixableDiagnosticIds { get; } =
  [
      CodeFirstDiagnosticCatalog.UnsupportedSelectorMetadata.Id,
      CodeFirstDiagnosticCatalog.DuplicateMemberMetadata.Id,
  ];

  /// <inheritdoc />
  public override FixAllProvider? GetFixAllProvider() {
    return null;
  }

  /// <inheritdoc />
  public override async Task RegisterCodeFixesAsync(CodeFixContext context) {
    var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
    if (root is null) {
      return;
    }

    foreach (var diagnostic in context.Diagnostics) {
      if (string.Equals(
          diagnostic.Id,
          CodeFirstDiagnosticCatalog.UnsupportedSelectorMetadata.Id,
          StringComparison.Ordinal)) {
        await RegisterUnsupportedSelectorFixAsync(context, root, diagnostic).ConfigureAwait(false);
        continue;
      }

      if (string.Equals(
          diagnostic.Id,
          CodeFirstDiagnosticCatalog.DuplicateMemberMetadata.Id,
          StringComparison.Ordinal)) {
        RegisterDuplicateMemberFix(context, root, diagnostic);
      }
    }
  }

  private static async Task RegisterUnsupportedSelectorFixAsync(
      CodeFixContext context,
      SyntaxNode root,
      Diagnostic diagnostic) {
    if (!TryGetDiagnosticLambda(root, diagnostic, out var lambda) ||
        !TryGetSelectorInvocation(lambda, out var invocation) ||
        !TryGetSupportedVerb(invocation, out _)) {
      return;
    }

    var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken)
        .ConfigureAwait(false);
    if (semanticModel is null ||
        !TryGetExpandableAnonymousObjectMembers(
            semanticModel,
            lambda,
            context.CancellationToken,
            out var memberExpressions)) {
      return;
    }

    context.RegisterCodeFix(
        CodeAction.Create(
            ExpandAnonymousSelectorTitle,
            cancellationToken => ExpandAnonymousSelectorAsync(
                context.Document,
                invocation,
                lambda,
                memberExpressions,
                cancellationToken),
            ExpandAnonymousSelectorTitle),
        diagnostic);
  }

  private static void RegisterDuplicateMemberFix(
      CodeFixContext context,
      SyntaxNode root,
      Diagnostic diagnostic) {
    if (!TryGetDiagnosticInvocation(root, diagnostic, out var invocation) ||
        !TryGetSupportedVerb(invocation, out _)) {
      return;
    }

    context.RegisterCodeFix(
        CodeAction.Create(
            RemoveDuplicateDeclarationTitle,
            cancellationToken => RemoveDuplicateInvocationAsync(
                context.Document,
                invocation,
                cancellationToken),
            RemoveDuplicateDeclarationTitle),
        diagnostic);
  }

  private static async Task<Document> ExpandAnonymousSelectorAsync(
      Document document,
      InvocationExpressionSyntax invocation,
      LambdaExpressionSyntax lambda,
      ImmutableArray<MemberAccessExpressionSyntax> memberExpressions,
      CancellationToken cancellationToken) {
    var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
    if (root is null ||
        invocation.Expression is not MemberAccessExpressionSyntax memberAccess ||
        !TryGetSupportedVerb(invocation, out var verb)) {
      return document;
    }

    ExpressionSyntax expandedExpression = memberAccess.Expression.WithoutTrivia();
    foreach (var memberExpression in memberExpressions) {
      var selector = CreateSingleMemberSelector(lambda, memberExpression);
      expandedExpression = SyntaxFactory.InvocationExpression(
          SyntaxFactory.MemberAccessExpression(
              SyntaxKind.SimpleMemberAccessExpression,
              expandedExpression,
              SyntaxFactory.IdentifierName(verb)),
          SyntaxFactory.ArgumentList(
              SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(selector))));
    }

    var replacement = expandedExpression
        .WithTriviaFrom(invocation)
        .WithAdditionalAnnotations(Formatter.Annotation);
    var newRoot = root.ReplaceNode(invocation, replacement);
    var newDocument = document.WithSyntaxRoot(newRoot);

    return await Formatter.FormatAsync(newDocument, Formatter.Annotation, cancellationToken: cancellationToken)
        .ConfigureAwait(false);
  }

  private static async Task<Document> RemoveDuplicateInvocationAsync(
      Document document,
      InvocationExpressionSyntax invocation,
      CancellationToken cancellationToken) {
    var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
    if (root is null) {
      return document;
    }

    var newRoot = TryGetChainedReceiver(invocation, out var chainedReceiver)
        ? root.ReplaceNode(
            invocation,
            chainedReceiver
                .WithTriviaFrom(invocation)
                .WithAdditionalAnnotations(Formatter.Annotation))
        : invocation.Parent is ExpressionStatementSyntax statement
            ? root.RemoveNode(statement, SyntaxRemoveOptions.KeepExteriorTrivia)
            : root;
    if (newRoot is null || ReferenceEquals(newRoot, root)) {
      return document;
    }

    var newDocument = document.WithSyntaxRoot(newRoot);
    return await Formatter.FormatAsync(newDocument, Formatter.Annotation, cancellationToken: cancellationToken)
        .ConfigureAwait(false);
  }

  private static bool TryGetDiagnosticLambda(
      SyntaxNode root,
      Diagnostic diagnostic,
      out LambdaExpressionSyntax lambda) {
    var node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
    lambda = node.AncestorsAndSelf().OfType<LambdaExpressionSyntax>().FirstOrDefault()!;

    return lambda is not null;
  }

  private static bool TryGetDiagnosticInvocation(
      SyntaxNode root,
      Diagnostic diagnostic,
      out InvocationExpressionSyntax invocation) {
    var node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
    invocation = node.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().FirstOrDefault()!;

    return invocation is not null;
  }

  private static bool TryGetSelectorInvocation(
      LambdaExpressionSyntax lambda,
      out InvocationExpressionSyntax invocation) {
    foreach (var candidate in lambda.Ancestors().OfType<InvocationExpressionSyntax>()) {
      var selectorArgument = candidate.ArgumentList.Arguments.FirstOrDefault();
      if (selectorArgument is not null &&
          UnwrapParenthesesAndNullableSuppression(selectorArgument.Expression) == lambda) {
        invocation = candidate;
        return true;
      }
    }

    invocation = null!;
    return false;
  }

  private static bool TryGetExpandableAnonymousObjectMembers(
      SemanticModel semanticModel,
      LambdaExpressionSyntax lambda,
      CancellationToken cancellationToken,
      out ImmutableArray<MemberAccessExpressionSyntax> memberExpressions) {
    memberExpressions = [];

    if (!TryGetSingleLambdaParameter(lambda, out var parameter) ||
        semanticModel.GetDeclaredSymbol(parameter, cancellationToken) is not IParameterSymbol parameterSymbol ||
        lambda.Body is not ExpressionSyntax bodyExpression) {
      return false;
    }

    bodyExpression = UnwrapParenthesesAndNullableSuppression(bodyExpression);
    if (bodyExpression is not AnonymousObjectCreationExpressionSyntax anonymousObject ||
        anonymousObject.Initializers.Count == 0) {
      return false;
    }

    var builder = ImmutableArray.CreateBuilder<MemberAccessExpressionSyntax>(anonymousObject.Initializers.Count);
    foreach (var initializer in anonymousObject.Initializers) {
      var initializerExpression = UnwrapParenthesesAndNullableSuppression(initializer.Expression);
      if (initializerExpression is not MemberAccessExpressionSyntax memberAccess ||
          !IsDirectReadableScalarMember(
              semanticModel,
              memberAccess,
              parameterSymbol,
              cancellationToken)) {
        return false;
      }

      builder.Add((MemberAccessExpressionSyntax)memberAccess.WithoutTrivia());
    }

    memberExpressions = builder.ToImmutable();
    return true;
  }

  private static bool IsDirectReadableScalarMember(
      SemanticModel semanticModel,
      MemberAccessExpressionSyntax memberAccess,
      IParameterSymbol parameterSymbol,
      CancellationToken cancellationToken) {
    var receiverExpression = UnwrapParenthesesAndNullableSuppression(memberAccess.Expression);
    var receiverSymbol = semanticModel.GetSymbolInfo(receiverExpression, cancellationToken).Symbol;
    if (!SymbolEqualityComparer.Default.Equals(receiverSymbol, parameterSymbol)) {
      return false;
    }

    var selectedSymbol = semanticModel.GetSymbolInfo(memberAccess, cancellationToken).Symbol;
    return selectedSymbol switch {
      IPropertySymbol propertySymbol => propertySymbol.GetMethod is not null &&
          !propertySymbol.IsIndexer &&
          propertySymbol.Parameters.Length == 0 &&
          IsReadableScalarType(propertySymbol.Type),
      IFieldSymbol fieldSymbol => IsReadableScalarType(fieldSymbol.Type),
      _ => false,
    };
  }

  private static LambdaExpressionSyntax CreateSingleMemberSelector(
      LambdaExpressionSyntax sourceLambda,
      MemberAccessExpressionSyntax memberExpression) {
    var selectorBody = memberExpression.WithoutTrivia();
    return sourceLambda switch {
      SimpleLambdaExpressionSyntax simpleLambda => SyntaxFactory.SimpleLambdaExpression(
          simpleLambda.Parameter.WithoutTrivia(),
          selectorBody),
      ParenthesizedLambdaExpressionSyntax parenthesizedLambda => SyntaxFactory.ParenthesizedLambdaExpression(
          parenthesizedLambda.ParameterList.WithoutTrivia(),
          selectorBody),
      _ => throw new ArgumentOutOfRangeException(nameof(sourceLambda), sourceLambda, "Unsupported lambda syntax."),
    };
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

  private static bool TryGetSupportedVerb(InvocationExpressionSyntax invocation, out string verb) {
    verb = string.Empty;
    if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess) {
      return false;
    }

    verb = memberAccess.Name.Identifier.ValueText;
    return string.Equals(verb, BusinessKeyVerb, StringComparison.Ordinal) ||
        string.Equals(verb, PayloadVerb, StringComparison.Ordinal) ||
        string.Equals(verb, DrivingKeyVerb, StringComparison.Ordinal);
  }

  private static bool TryGetChainedReceiver(
      InvocationExpressionSyntax invocation,
      out ExpressionSyntax receiver) {
    receiver = null!;
    if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess) {
      return false;
    }

    var unwrappedReceiver = UnwrapParenthesesAndNullableSuppression(memberAccess.Expression);
    if (unwrappedReceiver is not InvocationExpressionSyntax) {
      return false;
    }

    receiver = unwrappedReceiver.WithoutTrivia();
    return true;
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
}
