namespace DCoding.Data.DVault;

internal readonly record struct DataVaultBridgeProjectionValue {
  private DataVaultBridgeProjectionValue(bool isMissing, object? value) {
    IsMissing = isMissing;
    Value = value;
  }

  public bool IsMissing { get; }

  public object? Value { get; }

  public static DataVaultBridgeProjectionValue Missing() {
    return new DataVaultBridgeProjectionValue(isMissing: true, value: null);
  }

  public static DataVaultBridgeProjectionValue Present(object? value) {
    return new DataVaultBridgeProjectionValue(isMissing: false, value);
  }
}
