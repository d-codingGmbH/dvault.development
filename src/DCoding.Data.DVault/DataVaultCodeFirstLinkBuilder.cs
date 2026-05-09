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
    _declaration.ParticipantClrTypes.Add(typeof(TEntity));

    return this;
  }
}
