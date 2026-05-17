namespace DCoding.Data.DVault;

/// <summary>
/// Builds a code-first Data Vault link declaration from ordered participant CLR entity types.
/// </summary>
public sealed class DataVaultCodeFirstLinkBuilder {
  private readonly DataVaultCodeFirstModelBuilder.LinkDeclaration _declaration;

  internal DataVaultCodeFirstLinkBuilder(DataVaultCodeFirstModelBuilder.LinkDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds one participating hub CLR type to the link in declaration order.
  /// </summary>
  /// <typeparam name="TEntity">The CLR entity type for a previously configured hub participant.</typeparam>
  /// <returns>The same link builder so additional participants can be configured fluently.</returns>
  public DataVaultCodeFirstLinkBuilder Participant<TEntity>()
      where TEntity : class {
    _declaration.Participants.Add(new DataVaultCodeFirstModelBuilder.ParticipantDeclaration(typeof(TEntity), role: null));

    return this;
  }

  /// <summary>
  /// Adds one participating hub CLR type to the link in declaration order with an explicit participant role.
  /// </summary>
  /// <typeparam name="TEntity">The CLR entity type for a previously configured hub participant.</typeparam>
  /// <param name="role">The provider-neutral participant role used as the produced participant name.</param>
  /// <returns>The same link builder so additional participants can be configured fluently.</returns>
  public DataVaultCodeFirstLinkBuilder Participant<TEntity>(string role)
      where TEntity : class {
    ArgumentException.ThrowIfNullOrWhiteSpace(role);

    _declaration.Participants.Add(new DataVaultCodeFirstModelBuilder.ParticipantDeclaration(typeof(TEntity), role));

    return this;
  }

  /// <summary>
  /// Adds a link-parent satellite declaration with an explicit satellite name.
  /// </summary>
  /// <typeparam name="TSatellite">The CLR type used by the satellite configuration selectors.</typeparam>
  /// <param name="satelliteName">The provider-neutral satellite name.</param>
  /// <param name="configure">The optional satellite configuration callback.</param>
  /// <returns>The same link builder so additional participants or satellites can be configured fluently.</returns>
  public DataVaultCodeFirstLinkBuilder Satellite<TSatellite>(
      string satelliteName,
      Action<DataVaultCodeFirstSatelliteBuilder<TSatellite>>? configure = null)
      where TSatellite : class {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);

    var declaration = new DataVaultCodeFirstModelBuilder.SatelliteDeclaration(satelliteName);
    _declaration.Satellites.Add(declaration);

    var builder = new DataVaultCodeFirstSatelliteBuilder<TSatellite>(declaration);
    configure?.Invoke(builder);

    return this;
  }
}
