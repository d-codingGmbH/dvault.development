namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Builds a satellite declaration.
/// </summary>
public sealed class DataVaultSatelliteBuilder {
  private readonly DataVaultModelBuilder.SatelliteDeclaration _declaration;

  internal DataVaultSatelliteBuilder(DataVaultModelBuilder.SatelliteDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds a payload property to the satellite.
  /// </summary>
  public DataVaultSatelliteBuilder Payload(string propertyName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
    _declaration.PayloadProperties.Add(propertyName);

    return this;
  }

  /// <summary>
  /// Adds a multi-active driving-key property to the satellite.
  /// </summary>
  public DataVaultSatelliteBuilder DrivingKey(string propertyName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
    _declaration.DrivingKeyProperties.Add(propertyName);

    return this;
  }
}
