using System.Security.Cryptography;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class StableHashServiceTests {
  [Theory]
  [InlineData("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
  [InlineData("s:0:", "68531113e40fffcea6caa4b72302c47015bb82b9e9ff2ceb9f2c6953e5f9a2b0")]
  [InlineData("n:", "1f8dc03d51e3ddcc59b608508bba5c34aecac15f1b250390a629a4231ad80a9a")]
  [InlineData("s:21:dvault:stable-hash:v1", "eb99c3da5f4b0e5f6137357a0134b1d8d92133d1137ebe0606daae281a6a4281")]
  [InlineData(
      "active=b:true\nname=s:5:Alice\nnickname=n:\nscore=i:42",
      "d2fb098dce221d02fc6561aabacfe9418c331fd576b3518c01e70cf6ba7ea115")]
  [InlineData(
      "amount=d:1234.50\ntimestamp=t:2026-04-28T00:00:00.0000000Z",
      "1a84b2aacf8d30fe82e26bf2c21e2948a9ebf43780e6667718191c5ef8abb83a")]
  public void DefaultServiceProducesPublishedSha256Vectors(string normalizedInput, string expectedDigest) {
    var service = CreateDefaultService();

    var digest = service.ComputeHash(normalizedInput);

    Assert.Equal("sha256-v1", service.AlgorithmId);
    Assert.Equal(service.AlgorithmId, digest.AlgorithmId);
    Assert.Equal(expectedDigest, digest.Value);
    Assert.Equal(32, digest.DigestByteLength);
    Assert.Matches("^[0-9a-f]{64}$", digest.Value);
  }

  [Fact]
  public void DefaultServiceRejectsNullButAcceptsEmptyInput() {
    var service = CreateDefaultService();

    var emptyDigest = service.ComputeHash("");
    var exception = Assert.Throws<ArgumentNullException>(() => service.ComputeHash(null!));

    Assert.Equal("normalizedInput", exception.ParamName);
    Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", emptyDigest.Value);
  }

  [Fact]
  public void DefaultServiceIsDeterministicAcrossRepeatedHashing() {
    var service = CreateDefaultService();
    const string normalizedInput = "s:21:dvault:stable-hash:v1";

    var firstDigest = service.ComputeHash(normalizedInput);
    var secondDigest = service.ComputeHash(normalizedInput);

    Assert.Equal(firstDigest.AlgorithmId, secondDigest.AlgorithmId);
    Assert.Equal(firstDigest.Value, secondDigest.Value);
  }

  [Fact]
  public void DefaultServiceHashesUtf8BytesWithoutByteOrderMark() {
    var service = CreateDefaultService();
    const string normalizedInput = "s:2:\u00e9";

    var digest = service.ComputeHash(normalizedInput);

    var utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
    var expectedDigest = Hex(SHA256.HashData(utf8NoBom.GetBytes(normalizedInput)));
    var utf8WithBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true);
    var bomPrefixedInput = utf8WithBom.GetPreamble().Concat(utf8WithBom.GetBytes(normalizedInput)).ToArray();
    var bomDigest = Hex(SHA256.HashData(bomPrefixedInput));

    Assert.Equal(expectedDigest, digest.Value);
    Assert.NotEqual(bomDigest, digest.Value);
  }

  [Fact]
  public void AddDVaultProvidesDefaultStableHashServices() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var hashService = provider.GetRequiredService<IStableHashService>();
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();

    Assert.Equal("sha256-v1", hashService.AlgorithmId);
    Assert.Equal("sha256-v1", conventions.StableHashAlgorithmId);
    Assert.Equal(32, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    Assert.Equal("default", conventions.ProfileName);
    Assert.Equal("sha-256", conventions.PersistenceContentHashAlgorithm);
    Assert.Equal("n:", normalizer.NormalizeValue(null));
  }

  [Fact]
  public void AddDVaultCanSelectBinaryFirstProfileWithoutChangingLogicalHashBoundary() {
    var services = new ServiceCollection();

    services.AddDVault(options => options.UseBinaryFirstProfile());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var hashService = provider.GetRequiredService<IStableHashService>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();
    var digest = hashService.ComputeHash("s:21:dvault:stable-hash:v1");

    Assert.Equal("sha256-v1", hashService.AlgorithmId);
    Assert.Equal("sha256-v1", conventions.StableHashAlgorithmId);
    Assert.Equal(32, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    Assert.Equal("binary-first", conventions.ProfileName);
    Assert.Equal("eb99c3da5f4b0e5f6137357a0134b1d8d92133d1137ebe0606daae281a6a4281", digest.Value);
    Assert.Matches("^[0-9a-f]{64}$", digest.Value);
  }

  [Fact]
  public void AddDVaultCanSelectHexStringStorageProfileForExistingSchemas() {
    var services = new ServiceCollection();

    services.AddDVault(options => options.UseHexStringStorageProfile());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var hashService = provider.GetRequiredService<IStableHashService>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();
    var digest = hashService.ComputeHash("s:21:dvault:stable-hash:v1");

    Assert.Equal("sha256-v1", hashService.AlgorithmId);
    Assert.Equal("sha256-v1", conventions.StableHashAlgorithmId);
    Assert.Equal(32, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.HexString, conventions.HashKeyStorageProfile);
    Assert.Equal("hex-string-compatibility", conventions.ProfileName);
    Assert.Equal("eb99c3da5f4b0e5f6137357a0134b1d8d92133d1137ebe0606daae281a6a4281", digest.Value);
    Assert.Matches("^[0-9a-f]{64}$", digest.Value);
  }

  [Fact]
  public void AddDVaultBinaryFirstProfilePreservesSelectedStableHashAlgorithm() {
    var services = new ServiceCollection();

    services.AddDVault(options => options
        .UseStableHashAlgorithm("sha1-v1")
        .UseBinaryFirstProfile());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var hashService = provider.GetRequiredService<IStableHashService>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();

    Assert.Equal("sha1-v1", hashService.AlgorithmId);
    Assert.Equal("sha1-v1", conventions.StableHashAlgorithmId);
    Assert.Equal(20, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    Assert.Equal("binary-first", conventions.ProfileName);
  }

  [Fact]
  public void AddDVaultBinaryFirstProfilePreservesLaterSelectedStableHashAlgorithm() {
    var services = new ServiceCollection();

    services.AddDVault(options => options
        .UseBinaryFirstProfile()
        .UseStableHashAlgorithm("sha1-v1"));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var hashService = provider.GetRequiredService<IStableHashService>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();

    Assert.Equal("sha1-v1", hashService.AlgorithmId);
    Assert.Equal("sha1-v1", conventions.StableHashAlgorithmId);
    Assert.Equal(20, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    Assert.Equal("binary-first", conventions.ProfileName);
  }

  [Theory]
  [InlineData(
      "sha256-v1",
      64,
      32,
      "eb99c3da5f4b0e5f6137357a0134b1d8d92133d1137ebe0606daae281a6a4281")]
  [InlineData(
      "sha1-v1",
      40,
      20,
      "1fae773f805277eaf33fc5f96d6fc4a7f7e1d84d")]
  [InlineData(
      "sha256-128-v1",
      32,
      16,
      "eb99c3da5f4b0e5f6137357a0134b1d8")]
  [InlineData(
      "sha256-160-v1",
      40,
      20,
      "eb99c3da5f4b0e5f6137357a0134b1d8d92133d1")]
  public void AddDVaultCanSelectBuiltInStableHashAlgorithm(
      string algorithmId,
      int hexLength,
      int byteLength,
      string expectedDigest) {
    var services = new ServiceCollection();

    services.AddDVault(options => options.UseStableHashAlgorithm(algorithmId));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var hashService = provider.GetRequiredService<IStableHashService>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();
    var digest = hashService.ComputeHash("s:21:dvault:stable-hash:v1");

    Assert.Equal(algorithmId, hashService.AlgorithmId);
    Assert.Equal(algorithmId, digest.AlgorithmId);
    Assert.Equal(expectedDigest, digest.Value);
    Assert.Equal(byteLength, digest.DigestByteLength);
    Assert.Matches("^[0-9a-f]{" + hexLength + "}$", digest.Value);
    Assert.Equal(algorithmId, conventions.StableHashAlgorithmId);
    Assert.Equal(byteLength, conventions.StableHashDigestByteLength);
    Assert.Equal(DataVaultHashKeyStorageProfile.Binary, conventions.HashKeyStorageProfile);
    Assert.Equal("default", conventions.ProfileName);
    Assert.Equal("sha-256", conventions.PersistenceContentHashAlgorithm);
  }

  [Theory]
  [InlineData("sha256-128-v1", 32)]
  [InlineData("sha256-160-v1", 40)]
  public void TruncatedSha256BuiltInsUseLeadingSha256DigestCharacters(string algorithmId, int hexLength) {
    var sha256Service = CreateSelectedService("sha256-v1");
    var truncatedService = CreateSelectedService(algorithmId);

    var sha256Digest = sha256Service.ComputeHash("s:21:dvault:stable-hash:v1");
    var truncatedDigest = truncatedService.ComputeHash("s:21:dvault:stable-hash:v1");

    Assert.Equal(sha256Digest.Value[..hexLength], truncatedDigest.Value);
  }

  [Theory]
  [InlineData("sha1-v1")]
  [InlineData("sha256-128-v1")]
  [InlineData("sha256-160-v1")]
  public void AddDVaultDoesNotEnableNonDefaultBuiltInAlgorithmsWithoutSelection(string algorithmId) {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var hashServices = provider.GetServices<IStableHashService>().ToArray();
    var conventions = provider.GetRequiredService<DataVaultConventions>();

    var hashService = Assert.Single(hashServices);
    Assert.Equal("sha256-v1", hashService.AlgorithmId);
    Assert.NotEqual(algorithmId, hashService.AlgorithmId);
    Assert.Equal("sha256-v1", conventions.StableHashAlgorithmId);
  }

  [Fact]
  public void AddDVaultPreservesCallerStableHashServiceOverride() {
    var replacement = new ReplacementStableHashService();
    var services = new ServiceCollection();
    services.AddSingleton<IStableHashService>(replacement);

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var resolved = provider.GetRequiredService<IStableHashService>();
    var digest = resolved.ComputeHash("s:21:dvault:stable-hash:v1");

    Assert.Same(replacement, resolved);
    Assert.Equal("test-double-v1", digest.AlgorithmId);
    Assert.Equal("00000000000000000000000000000001", digest.Value);
    Assert.Equal(16, digest.DigestByteLength);
  }

  [Fact]
  public void ExplicitBuiltInSelectionReplacesCallerStableHashServiceOverride() {
    var replacement = new ReplacementStableHashService();
    var services = new ServiceCollection();
    services.AddSingleton<IStableHashService>(replacement);

    services.AddDVault(options => options.UseStableHashAlgorithm("sha1-v1"));

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var resolved = provider.GetRequiredService<IStableHashService>();
    var conventions = provider.GetRequiredService<DataVaultConventions>();
    var digest = resolved.ComputeHash("s:21:dvault:stable-hash:v1");

    Assert.NotSame(replacement, resolved);
    Assert.Equal("sha1-v1", resolved.AlgorithmId);
    Assert.Equal("sha1-v1", digest.AlgorithmId);
    Assert.Equal("1fae773f805277eaf33fc5f96d6fc4a7f7e1d84d", digest.Value);
    Assert.Equal("sha1-v1", conventions.StableHashAlgorithmId);
  }

  [Theory]
  [InlineData("")]
  [InlineData(" ")]
  [InlineData("SHA256-V1")]
  [InlineData("sha256")]
  [InlineData("sha256-v1 ")]
  [InlineData(" sha256-v1")]
  [InlineData("test-double-v1")]
  public void UseStableHashAlgorithmRejectsUnsupportedAlgorithmIds(string algorithmId) {
    var options = new DataVaultOptions();

    var exception = Assert.Throws<ArgumentException>(() => options.UseStableHashAlgorithm(algorithmId));

    Assert.Equal("algorithmId", exception.ParamName);
  }

  [Fact]
  public void UseStableHashAlgorithmRejectsNullAlgorithmId() {
    var options = new DataVaultOptions();

    var exception = Assert.Throws<ArgumentNullException>(() => options.UseStableHashAlgorithm(null!));

    Assert.Equal("algorithmId", exception.ParamName);
  }

  [Theory]
  [InlineData("sha1-v1", 40, 20)]
  [InlineData("sha256-128-v1", 32, 16)]
  [InlineData("sha256-160-v1", 40, 20)]
  public void StableHashDigestAcceptsKnownAlgorithmDigestLengths(
      string algorithmId,
      int hexLength,
      int byteLength) {
    var value = new string('0', hexLength);

    var digest = new StableHashDigest(algorithmId, value);

    Assert.Equal(algorithmId, digest.AlgorithmId);
    Assert.Equal(value, digest.Value);
    Assert.Equal(byteLength, digest.DigestByteLength);
  }

  [Theory]
  [InlineData("test-double-v1", "0123456789abcdef", 8)]
  [InlineData("custom-truncated-v1", "abcdef012345", 6)]
  public void StableHashDigestAcceptsCustomAlgorithmWholeByteLowerHex(
      string algorithmId,
      string value,
      int byteLength) {
    var digest = new StableHashDigest(algorithmId, value);

    Assert.Equal(algorithmId, digest.AlgorithmId);
    Assert.Equal(value, digest.Value);
    Assert.Equal(byteLength, digest.DigestByteLength);
  }

  [Theory]
  [InlineData("sha256-v1", 40)]
  [InlineData("sha1-v1", 64)]
  [InlineData("sha256-128-v1", 40)]
  [InlineData("sha256-160-v1", 64)]
  public void StableHashDigestRejectsKnownAlgorithmsWithWrongDigestLengths(
      string algorithmId,
      int hexLength) {
    var value = new string('0', hexLength);

    var exception = Assert.Throws<ArgumentException>(() => new StableHashDigest(algorithmId, value));

    Assert.Equal("value", exception.ParamName);
  }

  [Theory]
  [InlineData("")]
  [InlineData("abc")]
  [InlineData("000000000000000000000000000000000000000000000000000000000000000G")]
  [InlineData("000000000000000000000000000000000000000000000000000000000000000A")]
  public void StableHashDigestRejectsValuesOutsideCanonicalLowerHexByteShape(string value) {
    var exception = Assert.Throws<ArgumentException>(() => new StableHashDigest("sha256-v1", value));

    Assert.Equal("value", exception.ParamName);
  }

  private static IStableHashService CreateDefaultService() {
    var services = new ServiceCollection();
    services.AddDVault();

    return services.BuildServiceProvider(validateScopes: true).GetRequiredService<IStableHashService>();
  }

  private static IStableHashService CreateSelectedService(string algorithmId) {
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseStableHashAlgorithm(algorithmId));

    return services.BuildServiceProvider(validateScopes: true).GetRequiredService<IStableHashService>();
  }

  private static string Hex(byte[] value) {
    return Convert.ToHexString(value).ToLowerInvariant();
  }

  private sealed class ReplacementStableHashService : IStableHashService {
    public string AlgorithmId => "test-double-v1";

    public StableHashDigest ComputeHash(string normalizedInput) {
      ArgumentNullException.ThrowIfNull(normalizedInput);

      return new StableHashDigest(
          AlgorithmId,
          "00000000000000000000000000000001");
    }
  }
}
