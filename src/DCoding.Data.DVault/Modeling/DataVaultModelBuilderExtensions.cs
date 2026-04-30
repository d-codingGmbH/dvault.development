namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Provides model configuration extensions for DVault conventions.
/// </summary>
public static class DataVaultModelBuilderExtensions {
  /// <summary>
  /// Enables provider-neutral Data Vault modeling conventions using the optionless v1 defaults.
  /// This convention-first entry point requires no custom naming, hashing, provider, or model options; advanced configuration remains optional.
  /// </summary>
  /// <param name="modelBuilder">The model builder to configure.</param>
  /// <returns>The same model builder so model configuration can continue fluently.</returns>
  public static DataVaultModelBuilder UseDataVault(this DataVaultModelBuilder modelBuilder) {
    ArgumentNullException.ThrowIfNull(modelBuilder);

    modelBuilder.UseConventions(DataVaultConventions.Default);

    return modelBuilder;
  }
}
