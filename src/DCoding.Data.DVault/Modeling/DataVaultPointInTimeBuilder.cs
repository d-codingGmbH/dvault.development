namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Builds a point-in-time table declaration.
/// </summary>
public sealed class DataVaultPointInTimeBuilder {
  private readonly DataVaultModelBuilder.PointInTimeDeclaration _declaration;

  internal DataVaultPointInTimeBuilder(DataVaultModelBuilder.PointInTimeDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds a satellite reference to the point-in-time table in declaration order.
  /// </summary>
  public DataVaultPointInTimeBuilder Satellite(string satelliteName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);
    _declaration.SatelliteNames.Add(satelliteName);

    return this;
  }
}
