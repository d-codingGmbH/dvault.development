using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed class BuiltInStableHashService : IStableHashService {
  private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

  public static IStableHashService Sha256 { get; } = new BuiltInStableHashService(
      "sha256-v1",
      SHA256.HashData);

  public static IStableHashService Sha1 { get; } = new BuiltInStableHashService(
      "sha1-v1",
      SHA1.HashData);

  public static IStableHashService Sha256128 { get; } = new BuiltInStableHashService(
      "sha256-128-v1",
      inputBytes => ComputeTruncatedSha256(inputBytes, 16));

  public static IStableHashService Sha256160 { get; } = new BuiltInStableHashService(
      "sha256-160-v1",
      inputBytes => ComputeTruncatedSha256(inputBytes, 20));

  private readonly Func<byte[], byte[]> _computeDigestBytes;

  private BuiltInStableHashService(string algorithmId, Func<byte[], byte[]> computeDigestBytes) {
    AlgorithmId = algorithmId;
    _computeDigestBytes = computeDigestBytes;
  }

  public string AlgorithmId { get; }

  public static IStableHashService Create(string algorithmId) {
    ArgumentNullException.ThrowIfNull(algorithmId);

    return algorithmId switch {
      "sha256-v1" => Sha256,
      "sha1-v1" => Sha1,
      "sha256-128-v1" => Sha256128,
      "sha256-160-v1" => Sha256160,
      _ => throw new ArgumentException(
          "The stable hash algorithm id must be one of the built-in ids: sha256-v1, sha1-v1, sha256-128-v1, or sha256-160-v1.",
          nameof(algorithmId)),
    };
  }

  public StableHashDigest ComputeHash(string normalizedInput) {
    return DataVaultAllocationProfiler.Measure(
        "digest generation",
        "BuiltInStableHashService.ComputeHash",
        () => ComputeHashCore(normalizedInput));
  }

  private StableHashDigest ComputeHashCore(string normalizedInput) {
    ArgumentNullException.ThrowIfNull(normalizedInput);

    var inputBytes = Utf8NoBom.GetBytes(normalizedInput);
    var digestBytes = _computeDigestBytes(inputBytes);
    var digestValue = Convert.ToHexString(digestBytes).ToLowerInvariant();

    return new StableHashDigest(AlgorithmId, digestValue);
  }

  private static byte[] ComputeTruncatedSha256(byte[] inputBytes, int byteLength) {
    return SHA256.HashData(inputBytes)[..byteLength];
  }
}
