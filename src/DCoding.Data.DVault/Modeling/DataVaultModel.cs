namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Represents Data Vault names produced by the modeling flow.
/// </summary>
public sealed class DataVaultModel {
  private DataVaultModel(IReadOnlyList<DataVaultTable> tables) {
    Tables = tables;
  }

  /// <summary>
  /// Gets the tables produced for the model.
  /// </summary>
  public IReadOnlyList<DataVaultTable> Tables { get; }

  /// <summary>
  /// Builds a Data Vault model using optional model convention options.
  /// </summary>
  public static DataVaultModel Create(
      Action<DataVaultModelBuilder> configureModel,
      Action<DataVaultModelOptions>? configureOptions = null) {
    ArgumentNullException.ThrowIfNull(configureModel);

    var options = new DataVaultModelOptions();
    configureOptions?.Invoke(options);

    var builder = new DataVaultModelBuilder(options);
    configureModel(builder);

    return builder.Build();
  }

  internal static DataVaultModel FromTables(IReadOnlyList<DataVaultTable> tables) {
    return new DataVaultModel(tables);
  }
}
