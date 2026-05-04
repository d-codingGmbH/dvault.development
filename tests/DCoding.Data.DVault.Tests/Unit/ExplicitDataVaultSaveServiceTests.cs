using System.Collections;
using System.Reflection;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class ExplicitDataVaultSaveServiceTests {
  [Fact]
  public void AddDVaultProvidesDefaultExplicitSaveServiceWithoutSaveChangesInterceptor() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.NotNull(provider.GetRequiredService<IDataVaultSaveService>());
    Assert.Empty(provider.GetServices<ISaveChangesInterceptor>());
  }

  [Fact]
  public void AddDVaultPreservesCallerExplicitSaveServiceOverride() {
    var replacement = new ReplacementDataVaultSaveService();
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultSaveService>(replacement);

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Same(replacement, provider.GetRequiredService<IDataVaultSaveService>());
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.OracleProvider)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
  public void ProviderPackagesRegisterCoreSaveService() {
    AssertProviderRegistration(services => services.AddDVaultOracle(), expectProviderStrategy: true);
    AssertProviderRegistration(services => services.AddDVaultMySql(), expectProviderStrategy: false);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
  public void PostgresProviderPackageRegistersOptimizedSaveStrategy() {
    AssertProviderRegistration(services => services.AddDVaultPostgres(), expectProviderStrategy: true);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
  public void SqliteProviderPackageRegistersOptimizedSaveStrategy() {
    AssertProviderRegistration(services => services.AddDVaultSqlite(), expectProviderStrategy: true);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerProviderPackageRegistersOptimizedSaveStrategy() {
    AssertProviderRegistration(services => services.AddDVaultSqlServer(), expectProviderStrategy: true);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerProviderSaveStrategyAcceptsOnlyCleanSqlServerContexts() {
    var services = new ServiceCollection();
    services.AddDVaultSqlServer();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    Assert.Single(provider.GetServices<IDataVaultProviderSaveStrategy>());

    Assert.True(InvokeSqlServerCanSaveProvider("Microsoft.EntityFrameworkCore.SqlServer", hasPendingTrackedChanges: false));
    Assert.False(InvokeSqlServerCanSaveProvider("Microsoft.EntityFrameworkCore.SqlServer", hasPendingTrackedChanges: true));
    Assert.False(InvokeSqlServerCanSaveProvider("Microsoft.EntityFrameworkCore.Sqlite", hasPendingTrackedChanges: false));
    Assert.False(InvokeSqlServerCanSaveProvider(null, hasPendingTrackedChanges: false));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerUniqueInsertSqlUsesSetBasedExistenceDetection() {
    var commandText = InvokeSqlServerCommandTextFactory(
        "CreateSqlServerUniqueInsertCommandText",
        "HubCustomer",
        new[] { "CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId" },
        "CustomerHashKey",
        3);

    Assert.Contains("INSERT INTO [HubCustomer]", commandText, StringComparison.Ordinal);
    Assert.Contains("VALUES (@p0, @p1, @p2, @p3, @p4)", commandText, StringComparison.Ordinal);
    Assert.Contains("(@p10, @p11, @p12, @p13, @p14)", commandText, StringComparison.Ordinal);
    Assert.Contains("ROW_NUMBER() OVER (PARTITION BY [source].[CustomerHashKey] ORDER BY [source].[__dvault_ordinal])", commandText, StringComparison.Ordinal);
    Assert.Contains("NOT EXISTS (SELECT 1 FROM [HubCustomer]", commandText, StringComparison.Ordinal);
    Assert.Equal(1, CountOccurrences(commandText, "NOT EXISTS"));
    Assert.Equal(1, CountOccurrences(commandText, "WITH (UPDLOCK, HOLDLOCK)"));
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerSatelliteLookupSqlRanksLatestHashDiffsByParentBatch() {
    var commandText = InvokeSqlServerCommandTextFactory(
        "CreateSqlServerLatestSatelliteHashDiffCommandText",
        "SatCustomerContact",
        "CustomerHashKey",
        "HashDiff",
        "LoadTimestamp",
        2);

    Assert.Contains("FROM (VALUES (@p0), (@p1))", commandText, StringComparison.Ordinal);
    Assert.Contains("INNER JOIN [requested]", commandText, StringComparison.Ordinal);
    Assert.Contains("ROW_NUMBER() OVER (PARTITION BY [target].[CustomerHashKey] ORDER BY [target].[LoadTimestamp] DESC)", commandText, StringComparison.Ordinal);
    Assert.Contains("WHERE [__dvault_row_number] = 1", commandText, StringComparison.Ordinal);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerSatelliteFilterUsesLatestHashDiffAcrossOrderedBatch() {
    var latestHashDiff = "hash-a";
    var latestLoadTimestamp = new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero);
    var candidates = new[] {
        new SatelliteDecisionCandidate("hash-a", new DateTimeOffset(2026, 5, 4, 10, 5, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-b", new DateTimeOffset(2026, 5, 4, 10, 10, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-b", new DateTimeOffset(2026, 5, 4, 10, 20, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-c", new DateTimeOffset(2026, 5, 4, 9, 30, 0, TimeSpan.Zero)),
        new SatelliteDecisionCandidate("hash-b", new DateTimeOffset(2026, 5, 4, 10, 30, 0, TimeSpan.Zero)),
    };

    var rowWrittenDecisions = new List<bool>();
    foreach (var candidate in candidates) {
      var rowWritten = InvokeSqlServerBooleanFactory(
          "ShouldWriteSatelliteHashDiff",
          latestHashDiff,
          candidate.HashDiff);
      rowWrittenDecisions.Add(rowWritten);

      if (rowWritten && InvokeSqlServerBooleanFactory(
          "ShouldAdvanceLatestSatelliteHashDiff",
          latestLoadTimestamp,
          candidate.LoadTimestamp)) {
        latestHashDiff = candidate.HashDiff;
        latestLoadTimestamp = candidate.LoadTimestamp;
      }
    }

    Assert.Equal([false, true, false, true, false], rowWrittenDecisions);
    Assert.Equal(2, rowWrittenDecisions.Count(rowWritten => rowWritten));
    Assert.Equal("hash-b", latestHashDiff);
    Assert.Equal(new DateTimeOffset(2026, 5, 4, 10, 10, 0, TimeSpan.Zero), latestLoadTimestamp);
  }

  [Fact]
  [Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.DefaultProviderSmoke)]
  [Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqlServerProvider)]
  public void SqlServerSavePlansKeepFallbackSavedRecordOrderingForBulkRequests() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["Email"]);
    var firstRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 4, 10, 0, 0, TimeSpan.Zero),
        "source-a",
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-100")])],
        [new DataVaultLinkSaveOperation(customerOrder, [new("Customer", "customer-hash"), new("Order", "order-hash")])],
        [new DataVaultSatelliteSaveOperation(contact, "customer-hash", [new("Email", "a@example.test")], "hash-a")]);
    var secondRequest = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 4, 10, 5, 0, TimeSpan.Zero),
        "source-b",
        [new DataVaultHubSaveOperation(order, [new("Order Id", "O-100")])],
        [],
        [new DataVaultSatelliteSaveOperation(contact, "customer-hash", [new("Email", "b@example.test")], "hash-b")]);

    using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);
    var requests = new[] { firstRequest, secondRequest };
    var strategyContext = new DataVaultProviderSaveStrategyContext(
        dbContext,
        requests,
        new TestStableHashService(),
        new TestStableHashNormalizer());
    var savedRecords = InvokeSqlServerSavedRecords("CreateUniqueRowSavePlans", strategyContext)
        .Concat(InvokeSqlServerSavedRecords("CreateSatelliteSavePlans", (object)requests))
        .ToArray();

    Assert.Collection(
        savedRecords,
        record => AssertSavedRecord(record, DataVaultTableKind.Hub, "Customer", "HubCustomer"),
        record => AssertSavedRecord(record, DataVaultTableKind.Link, "CustomerOrder", "LinkCustomerOrder"),
        record => AssertSavedRecord(record, DataVaultTableKind.Hub, "Order", "HubOrder"),
        record => AssertSavedRecord(record, DataVaultTableKind.Satellite, "Contact", "SatCustomerContact"),
        record => AssertSavedRecord(record, DataVaultTableKind.Satellite, "Contact", "SatCustomerContact"));
  }

  [Fact]
  public void SaveRequestKeepsExplicitMetadataBoundaryDeterministic() {
    var suppliedTimestamp = new DateTimeOffset(2026, 4, 29, 12, 15, 0, TimeSpan.FromHours(2));
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var request = new DataVaultSaveRequest(
        suppliedTimestamp,
        "crm-import",
        [new DataVaultHubSaveOperation(hub, [new("Customer Id", "C-100")])],
        []);

    Assert.Equal(new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero), request.LoadTimestamp);
    Assert.Equal("crm-import", request.RecordSource);
    Assert.Single(request.HubOperations);
    Assert.Empty(request.LinkOperations);
  }

  [Fact]
  public void BulkSaveRequestKeepsCallerSuppliedOrder() {
    var first = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
        "first-source",
        [],
        []);
    var second = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 11, 15, 0, TimeSpan.Zero),
        "second-source",
        [],
        []);
    var bulkRequest = new DataVaultBulkSaveRequest([first, second]);

    Assert.Equal([first, second], bulkRequest.Requests);
  }

  [Fact]
  public void BulkSaveRequestRejectsNullRequests() {
    Assert.Throws<ArgumentNullException>(() => new DataVaultBulkSaveRequest(null!));
    Assert.Throws<ArgumentException>(() => new DataVaultBulkSaveRequest([null!]));
  }

  [Fact]
  public void SaveOperationsRequireNamedValuesWithoutDuplicates() {
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new("Customer Id", "C-100"), new("Customer Id", "C-101")]));
    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new(" ", "C-100")]));
    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new("Customer Id", null!)]));
  }

  private sealed class ReplacementDataVaultSaveService : IDataVaultSaveService {
    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultBulkSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }
  }

  private static DataVaultSaveRequest CreateCustomerSaveRequest(string recordSource) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultSaveRequest(
        new DateTimeOffset(2026, 5, 4, 9, 0, 0, TimeSpan.Zero),
        recordSource,
        [new DataVaultHubSaveOperation(customer, [new("Customer Id", "C-100")])],
        []);
  }

  private static string InvokeSqlServerCommandTextFactory(string methodName, params object[] arguments) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = GetSqlServerStrategyMethod(strategyType!, methodName, arguments);

    Assert.NotNull(method);

    return Assert.IsType<string>(method.Invoke(null, arguments));
  }

  private static MethodInfo? GetSqlServerStrategyMethod(Type strategyType, string methodName, object?[] arguments) {
    return strategyType
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .SingleOrDefault(method => {
          if (!string.Equals(method.Name, methodName, StringComparison.Ordinal)) {
            return false;
          }

          var parameters = method.GetParameters();
          return parameters.Length == arguments.Length &&
              parameters
                  .Zip(arguments)
                  .All(pair => pair.Second is null || pair.First.ParameterType.IsInstanceOfType(pair.Second));
        });
  }

  private static int CountOccurrences(string value, string searchValue) {
    var count = 0;
    var searchIndex = 0;

    while (true) {
      var matchIndex = value.IndexOf(searchValue, searchIndex, StringComparison.Ordinal);
      if (matchIndex < 0) {
        return count;
      }

      count++;
      searchIndex = matchIndex + searchValue.Length;
    }
  }

  private static IReadOnlyList<DataVaultSavedRecord> InvokeSqlServerSavedRecords(string methodName, params object[] arguments) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

    Assert.NotNull(method);

    var values = Assert.IsAssignableFrom<IEnumerable>(method.Invoke(null, arguments));

    return values
        .Cast<object>()
        .Select(value => {
          var savedRecordProperty = value.GetType().GetProperty("SavedRecord");

          Assert.NotNull(savedRecordProperty);

          return Assert.IsType<DataVaultSavedRecord>(savedRecordProperty.GetValue(value));
        })
        .ToArray();
  }

  private static bool InvokeSqlServerBooleanFactory(string methodName, params object?[] arguments) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

    Assert.NotNull(method);

    return Assert.IsType<bool>(method.Invoke(null, arguments));
  }

  private static void AssertSavedRecord(
      DataVaultSavedRecord record,
      DataVaultTableKind kind,
      string metadataName,
      string tableName) {
    Assert.Equal(kind, record.Kind);
    Assert.Equal(metadataName, record.MetadataName);
    Assert.Equal(tableName, record.TableName);
    Assert.False(string.IsNullOrWhiteSpace(record.HashKey));
  }

  private static void AssertProviderRegistration(
      Action<IServiceCollection> configure,
      bool expectProviderStrategy) {
    var services = new ServiceCollection();

    configure(services);

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.NotNull(provider.GetRequiredService<IDataVaultSaveService>());
    if (expectProviderStrategy) {
      Assert.NotEmpty(provider.GetServices<IDataVaultProviderSaveStrategy>());
    }
    else {
      Assert.Empty(provider.GetServices<IDataVaultProviderSaveStrategy>());
    }
  }

  private static bool InvokeSqlServerCanSaveProvider(string? providerName, bool hasPendingTrackedChanges) {
    var strategyType = typeof(DVaultSqlServerServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.SqlServerDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!.GetMethod("CanSaveProvider", BindingFlags.Static | BindingFlags.NonPublic);

    Assert.NotNull(method);

    return Assert.IsType<bool>(method.Invoke(null, [providerName, hasPendingTrackedChanges]));
  }

  private sealed record SatelliteDecisionCandidate(string HashDiff, DateTimeOffset LoadTimestamp);

  private sealed class TestStableHashService : IStableHashService {
    public string AlgorithmId => "test-sha256-v1";

    public StableHashDigest ComputeHash(string normalizedInput) {
      ArgumentNullException.ThrowIfNull(normalizedInput);

      return new StableHashDigest(AlgorithmId, new string('a', 64));
    }
  }

  private sealed class TestStableHashNormalizer : IStableHashNormalizer {
    public string NormalizeValue(object? value) {
      return value?.ToString() ?? string.Empty;
    }

    public string NormalizeFields(IEnumerable<KeyValuePair<string, object?>> fields) {
      ArgumentNullException.ThrowIfNull(fields);

      return string.Join(
          "\n",
          fields.Select(field => field.Key + "=" + NormalizeValue(field.Value)));
    }
  }
}
