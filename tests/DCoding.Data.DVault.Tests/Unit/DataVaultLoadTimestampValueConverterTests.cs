using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultLoadTimestampValueConverterTests {
  [Fact]
  public void EfModelValueKeepsIsoTextTimestampAsDateTimeOffsetWhenClrPropertyIsDateTimeOffset() {
    var timestamp = new DateTimeOffset(2026, 5, 8, 10, 30, 45, TimeSpan.FromHours(2));

    var value = DataVaultLoadTimestampValueConverter.ToProviderValue(
        DataVaultProviderValueFormat.Iso8601UtcText,
        typeof(DateTimeOffset),
        timestamp);

    Assert.Equal(timestamp.ToUniversalTime(), Assert.IsType<DateTimeOffset>(value));
  }

  [Fact]
  public void RawProviderParameterFormatsIsoTextTimestampAsText() {
    var timestamp = new DateTimeOffset(2026, 5, 8, 10, 30, 45, TimeSpan.FromHours(2));

    var value = DataVaultLoadTimestampValueConverter.ToProviderParameterValue(
        DataVaultProviderValueFormat.Iso8601UtcText,
        timestamp);

    Assert.Equal("2026-05-08T08:30:45.0000000+00:00", Assert.IsType<string>(value));
  }

  [Fact]
  public void UtcTicksTimestampStorageRoundTripsThroughProviderValueReader() {
    var timestamp = new DateTimeOffset(2026, 5, 8, 10, 30, 45, TimeSpan.FromHours(2));

    var value = DataVaultLoadTimestampValueConverter.ToProviderParameterValue(
        DataVaultProviderValueFormat.UtcTicks,
        timestamp);
    var roundTrippedTimestamp = DataVaultLoadTimestampValueConverter.ReadProviderValue(value);

    Assert.Equal(timestamp.ToUniversalTime(), roundTrippedTimestamp);
  }
}
