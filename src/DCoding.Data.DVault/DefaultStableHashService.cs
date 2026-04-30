using System.Security.Cryptography;
using System.Text;

namespace DCoding.Data.DVault;

internal sealed class DefaultStableHashService : IStableHashService
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    public static DefaultStableHashService Instance { get; } = new();

    public string AlgorithmId => "sha256-v1";

    public StableHashDigest ComputeHash(string normalizedInput)
    {
        ArgumentNullException.ThrowIfNull(normalizedInput);

        var inputBytes = Utf8NoBom.GetBytes(normalizedInput);
        var digestBytes = SHA256.HashData(inputBytes);
        var digestValue = Convert.ToHexString(digestBytes).ToLowerInvariant();

        return new StableHashDigest(AlgorithmId, digestValue);
    }
}
