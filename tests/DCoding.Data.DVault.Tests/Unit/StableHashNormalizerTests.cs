using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class StableHashNormalizerTests
{
    [Fact]
    public void ScalarNormalizationUsesCanonicalTypeTags()
    {
        var normalizer = CreateDefaultNormalizer();

        Assert.Equal("n:", normalizer.NormalizeValue(null));
        Assert.Equal("s:0:", normalizer.NormalizeValue(""));
        Assert.Equal("b:true", normalizer.NormalizeValue(true));
        Assert.Equal("b:false", normalizer.NormalizeValue(false));
        Assert.Equal("i:-42", normalizer.NormalizeValue(-42));
        Assert.Equal("d:1234.50", normalizer.NormalizeValue(1234.50m));
        Assert.Equal("t:2026-04-28T00:00:00.0000000Z", normalizer.NormalizeValue(
            new DateTime(2026, 4, 28, 0, 0, 0, DateTimeKind.Utc)));
        Assert.Equal("g:00112233-4455-6677-8899-aabbccddeeff", normalizer.NormalizeValue(
            new Guid("00112233-4455-6677-8899-AABBCCDDEEFF")));
    }

    [Fact]
    public void StringNormalizationUsesNfcTextLfLineEndingsAndUtf8ByteCounts()
    {
        var normalizer = CreateDefaultNormalizer();

        Assert.Equal("s:2:\u00e9", normalizer.NormalizeValue("\u0065\u0301"));
        Assert.Equal("s:5:a\nb\nc", normalizer.NormalizeValue("a\r\nb\rc"));
    }

    [Fact]
    public void StructuredNormalizationIncludesNullsAndSortsFieldsOrdinally()
    {
        var normalizer = CreateDefaultNormalizer();

        var normalized = normalizer.NormalizeFields(
            [
                new KeyValuePair<string, object?>("score", 42),
                new KeyValuePair<string, object?>("nickname", null),
                new KeyValuePair<string, object?>("name", "Alice"),
                new KeyValuePair<string, object?>("active", true),
            ]);

        Assert.Equal("active=b:true\nname=s:5:Alice\nnickname=n:\nscore=i:42", normalized);
    }

    [Fact]
    public void StructuredNormalizationRejectsDuplicateFieldPaths()
    {
        var normalizer = CreateDefaultNormalizer();

        var exception = Assert.Throws<ArgumentException>(() => normalizer.NormalizeFields(
            [
                new KeyValuePair<string, object?>("name", "Alice"),
                new KeyValuePair<string, object?>("name", "Bob"),
            ]));

        Assert.Equal("fields", exception.ParamName);
        Assert.Contains("duplicate", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("name=value")]
    [InlineData("name\nvalue")]
    [InlineData("name\rvalue")]
    public void StructuredNormalizationRejectsInvalidFieldPaths(string? fieldPath)
    {
        var normalizer = CreateDefaultNormalizer();

        var exception = Assert.ThrowsAny<ArgumentException>(() => normalizer.NormalizeFields(
            [new KeyValuePair<string, object?>(fieldPath!, "value")]));

        Assert.Contains("field", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void StructuredNormalizationIsIndependentOfSourceFieldOrder()
    {
        var normalizer = CreateDefaultNormalizer();
        var firstFields = new Dictionary<string, object?>
        {
            ["timestamp"] = new DateTime(2026, 4, 28, 0, 0, 0, DateTimeKind.Utc),
            ["amount"] = 1234.50m,
        };
        var secondFields = new Dictionary<string, object?>
        {
            ["amount"] = 1234.50m,
            ["timestamp"] = new DateTime(2026, 4, 28, 0, 0, 0, DateTimeKind.Utc),
        };

        var firstNormalized = normalizer.NormalizeFields(firstFields);
        var secondNormalized = normalizer.NormalizeFields(secondFields);
        var hashService = CreateDefaultHashService();

        Assert.Equal("amount=d:1234.50\ntimestamp=t:2026-04-28T00:00:00.0000000Z", firstNormalized);
        Assert.Equal(firstNormalized, secondNormalized);
        Assert.Equal(hashService.ComputeHash(firstNormalized).Value, hashService.ComputeHash(secondNormalized).Value);
    }

    [Fact]
    public void CultureChangesDoNotAffectNormalizedTextOrDigest()
    {
        var normalizer = CreateDefaultNormalizer();
        var hashService = CreateDefaultHashService();
        var fields = new[]
        {
            new KeyValuePair<string, object?>("amount", 1234.50m),
            new KeyValuePair<string, object?>("count", 1234),
            new KeyValuePair<string, object?>("name", "\u0065\u0301"),
            new KeyValuePair<string, object?>(
                "timestamp",
                new DateTimeOffset(2026, 4, 28, 2, 0, 0, TimeSpan.FromHours(2))),
        };
        var invariantText = normalizer.NormalizeFields(fields);
        var invariantDigest = hashService.ComputeHash(invariantText);

        using (new CultureSwap("de-DE"))
        {
            var cultureText = normalizer.NormalizeFields(fields);
            var cultureDigest = hashService.ComputeHash(cultureText);

            Assert.Equal(
                "amount=d:1234.50\ncount=i:1234\nname=s:2:\u00e9\ntimestamp=t:2026-04-28T00:00:00.0000000Z",
                cultureText);
            Assert.Equal(invariantText, cultureText);
            Assert.Equal(invariantDigest.Value, cultureDigest.Value);
            Assert.Equal(invariantDigest.AlgorithmId, cultureDigest.AlgorithmId);
        }
    }

    [Fact]
    public void UnsupportedValuesFailBeforeHashing()
    {
        var normalizer = CreateDefaultNormalizer();
        var hashService = new CountingHashService();

        var exception = Assert.Throws<NotSupportedException>(() => NormalizeAndHash(
            normalizer,
            hashService,
            [new KeyValuePair<string, object?>("binary", new byte[] { 1, 2, 3 })]));

        Assert.Contains("binary", exception.Message, StringComparison.Ordinal);
        Assert.Equal(0, hashService.CallCount);
    }

    [Fact]
    public void UnsupportedScalarValuesIdentifyTheValueType()
    {
        var normalizer = CreateDefaultNormalizer();

        var exception = Assert.Throws<NotSupportedException>(() => normalizer.NormalizeValue(new byte[] { 1, 2, 3 }));

        Assert.Contains("System.Byte[]", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void InvalidSupportedValuesFailBeforeHashing()
    {
        var normalizer = CreateDefaultNormalizer();
        var hashService = new CountingHashService();

        var exception = Assert.Throws<ArgumentException>(() => NormalizeAndHash(
            normalizer,
            hashService,
            [
                new KeyValuePair<string, object?>(
                    "timestamp",
                    new DateTime(2026, 4, 28, 0, 0, 0, DateTimeKind.Unspecified)),
            ]));

        Assert.Contains("timestamp", exception.Message, StringComparison.Ordinal);
        Assert.Equal(0, hashService.CallCount);
    }

    [Fact]
    public void InvalidStringValuesFailBeforeHashing()
    {
        var normalizer = CreateDefaultNormalizer();
        var hashService = new CountingHashService();

        var exception = Assert.Throws<ArgumentException>(() => NormalizeAndHash(
            normalizer,
            hashService,
            [new KeyValuePair<string, object?>("name", "\ud800")]));

        Assert.Contains("name", exception.Message, StringComparison.Ordinal);
        Assert.Equal(0, hashService.CallCount);
    }

    [Fact]
    public void AddDVaultPreservesCallerStableHashNormalizerOverride()
    {
        var replacement = new ReplacementStableHashNormalizer();
        var services = new ServiceCollection();
        services.AddSingleton<IStableHashNormalizer>(replacement);

        services.AddDVault();

        using var provider = services.BuildServiceProvider(validateScopes: true);
        var resolved = provider.GetRequiredService<IStableHashNormalizer>();

        Assert.Same(replacement, resolved);
        Assert.Equal("replacement", resolved.NormalizeValue("ignored"));
    }

    private static IStableHashNormalizer CreateDefaultNormalizer()
    {
        var services = new ServiceCollection();
        services.AddDVault();

        return services.BuildServiceProvider(validateScopes: true).GetRequiredService<IStableHashNormalizer>();
    }

    private static IStableHashService CreateDefaultHashService()
    {
        var services = new ServiceCollection();
        services.AddDVault();

        return services.BuildServiceProvider(validateScopes: true).GetRequiredService<IStableHashService>();
    }

    private static StableHashDigest NormalizeAndHash(
        IStableHashNormalizer normalizer,
        IStableHashService hashService,
        IEnumerable<KeyValuePair<string, object?>> fields)
    {
        return hashService.ComputeHash(normalizer.NormalizeFields(fields));
    }

    private sealed class CountingHashService : IStableHashService
    {
        public int CallCount { get; private set; }

        public string AlgorithmId => "counting-v1";

        public StableHashDigest ComputeHash(string normalizedInput)
        {
            CallCount++;

            return new StableHashDigest(
                AlgorithmId,
                "0000000000000000000000000000000000000000000000000000000000000000");
        }
    }

    private sealed class ReplacementStableHashNormalizer : IStableHashNormalizer
    {
        public string NormalizeValue(object? value)
        {
            return "replacement";
        }

        public string NormalizeFields(IEnumerable<KeyValuePair<string, object?>> fields)
        {
            return "replacement";
        }
    }

    private sealed class CultureSwap : IDisposable
    {
        private readonly CultureInfo originalCulture;
        private readonly CultureInfo originalUiCulture;

        public CultureSwap(string cultureName)
        {
            originalCulture = CultureInfo.CurrentCulture;
            originalUiCulture = CultureInfo.CurrentUICulture;

            var culture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }
}
