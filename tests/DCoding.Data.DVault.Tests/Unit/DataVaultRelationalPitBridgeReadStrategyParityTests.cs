using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultRelationalPitBridgeReadStrategyParityTests {
  private const string StableHashAlgorithmId = "sha256-v1";
  private const int StableHashDigestByteLength = 32;

  [Fact]
  public async Task RelationalLatestSatelliteCandidatesReturnProviderNeutralRowsAndProjections() {
    var metadata = CreatePitMetadata();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitReadParityContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateSqliteProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var fallbackProvider = CreateFallbackProvider();
    var fallbackReadService = fallbackProvider.GetRequiredService<IDataVaultReadService>();
    var sqlServerReadService = CreateLatestCandidateReadService(new SqlServerDataVaultReadStrategy());
    var oracleReadService = CreateLatestCandidateReadService(new OracleDataVaultReadStrategy());
    var db2ReadService = CreateLatestCandidateReadService(new Db2DataVaultReadStrategy());
    string customerHashKey;

    await using (var context = new PitReadParityContext(options)) {
      await context.Database.EnsureCreatedAsync();
      customerHashKey = await SaveCustomerAsync(saveService, context, metadata);
      await SaveProfileAsync(
          saveService,
          context,
          metadata,
          customerHashKey,
          Utc(2026, 5, 11, 10, 0),
          "Alice Adams",
          "Gold",
          "profile-hash-1");
      await SaveProfileAsync(
          saveService,
          context,
          metadata,
          customerHashKey,
          Utc(2026, 5, 11, 11, 0),
          "Alice Baker",
          "Platinum",
          "profile-hash-2");
    }

    await using (var context = new PitReadParityContext(options)) {
      var latestRequest = new DataVaultLatestSatelliteReadRequest(metadata.Profile, [customerHashKey]);
      var asOfRequest = new DataVaultLatestSatelliteReadRequest(
          metadata.Profile,
          [customerHashKey],
          Utc(2026, 5, 11, 10, 30));

      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateSqlServer(KnownProviderNames.SqlServer, latestRequest)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateOracle(KnownProviderNames.Oracle, latestRequest)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateDb2(KnownProviderNames.Db2, latestRequest)
          .CanRead);

      var fallbackLatestRows = await fallbackReadService.ReadLatestSatelliteRowsAsync(context, latestRequest);
      var sqlServerLatestRows = await new SqlServerDataVaultReadStrategy().ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(context, latestRequest));
      var oracleLatestRows = await new OracleDataVaultReadStrategy().ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(context, latestRequest));
      var db2LatestRows = await new Db2DataVaultReadStrategy().ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(context, latestRequest));
      var fallbackAsOfRows = await fallbackReadService.ReadLatestSatelliteRowsAsync(context, asOfRequest);
      var sqlServerAsOfRows = await new SqlServerDataVaultReadStrategy().ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(context, asOfRequest));
      var oracleAsOfRows = await new OracleDataVaultReadStrategy().ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(context, asOfRequest));
      var db2AsOfRows = await new Db2DataVaultReadStrategy().ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(context, asOfRequest));
      var fallbackLatestProjections = await ProjectLatestRowsAsync(fallbackReadService, context, latestRequest);
      var sqlServerLatestProjections = await ProjectLatestRowsAsync(sqlServerReadService, context, latestRequest);
      var oracleLatestProjections = await ProjectLatestRowsAsync(oracleReadService, context, latestRequest);
      var db2LatestProjections = await ProjectLatestRowsAsync(db2ReadService, context, latestRequest);
      var fallbackAsOfProjections = await ProjectLatestRowsAsync(fallbackReadService, context, asOfRequest);
      var sqlServerAsOfProjections = await ProjectLatestRowsAsync(sqlServerReadService, context, asOfRequest);
      var oracleAsOfProjections = await ProjectLatestRowsAsync(oracleReadService, context, asOfRequest);
      var db2AsOfProjections = await ProjectLatestRowsAsync(db2ReadService, context, asOfRequest);

      Assert.Equal(FormatSatelliteRows(fallbackLatestRows), FormatSatelliteRows(sqlServerLatestRows));
      Assert.Equal(FormatSatelliteRows(fallbackLatestRows), FormatSatelliteRows(oracleLatestRows));
      Assert.Equal(FormatSatelliteRows(fallbackLatestRows), FormatSatelliteRows(db2LatestRows));
      Assert.Equal(FormatSatelliteRows(fallbackAsOfRows), FormatSatelliteRows(sqlServerAsOfRows));
      Assert.Equal(FormatSatelliteRows(fallbackAsOfRows), FormatSatelliteRows(oracleAsOfRows));
      Assert.Equal(FormatSatelliteRows(fallbackAsOfRows), FormatSatelliteRows(db2AsOfRows));
      Assert.Equal(["Alice Baker|Platinum|profile-hash-2"], fallbackLatestProjections);
      Assert.Equal(fallbackLatestProjections, sqlServerLatestProjections);
      Assert.Equal(fallbackLatestProjections, oracleLatestProjections);
      Assert.Equal(fallbackLatestProjections, db2LatestProjections);
      Assert.Equal(["Alice Adams|Gold|profile-hash-1"], fallbackAsOfProjections);
      Assert.Equal(fallbackAsOfProjections, sqlServerAsOfProjections);
      Assert.Equal(fallbackAsOfProjections, oracleAsOfProjections);
      Assert.Equal(fallbackAsOfProjections, db2AsOfProjections);
    }
  }

  [Fact]
  public async Task RelationalPitCandidatesReturnProviderNeutralRowsAndProjections() {
    var metadata = CreatePitMetadata();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitReadParityContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateSqliteProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var fallbackProvider = CreateFallbackProvider();
    var fallbackReadService = fallbackProvider.GetRequiredService<IDataVaultReadService>();
    var postgresReadService = CreatePitCandidateReadService(new PostgresDataVaultReadStrategy());
    var sqlServerReadService = CreatePitCandidateReadService(new SqlServerDataVaultReadStrategy());
    var mySqlReadService = CreatePitCandidateReadService(new MySqlDataVaultReadStrategy());
    var oracleReadService = CreatePitCandidateReadService(new OracleDataVaultReadStrategy());
    var db2ReadService = CreatePitCandidateReadService(new Db2DataVaultReadStrategy());
    string customerHashKey;

    await using (var context = new PitReadParityContext(options)) {
      await context.Database.EnsureCreatedAsync();
      customerHashKey = await SaveCustomerAsync(saveService, context, metadata);
      await SaveProfileAsync(
          saveService,
          context,
          metadata,
          customerHashKey,
          Utc(2026, 5, 11, 10, 0),
          "Alice Adams",
          "Gold",
          "profile-hash-1");
      await SaveStatusAsync(
          saveService,
          context,
          metadata,
          customerHashKey,
          Utc(2026, 5, 11, 10, 30),
          "Active",
          "status-hash-1");
      await SaveProfileAsync(
          saveService,
          context,
          metadata,
          customerHashKey,
          Utc(2026, 5, 11, 11, 0),
          "Alice Baker",
          "Platinum",
          "profile-hash-2");

      context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["CustomerHashKey"] = customerHashKey,
        ["LoadTimestamp"] = Utc(2026, 5, 11, 11, 15),
        ["ProfileLoadTimestamp"] = Utc(2026, 5, 11, 11, 0),
        ["StatusLoadTimestamp"] = Utc(2026, 5, 11, 10, 30),
      });
      context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["CustomerHashKey"] = customerHashKey,
        ["LoadTimestamp"] = Utc(2026, 5, 11, 12, 30),
        ["ProfileLoadTimestamp"] = Utc(2026, 5, 11, 11, 0),
        ["StatusLoadTimestamp"] = Utc(2026, 5, 11, 10, 30),
      });
      await context.SaveChangesAsync();
    }

    await using (var context = new PitReadParityContext(options)) {
      var request = new DataVaultPitAsOfReadRequest(
          metadata.Pit,
          [customerHashKey],
          Utc(2026, 5, 11, 12, 0));

      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluatePostgres(KnownProviderNames.Postgres, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateSqlServer(KnownProviderNames.SqlServer, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateMySql(KnownProviderNames.MySqlPomelo, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateMySql(KnownProviderNames.MySqlOracle, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateOracle(KnownProviderNames.Oracle, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateDb2(KnownProviderNames.Db2, request)
          .CanRead);

      var fallbackRows = await fallbackReadService.ReadPitRowsAsync(context, request);
      var postgresRows = await new PostgresDataVaultReadStrategy().ReadPitRowsAsync(
          new DataVaultProviderPitReadStrategyContext(context, request));
      var sqlServerRows = await new SqlServerDataVaultReadStrategy().ReadPitRowsAsync(
          new DataVaultProviderPitReadStrategyContext(context, request));
      var mySqlRows = await new MySqlDataVaultReadStrategy().ReadPitRowsAsync(
          new DataVaultProviderPitReadStrategyContext(context, request));
      var oracleRows = await new OracleDataVaultReadStrategy().ReadPitRowsAsync(
          new DataVaultProviderPitReadStrategyContext(context, request));
      var db2Rows = await new Db2DataVaultReadStrategy().ReadPitRowsAsync(
          new DataVaultProviderPitReadStrategyContext(context, request));
      var fallbackProjections = await ProjectPitRowsAsync(fallbackReadService, context, request);
      var postgresProjections = await ProjectPitRowsAsync(postgresReadService, context, request);
      var sqlServerProjections = await ProjectPitRowsAsync(sqlServerReadService, context, request);
      var mySqlProjections = await ProjectPitRowsAsync(mySqlReadService, context, request);
      var oracleProjections = await ProjectPitRowsAsync(oracleReadService, context, request);
      var db2Projections = await ProjectPitRowsAsync(db2ReadService, context, request);

      Assert.Equal(FormatPitRows(fallbackRows), FormatPitRows(postgresRows));
      Assert.Equal(FormatPitRows(fallbackRows), FormatPitRows(sqlServerRows));
      Assert.Equal(FormatPitRows(fallbackRows), FormatPitRows(mySqlRows));
      Assert.Equal(FormatPitRows(fallbackRows), FormatPitRows(oracleRows));
      Assert.Equal(FormatPitRows(fallbackRows), FormatPitRows(db2Rows));
      Assert.Equal(fallbackProjections, postgresProjections);
      Assert.Equal(fallbackProjections, sqlServerProjections);
      Assert.Equal(fallbackProjections, mySqlProjections);
      Assert.Equal(fallbackProjections, oracleProjections);
      Assert.Equal(fallbackProjections, db2Projections);
    }
  }

  [Fact]
  public async Task PostgresLatestSatelliteCandidateReturnsProviderNeutralRowsAndProjections() {
    var metadata = CreatePitMetadata();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitReadParityContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateSqliteProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    using var fallbackProvider = CreateFallbackProvider();
    var fallbackReadService = fallbackProvider.GetRequiredService<IDataVaultReadService>();
    var postgresReadService = CreateLatestCandidateReadService(new PostgresDataVaultReadStrategy());
    string customerHashKey;

    await using (var context = new PitReadParityContext(options)) {
      await context.Database.EnsureCreatedAsync();
      customerHashKey = await SaveCustomerAsync(saveService, context, metadata);
      await SaveProfileAsync(
          saveService,
          context,
          metadata,
          customerHashKey,
          Utc(2026, 5, 11, 10, 0),
          "Alice Adams",
          "Gold",
          "profile-hash-1");
      await SaveProfileAsync(
          saveService,
          context,
          metadata,
          customerHashKey,
          Utc(2026, 5, 11, 11, 0),
          "Alice Baker",
          "Platinum",
          "profile-hash-2");
    }

    await using (var context = new PitReadParityContext(options)) {
      var request = new DataVaultLatestSatelliteReadRequest(
          metadata.Profile,
          [customerHashKey],
          Utc(2026, 5, 11, 10, 45));

      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluatePostgres(KnownProviderNames.Postgres, request)
          .CanRead);

      var fallbackRows = await fallbackReadService.ReadLatestSatelliteRowsAsync(context, request);
      var postgresRows = await new PostgresDataVaultReadStrategy().ReadLatestSatelliteRowsAsync(
          new DataVaultProviderReadStrategyContext(context, request));
      var fallbackProjections = await ProjectLatestRowsAsync(fallbackReadService, context, request);
      var postgresProjections = await ProjectLatestRowsAsync(postgresReadService, context, request);

      Assert.Equal(FormatLatestRows(fallbackRows), FormatLatestRows(postgresRows));
      Assert.Equal(fallbackProjections, postgresProjections);
    }
  }

  [Fact]
  public async Task RelationalBridgeCandidatesReturnProviderNeutralRowsAndProjections() {
    var bridge = ManyToManyMetadataModel.Bridges.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<BridgeReadParityContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var fallbackProvider = CreateFallbackProvider();
    var fallbackReadService = fallbackProvider.GetRequiredService<IDataVaultReadService>();
    var postgresReadService = CreateBridgeCandidateReadService(new PostgresDataVaultReadStrategy());
    var sqlServerReadService = CreateBridgeCandidateReadService(new SqlServerDataVaultReadStrategy());
    var mySqlReadService = CreateBridgeCandidateReadService(new MySqlDataVaultReadStrategy());
    var oracleReadService = CreateBridgeCandidateReadService(new OracleDataVaultReadStrategy());
    var db2ReadService = CreateBridgeCandidateReadService(new Db2DataVaultReadStrategy());

    await using (var context = new BridgeReadParityContext(options)) {
      await context.Database.EnsureCreatedAsync();
      await SeedBridgeRowAsync(context, "customer-1", "order-2");
      await SeedBridgeRowAsync(context, "customer-2", "order-3");
      await SeedBridgeRowAsync(context, "customer-1", "order-1");
    }

    await using (var context = new BridgeReadParityContext(options)) {
      var request = new DataVaultBridgeReadRequest(
          bridge,
          DataVaultBridgeTraversalEndpoint.From,
          ["customer-1"]);

      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluatePostgres(KnownProviderNames.Postgres, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateSqlServer(KnownProviderNames.SqlServer, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateMySql(KnownProviderNames.MySqlPomelo, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateMySql(KnownProviderNames.MySqlOracle, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateOracle(KnownProviderNames.Oracle, request)
          .CanRead);
      Assert.True(DataVaultProviderReadStrategyGateEvaluator
          .EvaluateDb2(KnownProviderNames.Db2, request)
          .CanRead);

      var fallbackRows = await fallbackReadService.ReadBridgeRowsAsync(context, request);
      var postgresRows = await new PostgresDataVaultReadStrategy().ReadBridgeRowsAsync(
          new DataVaultProviderBridgeReadStrategyContext(context, request));
      var sqlServerRows = await new SqlServerDataVaultReadStrategy().ReadBridgeRowsAsync(
          new DataVaultProviderBridgeReadStrategyContext(context, request));
      var mySqlRows = await new MySqlDataVaultReadStrategy().ReadBridgeRowsAsync(
          new DataVaultProviderBridgeReadStrategyContext(context, request));
      var oracleRows = await new OracleDataVaultReadStrategy().ReadBridgeRowsAsync(
          new DataVaultProviderBridgeReadStrategyContext(context, request));
      var db2Rows = await new Db2DataVaultReadStrategy().ReadBridgeRowsAsync(
          new DataVaultProviderBridgeReadStrategyContext(context, request));
      var fallbackProjections = await ProjectBridgeRowsAsync(fallbackReadService, context, request);
      var postgresProjections = await ProjectBridgeRowsAsync(postgresReadService, context, request);
      var sqlServerProjections = await ProjectBridgeRowsAsync(sqlServerReadService, context, request);
      var mySqlProjections = await ProjectBridgeRowsAsync(mySqlReadService, context, request);
      var oracleProjections = await ProjectBridgeRowsAsync(oracleReadService, context, request);
      var db2Projections = await ProjectBridgeRowsAsync(db2ReadService, context, request);

      Assert.Equal(FormatBridgeRows(fallbackRows), FormatBridgeRows(postgresRows));
      Assert.Equal(FormatBridgeRows(fallbackRows), FormatBridgeRows(sqlServerRows));
      Assert.Equal(FormatBridgeRows(fallbackRows), FormatBridgeRows(mySqlRows));
      Assert.Equal(FormatBridgeRows(fallbackRows), FormatBridgeRows(oracleRows));
      Assert.Equal(FormatBridgeRows(fallbackRows), FormatBridgeRows(db2Rows));
      Assert.Equal(fallbackProjections, postgresProjections);
      Assert.Equal(fallbackProjections, sqlServerProjections);
      Assert.Equal(fallbackProjections, mySqlProjections);
      Assert.Equal(fallbackProjections, oracleProjections);
      Assert.Equal(fallbackProjections, db2Projections);
    }
  }

  [Fact]
  public async Task RelationalPitAndBridgeCandidatesRoundTripBinaryHashKeyStorage() {
    var metadata = CreatePitMetadata();
    using var fallbackProvider = CreateFallbackProvider();
    var fallbackReadService = fallbackProvider.GetRequiredService<IDataVaultReadService>();
    var postgresPitReadService = CreatePitCandidateReadService(new PostgresDataVaultReadStrategy());
    var postgresBridgeReadService = CreateBridgeCandidateReadService(new PostgresDataVaultReadStrategy());
    string customerHashKey;

    using (var pitDatabase = SqliteTestDatabase.CreateTemporaryFile()) {
      var pitOptions = new DbContextOptionsBuilder<PitReadParityContext>()
          .UseSqlite("Data Source=" + Assert.IsType<string>(pitDatabase.DatabasePath) + ";Pooling=False")
          .ReplaceService<IModelCacheKeyFactory, ReadParityModelCacheKeyFactory>()
          .Options;
      using var saveProvider = CreateSqliteProvider();
      var saveService = saveProvider.GetRequiredService<IDataVaultSaveService>();

      await using (var context = new PitReadParityContext(pitOptions, DataVaultHashKeyStorageProfile.Binary)) {
        await context.Database.EnsureCreatedAsync();
        customerHashKey = await SaveCustomerAsync(saveService, context, metadata);
        await SaveProfileAsync(
            saveService,
            context,
            metadata,
            customerHashKey,
            Utc(2026, 5, 11, 11, 0),
            "Alice Binary",
            "Platinum",
            "profile-hash-2");
        await SaveStatusAsync(
            saveService,
            context,
            metadata,
            customerHashKey,
            Utc(2026, 5, 11, 10, 30),
            "Active",
            "status-hash-1");

        context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
          ["CustomerHashKey"] = customerHashKey,
          ["LoadTimestamp"] = Utc(2026, 5, 11, 11, 15),
          ["ProfileLoadTimestamp"] = Utc(2026, 5, 11, 11, 0),
          ["StatusLoadTimestamp"] = Utc(2026, 5, 11, 10, 30),
        });
        await context.SaveChangesAsync();
      }

      await using (var context = new PitReadParityContext(pitOptions, DataVaultHashKeyStorageProfile.Binary)) {
        var request = new DataVaultPitAsOfReadRequest(
            metadata.Pit,
            [customerHashKey],
            Utc(2026, 5, 11, 12, 0));
        var fallbackRows = await fallbackReadService.ReadPitRowsAsync(context, request);
        var postgresRows = await new PostgresDataVaultReadStrategy().ReadPitRowsAsync(
            new DataVaultProviderPitReadStrategyContext(context, request));
        var fallbackProjections = await ProjectPitRowsAsync(fallbackReadService, context, request);
        var postgresProjections = await ProjectPitRowsAsync(postgresPitReadService, context, request);

        Assert.Equal(FormatPitRows(fallbackRows), FormatPitRows(postgresRows));
        Assert.Equal(fallbackProjections, postgresProjections);
      }
    }

    using (var bridgeDatabase = SqliteTestDatabase.CreateTemporaryFile()) {
      var bridge = ManyToManyMetadataModel.Bridges.Single();
      var bridgeOptions = new DbContextOptionsBuilder<BridgeReadParityContext>()
          .UseSqlite("Data Source=" + Assert.IsType<string>(bridgeDatabase.DatabasePath) + ";Pooling=False")
          .ReplaceService<IModelCacheKeyFactory, ReadParityModelCacheKeyFactory>()
          .Options;
      var orderHashKey = CreateCanonicalHexDigest(seed: 17);

      await using (var context = new BridgeReadParityContext(bridgeOptions, DataVaultHashKeyStorageProfile.Binary)) {
        await context.Database.EnsureCreatedAsync();
        context.Set<Dictionary<string, object>>("BridgeCustomerOrder").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
          ["CustomerHashKey"] = customerHashKey,
          ["OrderHashKey"] = orderHashKey,
        });
        await context.SaveChangesAsync();
      }

      await using (var context = new BridgeReadParityContext(bridgeOptions, DataVaultHashKeyStorageProfile.Binary)) {
        var request = new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.From,
            [customerHashKey]);
        var fallbackRows = await fallbackReadService.ReadBridgeRowsAsync(context, request);
        var postgresRows = await new PostgresDataVaultReadStrategy().ReadBridgeRowsAsync(
            new DataVaultProviderBridgeReadStrategyContext(context, request));
        var fallbackProjections = await ProjectBridgeRowsAsync(fallbackReadService, context, request);
        var postgresProjections = await ProjectBridgeRowsAsync(postgresBridgeReadService, context, request);

        Assert.Equal(FormatBridgeRows(fallbackRows), FormatBridgeRows(postgresRows));
        Assert.Equal(fallbackProjections, postgresProjections);
      }
    }
  }

  private static ServiceProvider CreateSqliteProvider() {
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static ServiceProvider CreateFallbackProvider() {
    var services = new ServiceCollection();
    services.AddDVault();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static IDataVaultReadService CreateLatestCandidateReadService(IDataVaultProviderReadStrategy strategy) {
    return new DefaultDataVaultReadService(
        [new AlwaysAcceptLatestReadStrategy(strategy)],
        Array.Empty<IDataVaultProviderPitReadStrategy>(),
        Array.Empty<IDataVaultProviderBridgeReadStrategy>());
  }

  private static IDataVaultReadService CreatePitCandidateReadService(IDataVaultProviderPitReadStrategy strategy) {
    return new DefaultDataVaultReadService(
        Array.Empty<IDataVaultProviderReadStrategy>(),
        [new AlwaysAcceptPitReadStrategy(strategy)],
        Array.Empty<IDataVaultProviderBridgeReadStrategy>());
  }

  private static IDataVaultReadService CreateBridgeCandidateReadService(IDataVaultProviderBridgeReadStrategy strategy) {
    return new DefaultDataVaultReadService(
        Array.Empty<IDataVaultProviderReadStrategy>(),
        Array.Empty<IDataVaultProviderPitReadStrategy>(),
        [new AlwaysAcceptBridgeReadStrategy(strategy)]);
  }

  private static async Task<IReadOnlyList<CustomerSnapshotRead>> ProjectPitRowsAsync(
      IDataVaultReadService readService,
      DbContext context,
      DataVaultPitAsOfReadRequest request) {
    return await readService.ReadPitAsync(
        context,
        request,
        row => {
          var profile = row.RequiredSatellite("Profile");
          var status = row.RequiredSatellite("Status");

          return new CustomerSnapshotRead(
              row.RequiredString("ParentHashKey"),
              row.RequiredDateTimeOffset("LoadTimestamp"),
              profile.RequiredString("Customer Name"),
              profile.RequiredString("Customer Tier"),
              status.RequiredString("Status Code"));
        });
  }

  private static async Task<IReadOnlyList<string>> ProjectBridgeRowsAsync(
      IDataVaultReadService readService,
      DbContext context,
      DataVaultBridgeReadRequest request) {
    return await readService.ReadBridgeAsync(
        context,
        request,
        row => row.RequiredString("CustomerHashKey") + "->" + row.RequiredString("OrderHashKey"));
  }

  private static async Task<IReadOnlyList<string>> ProjectLatestRowsAsync(
      IDataVaultReadService readService,
      DbContext context,
      DataVaultLatestSatelliteReadRequest request) {
    return await readService.ReadLatestSatelliteAsync(
        context,
        request,
        row =>
            row.RequiredString("Customer Name") +
            "|" +
            row.RequiredString("Customer Tier") +
            "|" +
            row.RequiredString("HashDiff"));
  }

  private static async Task<string> SaveCustomerAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitReadMetadata metadata) {
    var result = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            Utc(2026, 5, 11, 8, 0),
            "crm-import",
            [new(metadata.Customer, [new("Customer Id", "C-100")])],
            []));

    return result.SavedRecords
        .Single(record => record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Customer")
        .HashKey;
  }

  private static Task SaveProfileAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitReadMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string customerName,
      string customerTier,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-profile",
            [],
            [],
            [
                new(
                    metadata.Profile,
                    customerHashKey,
                    [new("Customer Name", customerName), new("Customer Tier", customerTier)],
                    hashDiff),
            ]));
  }

  private static Task SaveStatusAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitReadMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string statusCode,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-status",
            [],
            [],
            [
                new(
                    metadata.Status,
                    customerHashKey,
                    [new("Status Code", statusCode)],
                    hashDiff),
            ]));
  }

  private static Task SeedBridgeRowAsync(
      DbContext context,
      string customerHashKey,
      string orderHashKey) {
    return context.Database.ExecuteSqlRawAsync(
        "INSERT INTO \"BridgeCustomerOrder\" (\"CustomerHashKey\", \"OrderHashKey\") VALUES ({0}, {1});",
        customerHashKey,
        orderHashKey);
  }

  private static IReadOnlyList<string> FormatPitRows(IReadOnlyList<DataVaultPitReadRecord> rows) {
    return rows
        .Select(row =>
            row.ParentHashKey +
            "|" +
            row.LoadTimestamp.ToString("O", CultureInfo.InvariantCulture) +
            "|" +
            FormatDictionary(row.DrivingKeyValues) +
            "|" +
            string.Join(";", row.SatelliteSnapshots.Select(FormatSnapshot)))
        .ToArray();
  }

  private static IReadOnlyList<string> FormatSatelliteRows(IReadOnlyList<DataVaultSatelliteReadRecord> rows) {
    return rows
        .Select(row =>
            row.MetadataName +
            "|" +
            row.TableName +
            "|" +
            row.ParentHashKey +
            "|" +
            row.HashDiff +
            "|" +
            row.LoadTimestamp.ToString("O", CultureInfo.InvariantCulture) +
            "|" +
            row.RecordSource +
            "|" +
            FormatDictionary(row.PayloadValues))
        .ToArray();
  }

  private static string FormatSnapshot(DataVaultPitSatelliteSnapshot snapshot) {
    return snapshot.SatelliteName +
        ":" +
        snapshot.Ordinal.ToString(CultureInfo.InvariantCulture) +
        ":" +
        snapshot.IsPresent.ToString(CultureInfo.InvariantCulture) +
        ":" +
        (snapshot.SnapshotLoadTimestamp?.ToString("O", CultureInfo.InvariantCulture) ?? "<null>") +
        ":" +
        (snapshot.HashDiff ?? "<null>") +
        ":" +
        (snapshot.RecordSource ?? "<null>") +
        ":" +
        FormatDictionary(snapshot.PayloadValues);
  }

  private static IReadOnlyList<string> FormatLatestRows(IReadOnlyList<DataVaultSatelliteReadRecord> rows) {
    return rows
        .Select(row =>
            row.MetadataName +
            "|" +
            row.TableName +
            "|" +
            row.ParentHashKey +
            "|" +
            FormatDictionary(row.DrivingKeyValues) +
            "|" +
            row.HashDiff +
            "|" +
            row.LoadTimestamp.ToString("O", CultureInfo.InvariantCulture) +
            "|" +
            row.RecordSource +
            "|" +
            FormatDictionary(row.PayloadValues))
        .ToArray();
  }

  private static IReadOnlyList<string> FormatBridgeRows(IReadOnlyList<DataVaultBridgeReadRecord> rows) {
    return rows
        .Select(row =>
            row.MetadataName +
            "|" +
            row.TableName +
            "|" +
            (row.TraversalDepth?.ToString(CultureInfo.InvariantCulture) ?? "<null>") +
            "|" +
            string.Join(";", row.EndpointHashKeys.Select(FormatEndpoint)))
        .ToArray();
  }

  private static string FormatEndpoint(DataVaultBridgeEndpointReadValue endpoint) {
    return endpoint.Endpoint +
        ":" +
        endpoint.EndpointName +
        ":" +
        endpoint.ColumnName +
        ":" +
        endpoint.HashKey;
  }

  private static string FormatDictionary<TValue>(IReadOnlyDictionary<string, TValue> values) {
    return string.Join(
        ",",
        values
            .OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => pair.Key + "=" + (pair.Value?.ToString() ?? "<null>")));
  }

  private static PitReadMetadata CreatePitMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new PitReadMetadata(customer, profile, status, pit, model);
  }

  private static DataVaultMetadataModel CreateManyToManyMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    return new DataVaultMetadataModel([customer, order], [customerOrder], [], [bridge]);
  }

  private static DataVaultProviderCapabilityProfile CreateSqliteProfile(
      DataVaultHashKeyStorageProfile storageProfile) {
    return DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
        storageProfile,
        StableHashAlgorithmId,
        StableHashDigestByteLength);
  }

  private static string CreateCanonicalHexDigest(int seed) {
    return Convert.ToHexString(Enumerable
        .Range(0, StableHashDigestByteLength)
        .Select(value => (byte)((value + seed) % 256))
        .ToArray()).ToLowerInvariant();
  }

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private static DataVaultMetadataModel ManyToManyMetadataModel { get; } = CreateManyToManyMetadataModel();

  private sealed class PitReadParityContext(
      DbContextOptions<PitReadParityContext> options,
      DataVaultHashKeyStorageProfile storageProfile = DataVaultHashKeyStorageProfile.HexString) : DbContext(options) {
    public DataVaultHashKeyStorageProfile StorageProfile { get; } = storageProfile;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreatePitMetadata().Model, CreateSqliteProfile(StorageProfile));
    }
  }

  private sealed class BridgeReadParityContext(
      DbContextOptions<BridgeReadParityContext> options,
      DataVaultHashKeyStorageProfile storageProfile = DataVaultHashKeyStorageProfile.HexString) : DbContext(options) {
    public DataVaultHashKeyStorageProfile StorageProfile { get; } = storageProfile;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ManyToManyMetadataModel, CreateSqliteProfile(StorageProfile));
    }
  }

  private sealed class ReadParityModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context switch {
        PitReadParityContext pitContext => (context.GetType(), pitContext.StorageProfile, designTime),
        BridgeReadParityContext bridgeContext => (context.GetType(), bridgeContext.StorageProfile, designTime),
        _ => (object)(context.GetType(), designTime),
      };
    }
  }

  private sealed class AlwaysAcceptLatestReadStrategy(IDataVaultProviderReadStrategy inner) : IDataVaultProviderReadStrategy {
    public int Priority => inner.Priority;

    public bool CanReadLatestSatelliteRows(DbContext dbContext, DataVaultLatestSatelliteReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return true;
    }

    public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      return inner.ReadLatestSatelliteRowsAsync(context, cancellationToken);
    }

    public Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
        DataVaultProviderReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      return inner.ReadLatestSatelliteProjectionRowsAsync(context, cancellationToken);
    }
  }

  private sealed class AlwaysAcceptPitReadStrategy(IDataVaultProviderPitReadStrategy inner) : IDataVaultProviderPitReadStrategy {
    public int Priority => inner.Priority;

    public bool CanReadPitRows(DbContext dbContext, DataVaultPitAsOfReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return true;
    }

    public Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
        DataVaultProviderPitReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      return inner.ReadPitRowsAsync(context, cancellationToken);
    }
  }

  private sealed class AlwaysAcceptBridgeReadStrategy(IDataVaultProviderBridgeReadStrategy inner) : IDataVaultProviderBridgeReadStrategy {
    public int Priority => inner.Priority;

    public bool CanReadBridgeRows(DbContext dbContext, DataVaultBridgeReadRequest request) {
      ArgumentNullException.ThrowIfNull(dbContext);
      ArgumentNullException.ThrowIfNull(request);

      return true;
    }

    public Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
        DataVaultProviderBridgeReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      return inner.ReadBridgeRowsAsync(context, cancellationToken);
    }

    public Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
        DataVaultProviderBridgeReadStrategyContext context,
        CancellationToken cancellationToken = default) {
      return inner.ReadBridgeProjectionRowsAsync(context, cancellationToken);
    }
  }

  private sealed record PitReadMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Status,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record CustomerSnapshotRead(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      string CustomerName,
      string CustomerTier,
      string StatusCode);

}
