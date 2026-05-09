using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace DCoding.Data.DVault;

internal static class DataVaultCodeFirstSelector {
  public static string RequireNewMemberName<TEntity, TProperty>(
      Expression<Func<TEntity, TProperty>> selector,
      string verb,
      IEnumerable<string> existingMemberNames) {
    ArgumentNullException.ThrowIfNull(selector);
    ArgumentNullException.ThrowIfNull(existingMemberNames);

    if (selector.Body is MemberExpression memberExpression &&
        memberExpression.Expression is ParameterExpression parameterExpression &&
        ReferenceEquals(parameterExpression, selector.Parameters[0]) &&
        memberExpression.Member is PropertyInfo or FieldInfo) {
      var memberName = memberExpression.Member.Name;
      if (IsCollectionMember(memberExpression.Member)) {
        throw new ArgumentException(
            verb +
            " selector must target a direct readable scalar member on the configured entity type, such as 'x => x.Member'. " +
            "Use repeated " +
            verb +
            "(x => x.Member) calls for each scalar member instead of collection, anonymous-object, computed, nested, or method-call selectors.",
            nameof(selector));
      }

      if (existingMemberNames.Contains(memberName, StringComparer.Ordinal)) {
        throw new ArgumentException(
            verb +
            " member '" +
            memberName +
            "' is already declared. Use each logical member name at most once for " +
            verb +
            "(...) declarations.",
            nameof(selector));
      }

      return memberName;
    }

    throw new ArgumentException(
        verb +
        " selector must target a direct readable scalar member on the configured entity type, such as 'x => x.Member'. " +
        "Use repeated " +
        verb +
        "(x => x.Member) calls for each scalar member instead of collection, anonymous-object, computed, nested, or method-call selectors.",
        nameof(selector));
  }

  private static bool IsCollectionMember(MemberInfo member) {
    var memberType = member switch {
      PropertyInfo propertyInfo => propertyInfo.PropertyType,
      FieldInfo fieldInfo => fieldInfo.FieldType,
      _ => throw new ArgumentOutOfRangeException(nameof(member), member, "Unsupported selector member kind."),
    };

    return memberType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(memberType);
  }
}
