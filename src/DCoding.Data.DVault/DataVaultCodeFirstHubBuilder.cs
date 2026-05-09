using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace DCoding.Data.DVault;

/// <summary>
/// Builds a code-first Data Vault hub declaration for one CLR entity type.
/// </summary>
/// <typeparam name="TEntity">The CLR entity type represented by the hub.</typeparam>
public sealed class DataVaultCodeFirstHubBuilder<TEntity>
    where TEntity : class {
  private readonly DataVaultCodeFirstModelBuilder.HubDeclaration _declaration;

  internal DataVaultCodeFirstHubBuilder(DataVaultCodeFirstModelBuilder.HubDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds one business-key member to the hub in declaration order.
  /// </summary>
  /// <typeparam name="TProperty">The selected member value type.</typeparam>
  /// <param name="propertySelector">A direct readable scalar member selector rooted at the entity parameter.</param>
  /// <returns>The same hub builder so additional business-key members can be configured fluently.</returns>
  public DataVaultCodeFirstHubBuilder<TEntity> BusinessKey<TProperty>(
      Expression<Func<TEntity, TProperty>> propertySelector) {
    ArgumentNullException.ThrowIfNull(propertySelector);

    var body = UnwrapConvert(propertySelector.Body);
    if (!TryGetDirectReadableScalarMemberName(body, propertySelector.Parameters[0], out var memberName)) {
      throw CreateUnsupportedBusinessKeySelectorException(body, propertySelector.Parameters[0]);
    }

    if (_declaration.BusinessKeyNames.Contains(memberName, StringComparer.Ordinal)) {
      throw new ArgumentException(
          "BusinessKey member '" +
          memberName +
          "' is already declared. Use each logical member name at most once for BusinessKey(...) declarations.",
          "selector");
    }

    _declaration.BusinessKeyNames.Add(memberName);

    return this;
  }

  /// <summary>
  /// Adds a hub-parent satellite declaration with an explicit satellite name.
  /// </summary>
  /// <param name="satelliteName">The provider-neutral satellite name.</param>
  /// <param name="configure">The optional satellite configuration callback.</param>
  /// <returns>The same hub builder so additional satellites can be configured fluently.</returns>
  public DataVaultCodeFirstHubBuilder<TEntity> Satellite(
      string satelliteName,
      Action<DataVaultCodeFirstSatelliteBuilder<TEntity>>? configure = null) {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);

    var declaration = new DataVaultCodeFirstModelBuilder.SatelliteDeclaration(satelliteName);
    _declaration.Satellites.Add(declaration);

    var builder = new DataVaultCodeFirstSatelliteBuilder<TEntity>(declaration);
    configure?.Invoke(builder);

    return this;
  }

  private static Expression? UnwrapConvert(Expression? expression) {
    while (expression is UnaryExpression unaryExpression &&
        unaryExpression.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked) {
      expression = unaryExpression.Operand;
    }

    return expression;
  }

  private static bool TryGetDirectReadableScalarMemberName(
      Expression? expression,
      ParameterExpression parameter,
      out string memberName) {
    if (expression is MemberExpression memberExpression &&
        ReferenceEquals(UnwrapConvert(memberExpression.Expression), parameter) &&
        TryGetReadableMemberType(memberExpression.Member, out var memberType) &&
        !IsUnsupportedMemberType(memberType)) {
      memberName = memberExpression.Member.Name;
      return true;
    }

    memberName = string.Empty;
    return false;
  }

  private static bool TryGetReadableMemberType(MemberInfo memberInfo, out Type memberType) {
    switch (memberInfo) {
      case PropertyInfo propertyInfo:
        memberType = propertyInfo.PropertyType;
        return propertyInfo.GetMethod is not null && propertyInfo.GetIndexParameters().Length == 0;
      case FieldInfo fieldInfo:
        memberType = fieldInfo.FieldType;
        return true;
      default:
        memberType = typeof(object);
        return false;
    }
  }

  private static bool IsUnsupportedMemberType(Type memberType) {
    memberType = Nullable.GetUnderlyingType(memberType) ?? memberType;

    if (memberType == typeof(string)) {
      return false;
    }

    if (typeof(IEnumerable).IsAssignableFrom(memberType)) {
      return true;
    }

    return !memberType.IsValueType;
  }

  private static ArgumentException CreateUnsupportedBusinessKeySelectorException(
      Expression? expression,
      ParameterExpression parameter) {
    if (expression is MemberExpression memberExpression &&
        !ReferenceEquals(UnwrapConvert(memberExpression.Expression), parameter)) {
      return new ArgumentException(
          "BusinessKey supports only a direct readable scalar member selector such as 'x => x.CustomerId'. Use repeated single-member calls for composite keys.",
          "propertySelector");
    }

    return new ArgumentException(
        "BusinessKey selector must target a direct readable scalar member on the configured entity type, such as 'x => x.Member'. " +
        "Use repeated BusinessKey(x => x.Member) calls for each scalar member instead of collection, anonymous-object, computed, nested, or method-call selectors.",
        "selector");
  }
}
