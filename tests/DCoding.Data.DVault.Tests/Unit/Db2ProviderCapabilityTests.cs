using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class Db2ProviderCapabilityTests {
  [Fact]
  public void AddDVaultDb2RegistersOptimizedStrategiesAndProviderProfileSelection() {
    try {
      var services = new ServiceCollection();

      services.AddDVaultDb2();

      using var provider = services.BuildServiceProvider(validateScopes: true);

      Assert.Contains(provider.GetServices<IDataVaultProviderSaveStrategy>(), strategy => strategy is Db2DataVaultSaveStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderPitReadStrategy>(), strategy => strategy is Db2DataVaultReadStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderBridgeReadStrategy>(), strategy => strategy is Db2DataVaultReadStrategy);
      Assert.Same(
          DataVaultProviderCapabilityProfiles.Db2,
          DataVaultProviderCapabilityProfileSelection.Select(Db2DataVaultSaveStrategy.Db2ProviderName));
      Assert.Same(
          DataVaultProviderCapabilityProfiles.Sqlite,
          DataVaultProviderCapabilityProfileSelection.Select((string?)null));
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  [Fact]
  public void Db2StrategyRecognizesOnlyIbmProviderName() {
    Assert.True(Db2DataVaultSaveStrategy.IsSupportedProviderName(Db2DataVaultSaveStrategy.Db2ProviderName));
    Assert.False(Db2DataVaultSaveStrategy.IsSupportedProviderName("Microsoft.EntityFrameworkCore.Sqlite"));
    Assert.False(Db2DataVaultSaveStrategy.IsSupportedProviderName(null));
  }

  [Fact]
  public void Db2StrategyBuildsParameterizedBatchSqlInsideProviderPackage() {
    var insertCommandText = Db2DataVaultSaveStrategy.CreateDb2InsertCommandText(
        "SatCustomerProfile",
        ["CustomerHashKey", "HashDiff"],
        rowCount: 2);
    var uniqueInsertCommandText = Db2DataVaultSaveStrategy.CreateDb2UniqueInsertCommandText(
        "Hub\"Customer",
        ["CustomerHashKey", "LoadTimestamp"],
        "CustomerHashKey",
        rowCount: 2);

    Assert.Equal(
        "INSERT INTO \"SATCUSTOMERPROFILE\" (\"CUSTOMERHASHKEY\", \"HASHDIFF\") " +
        "VALUES (CAST(@p0 AS VARCHAR(32672)), CAST(@p1 AS VARCHAR(32672))), " +
        "(CAST(@p2 AS VARCHAR(32672)), CAST(@p3 AS VARCHAR(32672)))",
        insertCommandText);
    Assert.Equal(
        "INSERT INTO \"HUB\"\"CUSTOMER\" (\"CUSTOMERHASHKEY\", \"LOADTIMESTAMP\") SELECT " +
        "\"DEDUP\".\"CUSTOMERHASHKEY\", \"DEDUP\".\"LOADTIMESTAMP\" FROM (SELECT " +
        "\"SOURCE\".\"CUSTOMERHASHKEY\", \"SOURCE\".\"LOADTIMESTAMP\", " +
        "ROW_NUMBER() OVER (PARTITION BY \"SOURCE\".\"CUSTOMERHASHKEY\" ORDER BY " +
        "\"SOURCE\".\"__DVAULT_ORDINAL\") AS \"__DVAULT_ROW_NUMBER\" FROM " +
        "(VALUES (CAST(@p0 AS INTEGER), CAST(@p1 AS VARCHAR(32672)), CAST(@p2 AS VARCHAR(32672))), " +
        "(CAST(@p3 AS INTEGER), CAST(@p4 AS VARCHAR(32672)), CAST(@p5 AS VARCHAR(32672)))) AS \"SOURCE\" " +
        "(\"__DVAULT_ORDINAL\", \"CUSTOMERHASHKEY\", \"LOADTIMESTAMP\")) AS \"DEDUP\" " +
        "WHERE \"DEDUP\".\"__DVAULT_ROW_NUMBER\" = 1 AND NOT EXISTS " +
        "(SELECT 1 FROM \"HUB\"\"CUSTOMER\" AS \"TARGET\" WHERE " +
        "\"TARGET\".\"CUSTOMERHASHKEY\" = \"DEDUP\".\"CUSTOMERHASHKEY\")",
        uniqueInsertCommandText);
  }

  [Fact]
  public void Db2StrategyUsesWindowFunctionForLatestSatelliteHashDiffLookup() {
    var commandText = Db2DataVaultSaveStrategy.CreateLatestSatelliteHashDiffsCommandText(
        "SatCustomerProfile",
        "CustomerHashKey",
        "HashDiff",
        "LoadTimestamp",
        ["@p0", "@p1"]);

    Assert.Equal(
        "SELECT \"LATEST\".\"CUSTOMERHASHKEY\", \"LATEST\".\"HASHDIFF\", \"LATEST\".\"LOADTIMESTAMP\" " +
        "FROM (SELECT \"CUSTOMERHASHKEY\", \"HASHDIFF\", \"LOADTIMESTAMP\", " +
        "ROW_NUMBER() OVER (PARTITION BY \"CUSTOMERHASHKEY\" ORDER BY \"LOADTIMESTAMP\" DESC) AS " +
        "\"__DVAULT_ROW_NUMBER\" FROM \"SATCUSTOMERPROFILE\" WHERE \"CUSTOMERHASHKEY\" IN (@p0, @p1)) " +
        "AS \"LATEST\" WHERE \"LATEST\".\"__DVAULT_ROW_NUMBER\" = 1",
        commandText);
  }
}
