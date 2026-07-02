using System.Linq.Expressions;

namespace DCoding.Data.DVault;

/// <summary>
/// Builds a fluent Code-First satellite declaration for one hub CLR entity type.
/// </summary>
/// <typeparam name="TEntity">The CLR entity type that owns the satellite.</typeparam>
public sealed class DataVaultCodeFirstSatelliteBuilder<TEntity> {
  private readonly DataVaultCodeFirstModelBuilder.SatelliteDeclaration _declaration;

  internal DataVaultCodeFirstSatelliteBuilder(DataVaultCodeFirstModelBuilder.SatelliteDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds one multi-active driving-key member to the satellite in declaration order.
  /// </summary>
  /// <typeparam name="TProperty">The selected member type.</typeparam>
  /// <param name="selector">A direct single-member selector, such as <c>x =&gt; x.ContactType</c>.</param>
  /// <returns>The same satellite builder so additional members can be configured fluently.</returns>
  public DataVaultCodeFirstSatelliteBuilder<TEntity> DrivingKey<TProperty>(
      Expression<Func<TEntity, TProperty>> selector) {
    _declaration.DrivingKeyNames.Add(DataVaultCodeFirstSelector.RequireNewMemberName(
        selector,
        "DrivingKey",
        _declaration.DrivingKeyNames));

    return this;
  }

  /// <summary>
  /// Adds one payload member to the satellite in declaration order.
  /// </summary>
  /// <typeparam name="TProperty">The selected member type.</typeparam>
  /// <param name="selector">A direct single-member selector, such as <c>x =&gt; x.EmailAddress</c>.</param>
  /// <returns>The same satellite builder so additional members can be configured fluently.</returns>
  public DataVaultCodeFirstSatelliteBuilder<TEntity> Payload<TProperty>(
      Expression<Func<TEntity, TProperty>> selector) {
    _declaration.PayloadNames.Add(DataVaultCodeFirstSelector.RequireNewMemberName(
        selector,
        "Payload",
        _declaration.PayloadNames));

    return this;
  }

  /// <summary>
  /// Marks one payload member as the lower effectivity boundary.
  /// </summary>
  /// <typeparam name="TProperty">The selected member type.</typeparam>
  /// <param name="selector">A direct single-member selector, such as <c>x =&gt; x.EffectiveFrom</c>.</param>
  /// <returns>The same satellite builder so additional members can be configured fluently.</returns>
  public DataVaultCodeFirstSatelliteBuilder<TEntity> EffectiveFrom<TProperty>(
      Expression<Func<TEntity, TProperty>> selector) {
    _declaration.EffectiveFromName = AddEffectivityPayload(selector, "EffectiveFrom");

    return this;
  }

  /// <summary>
  /// Marks one payload member as the optional upper effectivity boundary.
  /// </summary>
  /// <typeparam name="TProperty">The selected member type.</typeparam>
  /// <param name="selector">A direct single-member selector, such as <c>x =&gt; x.EffectiveTo</c>.</param>
  /// <returns>The same satellite builder so additional members can be configured fluently.</returns>
  public DataVaultCodeFirstSatelliteBuilder<TEntity> EffectiveTo<TProperty>(
      Expression<Func<TEntity, TProperty>> selector) {
    _declaration.EffectiveToName = AddEffectivityPayload(selector, "EffectiveTo");

    return this;
  }

  /// <summary>
  /// Marks one payload member as an optional current-row marker or status value.
  /// </summary>
  /// <typeparam name="TProperty">The selected member type.</typeparam>
  /// <param name="selector">A direct single-member selector, such as <c>x =&gt; x.IsCurrent</c>.</param>
  /// <returns>The same satellite builder so additional members can be configured fluently.</returns>
  public DataVaultCodeFirstSatelliteBuilder<TEntity> CurrentFlag<TProperty>(
      Expression<Func<TEntity, TProperty>> selector) {
    _declaration.CurrentFlagName = AddEffectivityPayload(selector, "CurrentFlag");

    return this;
  }

  private string AddEffectivityPayload<TProperty>(
      Expression<Func<TEntity, TProperty>> selector,
      string verb) {
    var memberName = DataVaultCodeFirstSelector.RequireMemberName(selector, verb);
    if (_declaration.DrivingKeyNames.Contains(memberName, StringComparer.Ordinal)) {
      throw new ArgumentException(
          verb + " member '" + memberName + "' is already declared as a driving key.",
          nameof(selector));
    }

    if (!_declaration.PayloadNames.Contains(memberName, StringComparer.Ordinal)) {
      _declaration.PayloadNames.Add(memberName);
    }

    return memberName;
  }
}
