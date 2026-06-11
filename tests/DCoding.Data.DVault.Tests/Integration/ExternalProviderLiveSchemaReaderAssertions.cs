using DCoding.Data.DVault.Tests.Shared;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class ExternalProviderLiveSchemaReaderAssertions {
  public static async Task AssertReadAsyncMatchesExpectedSnapshotAsync(
      Func<Task<ExternalProviderLiveSchemaFixture>> createFixtureAsync) {
    await using var fixture = await createFixtureAsync().ConfigureAwait(false);
    await using var context = fixture.CreateContext();

    var readResult = await DataVaultLiveSchemaReader.ReadAsync(context).ConfigureAwait(false);

    Assert.Equal(DataVaultLiveSchemaReadStatus.Succeeded, readResult.Status);
    Assert.NotNull(readResult.Snapshot);
    Assert.Equal(
        LiveSchemaReaderContractFixture.CreateSnapshotSignatures(fixture.ExpectedSnapshot),
        LiveSchemaReaderContractFixture.CreateSnapshotSignatures(readResult.Snapshot));
  }
}
