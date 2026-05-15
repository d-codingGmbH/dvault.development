using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class LiveSchemaReaderContractOutcomeTests {
  [Fact]
  public async Task CompareAsyncUsesExplicitReaderSuccessContractWithoutDrift() {
    await using var context = new ContractContext(new DbContextOptionsBuilder<ContractContext>().Options);
    var reader = new StubLiveSchemaReader(
        DataVaultLiveSchemaReadResult.Success(
            "Microsoft.EntityFrameworkCore.Sqlite",
            LiveSchemaReaderContractFixture.CreateExpectedSnapshot(DataVaultProviderCapabilityProfiles.Sqlite)));

    var report = await DataVaultLiveSchemaDriftReporter.CompareAsync(
        LiveSchemaReaderContractFixture.CreateCanonicalMetadataModel(),
        context,
        reader);

    Assert.False(report.HasBlockingDifferences);
    Assert.Empty(report.Differences);
    Assert.Equal("DVault model drift: no differences.", report.ToDisplayString());
  }

  [Fact]
  public async Task CompareAsyncClassifiesUnsupportedReaderResultWithoutThrowing() {
    await using var context = new ContractContext(new DbContextOptionsBuilder<ContractContext>().Options);
    var reader = new StubLiveSchemaReader(DataVaultLiveSchemaReadResult.UnsupportedProvider("Contract.Unsupported"));

    var report = await DataVaultLiveSchemaDriftReporter.CompareAsync(
        LiveSchemaReaderContractFixture.CreateCustomerOnlyMetadataModel(),
        context,
        reader);

    Assert.True(report.HasBlockingDifferences);
    var difference = Assert.Single(report.Differences);
    Assert.Equal("live-schema-provider-unsupported", difference.Code);
    Assert.Equal(DataVaultLiveSchemaReadStatus.Succeeded.ToString(), difference.ExpectedValue);
    Assert.Equal(DataVaultLiveSchemaReadStatus.UnsupportedProvider.ToString(), difference.ActualValue);
  }

  [Fact]
  public async Task CompareAsyncClassifiesUnavailableReaderResultWithoutThrowing() {
    await using var context = new ContractContext(new DbContextOptionsBuilder<ContractContext>().Options);
    var reader = new StubLiveSchemaReader(
        DataVaultLiveSchemaReadResult.Unavailable("Contract.Unavailable", "catalog connection failed"));

    var report = await DataVaultLiveSchemaDriftReporter.CompareAsync(
        LiveSchemaReaderContractFixture.CreateCustomerOnlyMetadataModel(),
        context,
        reader);

    Assert.True(report.HasBlockingDifferences);
    var difference = Assert.Single(report.Differences);
    Assert.Equal("live-schema-unavailable", difference.Code);
    Assert.Equal(DataVaultLiveSchemaReadStatus.Succeeded.ToString(), difference.ExpectedValue);
    Assert.Equal(DataVaultLiveSchemaReadStatus.Unavailable.ToString(), difference.ActualValue);
    Assert.Contains("catalog connection failed", difference.Message, StringComparison.Ordinal);
  }

  private sealed class ContractContext(DbContextOptions<ContractContext> options) : DbContext(options);

  private sealed class StubLiveSchemaReader(DataVaultLiveSchemaReadResult result) : IDataVaultLiveSchemaReader {
    public Task<DataVaultLiveSchemaReadResult> ReadAsync(
        DbContext dbContext,
        CancellationToken cancellationToken = default) {
      ArgumentNullException.ThrowIfNull(dbContext);

      return Task.FromResult(result);
    }
  }
}
