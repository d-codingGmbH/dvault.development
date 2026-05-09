using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace DCoding.Data.DVault;

internal static class DataVaultCodeFirstMemberSelector {
  public static string GetDirectScalarMemberName<T, TMember>(
      Expression<Func<T, TMember>> selector,
      string apiName,
      string parameterName) {
    ArgumentNullException.ThrowIfNull(selector, parameterName);

    var body = UnwrapConvert(selector.Body);
    if (body is not MemberExpression memberExpression ||
        UnwrapConvert(memberExpression.Expression) != selector.Parameters.Single()) {
      throw UnsupportedSelector(apiName, parameterName);
    }

    if (!TryGetReadableMemberType(memberExpression.Member, out var memberType) ||
        IsUnsupportedMemberType(memberType)) {
      throw UnsupportedSelector(apiName, parameterName);
    }

    return memberExpression.Member.Name;
  }

  private static Expression? UnwrapConvert(Expression? expression) {
    while (expression is UnaryExpression unaryExpression &&
        unaryExpression.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked) {
      expression = unaryExpression.Operand;
    }

    return expression;
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

  private static ArgumentException UnsupportedSelector(string apiName, string parameterName) {
    return new ArgumentException(
        apiName +
        " supports only a direct readable scalar member selector such as 'x => x.CustomerId'. Use repeated single-member calls for composite keys.",
        parameterName);
  }
}
