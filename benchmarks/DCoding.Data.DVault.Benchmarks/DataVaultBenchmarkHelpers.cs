using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault.Benchmarks;

internal static class DataVaultBenchmarkHelpers {
  public static string GetHashKey(DataVaultSaveResult result, DataVaultTableKind kind, string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && string.Equals(record.MetadataName, metadataName, StringComparison.Ordinal))
        .HashKey;
  }

  public static bool IsLowercaseSha256(string value) {
    return value.Length == 64 && value.All(static character =>
        character is >= '0' and <= '9' or >= 'a' and <= 'f');
  }
}
