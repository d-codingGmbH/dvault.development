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
}
