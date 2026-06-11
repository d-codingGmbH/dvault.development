namespace DCoding.Data.DVault;

internal readonly record struct DataVaultSatelliteProjectionValue {
  private DataVaultSatelliteProjectionValue(bool isMissing, object? value) {
    IsMissing = isMissing;
    Value = value;
  }

  public bool IsMissing { get; }

  public object? Value { get; }

  public static DataVaultSatelliteProjectionValue Missing() {
    return new DataVaultSatelliteProjectionValue(isMissing: true, value: null);
  }

  public static DataVaultSatelliteProjectionValue Present(object? value) {
    return new DataVaultSatelliteProjectionValue(isMissing: false, value);
  }
}
