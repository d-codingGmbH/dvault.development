namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkHashKeyVariantRunContext(
    string Label,
    string StableHashAlgorithmId,
    int DigestByteLength,
    int HexCharacterLength,
    string StorageProfile,
    int HashKeyPayloadBytes) {
  public static BenchmarkHashKeyVariantRunContext FromVariant(BenchmarkHashKeyVariant variant) {
    ArgumentNullException.ThrowIfNull(variant);

    return new BenchmarkHashKeyVariantRunContext(
        variant.Label,
        variant.StableHashAlgorithmId,
        variant.DigestByteLength,
        variant.HexCharacterLength,
        variant.StorageProfile.ToString(),
        variant.HashKeyPayloadBytes);
  }
}
