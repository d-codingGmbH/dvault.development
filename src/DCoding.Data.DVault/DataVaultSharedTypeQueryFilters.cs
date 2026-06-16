using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultSharedTypeQueryFilters {
  public static IQueryable<Dictionary<string, object>> WhereStringPropertyEqualsAny(
      this IQueryable<Dictionary<string, object>> rows,
      string propertyName,
      IReadOnlyCollection<string> values) {
    ArgumentNullException.ThrowIfNull(rows);
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
    ArgumentNullException.ThrowIfNull(values);

    var distinctValues = values
        .Distinct(StringComparer.Ordinal)
        .ToArray();
    if (distinctValues.Length == 0) {
      return rows.Where(_ => false);
    }

    var row = Expression.Parameter(typeof(Dictionary<string, object>), "row");
    var property = Expression.Call(
        typeof(EF),
        nameof(EF.Property),
        [typeof(string)],
        row,
        Expression.Constant(propertyName));
    var predicate = CreateBalancedStringEqualsAnyExpression(
        property,
        distinctValues,
        startIndex: 0,
        distinctValues.Length);

    return rows.Where(Expression.Lambda<Func<Dictionary<string, object>, bool>>(predicate, row));
  }

  private static Expression CreateBalancedStringEqualsAnyExpression(
      Expression property,
      IReadOnlyList<string> values,
      int startIndex,
      int length) {
    if (length <= 0) {
      throw new ArgumentOutOfRangeException(nameof(length));
    }

    if (length == 1) {
      return Expression.Equal(
          property,
          Expression.Constant(values[startIndex], typeof(string)));
    }

    var leftLength = length / 2;
    var rightLength = length - leftLength;

    return Expression.OrElse(
        CreateBalancedStringEqualsAnyExpression(property, values, startIndex, leftLength),
        CreateBalancedStringEqualsAnyExpression(property, values, startIndex + leftLength, rightLength));
  }

  public static IQueryable<Dictionary<string, object>> WhereIntPropertyLessThanOrEqual(
      this IQueryable<Dictionary<string, object>> rows,
      string propertyName,
      int value) {
    ArgumentNullException.ThrowIfNull(rows);
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

    var row = Expression.Parameter(typeof(Dictionary<string, object>), "row");
    var property = Expression.Call(
        typeof(EF),
        nameof(EF.Property),
        [typeof(int)],
        row,
        Expression.Constant(propertyName));
    var predicate = Expression.LessThanOrEqual(property, Expression.Constant(value));

    return rows.Where(Expression.Lambda<Func<Dictionary<string, object>, bool>>(predicate, row));
  }
}
