using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkHashKeyVariant(
    string Label,
    string StableHashAlgorithmId,
    int DigestByteLength,
    DataVaultHashKeyStorageProfile StorageProfile) {
  public static BenchmarkHashKeyVariant Default { get; } = new(
      "sha256-v1-binary",
      "sha256-v1",
      32,
      DataVaultHashKeyStorageProfile.Binary);

  public static IReadOnlyList<BenchmarkHashKeyVariant> BoundedStorageMatrix { get; } =
  [
      Default,
      new(
          "sha256-v1-hex",
          "sha256-v1",
          32,
          DataVaultHashKeyStorageProfile.HexString),
      new(
          "sha256-128-v1-binary",
          "sha256-128-v1",
          16,
          DataVaultHashKeyStorageProfile.Binary),
      new(
          "sha256-128-v1-hex",
          "sha256-128-v1",
          16,
          DataVaultHashKeyStorageProfile.HexString),
  ];

  public int HexCharacterLength => DigestByteLength * 2;

  public int HashKeyPayloadBytes => StorageProfile == DataVaultHashKeyStorageProfile.Binary
      ? DigestByteLength
      : HexCharacterLength;

  public string StorageProfileToken => StorageProfile == DataVaultHashKeyStorageProfile.Binary
      ? "binary"
      : "hex";

  public string CreateDeterministicHashKey(string seed) {
    ArgumentException.ThrowIfNullOrWhiteSpace(seed);

    var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(Label + ":" + seed));
    return Convert
        .ToHexString(bytes)
        .ToLowerInvariant()[..HexCharacterLength];
  }

  public string CreateExecutionDetail() {
    return "hashKeyVariant=" + Label +
        "; stableHashAlgorithm=" + StableHashAlgorithmId +
        "; digestBytes=" + DigestByteLength.ToString(CultureInfo.InvariantCulture) +
        "; hashKeyStorage=" + StorageProfile +
        "; hashKeyPayloadBytes=" + HashKeyPayloadBytes.ToString(CultureInfo.InvariantCulture);
  }
}
