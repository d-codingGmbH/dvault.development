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

    if (values.Count == 0) {
      return rows.Where(_ => false);
    }

    var row = Expression.Parameter(typeof(Dictionary<string, object>), "row");
    var property = Expression.Call(
        typeof(EF),
        nameof(EF.Property),
        [typeof(string)],
        row,
        Expression.Constant(propertyName));
    Expression? predicate = null;

    foreach (var value in values) {
      var equals = Expression.Equal(property, Expression.Constant(value, typeof(string)));
      predicate = predicate is null ? equals : Expression.OrElse(predicate, equals);
    }

    return rows.Where(Expression.Lambda<Func<Dictionary<string, object>, bool>>(predicate!, row));
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
