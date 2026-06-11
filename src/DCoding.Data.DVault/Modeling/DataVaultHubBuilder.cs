namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Builds a hub declaration.
/// </summary>
public sealed class DataVaultHubBuilder {
  private readonly DataVaultModelBuilder.HubDeclaration _declaration;

  internal DataVaultHubBuilder(DataVaultModelBuilder.HubDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds a business-key property to the hub.
  /// </summary>
  public DataVaultHubBuilder BusinessKey(string propertyName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
    _declaration.BusinessKeyProperties.Add(propertyName);

    return this;
  }

  /// <summary>
  /// Adds a satellite declaration to the hub.
  /// </summary>
  public DataVaultHubBuilder Satellite(string satelliteName, Action<DataVaultSatelliteBuilder>? configure = null) {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);

    var declaration = new DataVaultModelBuilder.SatelliteDeclaration(satelliteName);
    _declaration.Satellites.Add(declaration);

    var builder = new DataVaultSatelliteBuilder(declaration);
    configure?.Invoke(builder);

    return this;
  }
}
