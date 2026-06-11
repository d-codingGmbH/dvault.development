namespace DCoding.Data.DVault;

internal static class DataVaultBridgeProjectionFailures {
  public const string MissingName = "missing-name";
  public const string NullValue = "null-value";
  public const string InvalidValue = "invalid-value";

  public static InvalidOperationException Create(
      string failureKind,
      string metadataName,
      string mappedName,
      string detail) {
    return new InvalidOperationException(
        "DVault typed bridge projection failed (" +
        failureKind +
        "): bridge metadata '" +
        metadataName +
        "' mapped name '" +
        mappedName +
        "' " +
        detail +
        ".");
  }
}
