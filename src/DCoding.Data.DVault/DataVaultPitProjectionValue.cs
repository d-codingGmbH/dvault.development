namespace DCoding.Data.DVault;

internal readonly record struct DataVaultPitProjectionValue {
  private DataVaultPitProjectionValue(bool isMissing, object? value) {
    IsMissing = isMissing;
    Value = value;
  }

  public bool IsMissing { get; }

  public object? Value { get; }

  public static DataVaultPitProjectionValue Missing() {
    return new DataVaultPitProjectionValue(isMissing: true, value: null);
  }

  public static DataVaultPitProjectionValue Present(object? value) {
    return new DataVaultPitProjectionValue(isMissing: false, value);
  }
}
