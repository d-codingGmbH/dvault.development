using System.Linq.Expressions;

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
    var memberName = DataVaultCodeFirstMemberSelector.GetDirectScalarMemberName(
        propertySelector,
        "BusinessKey",
        nameof(propertySelector));

    if (_declaration.BusinessKeyNames.Contains(memberName, StringComparer.Ordinal)) {
      throw new ArgumentException(
          "Code-first hub '" +
          _declaration.Name +
          "' declares business-key member '" +
          memberName +
          "' more than once.",
          nameof(propertySelector));
    }

    _declaration.BusinessKeyNames.Add(memberName);

    return this;
  }
}
