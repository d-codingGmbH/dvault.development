using System.Linq.Expressions;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultSharedTypeQueryFiltersTests {
  [Fact]
  public void WhereStringPropertyEqualsAnyBuildsBalancedPredicateForLargeBatches() {
    var values = Enumerable.Range(0, 500)
        .Select(index => "hash-" + index.ToString("000"))
        .Concat(["hash-000"])
        .ToArray();
    var rows = Array.Empty<Dictionary<string, object>>().AsQueryable();

    var query = rows.WhereStringPropertyEqualsAny("HashKey", values);
    var predicate = ExtractWherePredicate(query.Expression);

    Assert.Equal(500, CountNodes(predicate.Body, ExpressionType.Equal));
    Assert.InRange(MeasureOrElseDepth(predicate.Body), 1, 10);
  }

  private static Expression<Func<Dictionary<string, object>, bool>> ExtractWherePredicate(Expression expression) {
    var call = Assert.IsAssignableFrom<MethodCallExpression>(expression);
    Assert.Equal("Where", call.Method.Name);
    var quote = Assert.IsType<UnaryExpression>(call.Arguments[1]);

    return Assert.IsAssignableFrom<Expression<Func<Dictionary<string, object>, bool>>>(quote.Operand);
  }

  private static int CountNodes(Expression expression, ExpressionType nodeType) {
    if (expression is not BinaryExpression binary) {
      return expression.NodeType == nodeType ? 1 : 0;
    }

    return (expression.NodeType == nodeType ? 1 : 0) +
        CountNodes(binary.Left, nodeType) +
        CountNodes(binary.Right, nodeType);
  }

  private static int MeasureOrElseDepth(Expression expression) {
    if (expression is not BinaryExpression binary ||
        expression.NodeType != ExpressionType.OrElse) {
      return 0;
    }

    return 1 + Math.Max(
        MeasureOrElseDepth(binary.Left),
        MeasureOrElseDepth(binary.Right));
  }
}
