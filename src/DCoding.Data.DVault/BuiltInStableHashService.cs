using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed class BuiltInStableHashService : IStableHashService {
  private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

  public static IStableHashService Sha256 { get; } = new BuiltInStableHashService(
      "sha256-v1",
      StableHashAlgorithmKind.Sha256,
      digestByteLength: 32);

  public static IStableHashService Sha1 { get; } = new BuiltInStableHashService(
      "sha1-v1",
      StableHashAlgorithmKind.Sha1,
      digestByteLength: 20);

  public static IStableHashService Sha256128 { get; } = new BuiltInStableHashService(
      "sha256-128-v1",
      StableHashAlgorithmKind.Sha256,
      digestByteLength: 16);

  public static IStableHashService Sha256160 { get; } = new BuiltInStableHashService(
      "sha256-160-v1",
      StableHashAlgorithmKind.Sha256,
      digestByteLength: 20);

  private readonly StableHashAlgorithmKind _algorithmKind;
  private readonly int _digestByteLength;

  private BuiltInStableHashService(
      string algorithmId,
      StableHashAlgorithmKind algorithmKind,
      int digestByteLength) {
    AlgorithmId = algorithmId;
    _algorithmKind = algorithmKind;
    _digestByteLength = digestByteLength;
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

    var byteCount = Utf8NoBom.GetByteCount(normalizedInput);
    var inputBytes = ArrayPool<byte>.Shared.Rent(byteCount);
    try {
      var writtenBytes = Utf8NoBom.GetBytes(normalizedInput.AsSpan(), inputBytes);
      Span<byte> digestBytes = stackalloc byte[32];
      ComputeDigest(inputBytes.AsSpan(0, writtenBytes), digestBytes);
      var digestValue = CreateLowerHexString(digestBytes[.._digestByteLength]);

      return new StableHashDigest(AlgorithmId, digestValue);
    }
    finally {
      ArrayPool<byte>.Shared.Return(inputBytes);
    }
  }

  private void ComputeDigest(ReadOnlySpan<byte> inputBytes, Span<byte> digestBytes) {
    _ = _algorithmKind switch {
      StableHashAlgorithmKind.Sha1 => SHA1.HashData(inputBytes, digestBytes),
      _ => SHA256.HashData(inputBytes, digestBytes),
    };
  }

  private static string CreateLowerHexString(ReadOnlySpan<byte> bytes) {
    const string LowerHexDigits = "0123456789abcdef";

    Span<char> chars = stackalloc char[bytes.Length * 2];
    for (var index = 0; index < bytes.Length; index++) {
      var value = bytes[index];
      chars[index * 2] = LowerHexDigits[value >> 4];
      chars[(index * 2) + 1] = LowerHexDigits[value & 0x0f];
    }

    return new string(chars);
  }

  private enum StableHashAlgorithmKind {
    Sha1,
    Sha256,
  }
}
