using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class OracleProviderOptimizationTests {
  private const string OracleProviderName = "Oracle.EntityFrameworkCore";
  private static readonly DateTimeOffset LoadTimestamp =
      new(2026, 5, 25, 0, 0, 0, TimeSpan.Zero);

  [Fact]
  public void OracleBulkPathRetainsDirectBatchingForEligibleOrdinaryBatch() {
    var path = InvokeOracleBulkPath(
        OracleProviderName,
        hasPendingTrackedChanges: false,
        CreateHubBatch(operationCount: 50));

    Assert.Equal("DirectOracleBatching", path);
  }

  [Fact]
  public void OracleBulkPathRetainsDirectBatchingForEligibleHubLinkAndOrdinarySatelliteBatch() {
    var path = InvokeOracleBulkPath(
        OracleProviderName,
        hasPendingTrackedChanges: false,
        CreateMixedHubLinkAndSatelliteBatch());

    Assert.Equal("DirectOracleBatching", path);
  }

  [Fact]
  public void OracleStagedBulkDecisionDeclinesStagingWithoutMeasuredDirectPathWin() {
    var decision = InvokeOracleStagedBulkDecision(
        OracleProviderName,
        hasPendingTrackedChanges: false,
        CreateMixedHubLinkAndSatelliteBatch());

    Assert.Equal("DirectOracleBatching", decision.SelectedPath);
    Assert.False(decision.UsesStagedBulk);
    Assert.Equal("not-selected-no-measured-win", decision.Reason);
  }

  [Fact]
  public void OracleBulkPathFallsBackForUnsupportedProviderOrDirtyContext() {
    var requestBatch = CreateHubBatch(operationCount: 50);

    Assert.Equal(
        "ProviderNeutralFallback",
        InvokeOracleBulkPath(
            "Microsoft.EntityFrameworkCore.Sqlite",
            hasPendingTrackedChanges: false,
            requestBatch));
    Assert.Equal(
        "ProviderNeutralFallback",
        InvokeOracleBulkPath(
            OracleProviderName,
            hasPendingTrackedChanges: true,
            requestBatch));
  }

  [Fact]
  public void OracleBulkPathFallsBackForUnsupportedBatchShapes() {
    Assert.Equal(
        "ProviderNeutralFallback",
        InvokeOracleBulkPath(
            OracleProviderName,
            hasPendingTrackedChanges: false,
            CreateHubBatch(operationCount: 49)));
    Assert.Equal(
        "ProviderNeutralFallback",
        InvokeOracleBulkPath(
            OracleProviderName,
            hasPendingTrackedChanges: false,
            CreateSatelliteBatch(operationCount: 10001, multiActive: false)));
    Assert.Equal(
        "ProviderNeutralFallback",
        InvokeOracleBulkPath(
            OracleProviderName,
            hasPendingTrackedChanges: false,
            CreateSatelliteBatch(operationCount: 50, multiActive: true)));
  }

  [Fact]
  public void OracleUniqueInsertSqlUsesSetBasedExistenceDetection() {
    var commandText = InvokeOracleCommandTextFactory(
        "CreateOracleUniqueInsertCommandText",
        "HubCustomer",
        new[] { "CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId" },
        "CustomerHashKey",
        3);

    Assert.Contains("INSERT INTO \"HubCustomer\"", commandText, StringComparison.Ordinal);
    Assert.Contains("UNION ALL SELECT :p5", commandText, StringComparison.Ordinal);
    Assert.Contains("UNION ALL SELECT :p10", commandText, StringComparison.Ordinal);
    Assert.Contains(
        "ROW_NUMBER() OVER (PARTITION BY \"source\".\"CustomerHashKey\" ORDER BY \"source\".\"__dvault_ordinal\")",
        commandText,
        StringComparison.Ordinal);
    Assert.Contains("WHERE \"ranked\".\"__dvault_row_number\" = 1", commandText, StringComparison.Ordinal);
    Assert.Contains("WHERE NOT EXISTS (SELECT 1 FROM \"HubCustomer\" \"target\"", commandText, StringComparison.Ordinal);
    Assert.Equal(1, CountOccurrences(commandText, "NOT EXISTS"));
  }

  [Fact]
  public void OracleInsertAllSqlBatchesSatelliteRows() {
    var commandText = InvokeOracleCommandTextFactory(
        "CreateOracleInsertAllCommandText",
        "SatCustomerProfile",
        new[] { "CustomerHashKey", "HashDiff", "LoadTimestamp" },
        2);

    Assert.StartsWith("INSERT ALL INTO \"SatCustomerProfile\"", commandText, StringComparison.Ordinal);
    Assert.Contains("VALUES (:p0, :p1, :p2)", commandText, StringComparison.Ordinal);
    Assert.Contains("VALUES (:p3, :p4, :p5)", commandText, StringComparison.Ordinal);
    Assert.EndsWith("SELECT 1 FROM DUAL", commandText, StringComparison.Ordinal);
    Assert.Equal(2, CountOccurrences(commandText, " INTO "));
  }

  [Fact]
  public void OracleArrayInsertSqlUsesSingleParameterizedRowShape() {
    var commandText = InvokeOracleCommandTextFactory(
        "CreateOracleArrayInsertCommandText",
        "SatCustomerProfile",
        new[] { "CustomerHashKey", "HashDiff", "LoadTimestamp" });

    Assert.Equal(
        "INSERT INTO \"SatCustomerProfile\" (\"CustomerHashKey\", \"HashDiff\", \"LoadTimestamp\") VALUES (:p0, :p1, :p2)",
        commandText);
  }

  [Fact]
  public void OracleArrayUniqueInsertSqlKeepsProviderSideExistenceGuard() {
    var commandText = InvokeOracleCommandTextFactory(
        "CreateOracleArrayUniqueInsertCommandText",
        "HubCustomer",
        new[] { "CustomerHashKey", "LoadTimestamp", "RecordSource", "CustomerId" },
        "CustomerHashKey");

    Assert.Equal(
        "INSERT INTO \"HubCustomer\" (\"CustomerHashKey\", \"LoadTimestamp\", \"RecordSource\", \"CustomerId\") " +
        "SELECT :p0, :p1, :p2, :p3 FROM DUAL WHERE NOT EXISTS " +
        "(SELECT 1 FROM \"HubCustomer\" WHERE \"CustomerHashKey\" = :p4)",
        commandText);
  }

  [Fact]
  public void OracleLatestSatelliteReadSqlUsesOracleBindParametersAndRowNumberSelection() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(profile);
    var selectedColumns = new[]
    {
        projection.ParentHashKeyColumnName,
        projection.HashDiffColumnName,
        projection.LoadTimestampColumnName,
        projection.RecordSourceColumnName,
    }
        .Concat(projection.PayloadColumnNames)
        .ToArray();
    using var dbContext = new DbContext(new DbContextOptionsBuilder().Options);

    var commandText = new OracleDataVaultReadStrategy().CreateLatestRowsCommandText(
        dbContext,
        projection,
        selectedColumns,
        parentHashKeyCount: 2,
        hasAsOf: true);

    Assert.Contains(
        "ROW_NUMBER() OVER (PARTITION BY \"target\".\"" + projection.ParentHashKeyColumnName + "\" ORDER BY \"target\".\"" + projection.LoadTimestampColumnName + "\" DESC) \"__dvault_row_number\"",
        commandText,
        StringComparison.Ordinal);
    Assert.Contains(
        "FROM \"" + projection.TableName + "\" \"target\"",
        commandText,
        StringComparison.Ordinal);
    Assert.Contains(
        "\"target\".\"" + projection.ParentHashKeyColumnName + "\" IN (:p0, :p1)",
        commandText,
        StringComparison.Ordinal);
    Assert.Contains(
        "\"target\".\"" + projection.LoadTimestampColumnName + "\" <= :p2",
        commandText,
        StringComparison.Ordinal);
    Assert.Contains(
        "WHERE \"__dvault_row_number\" = 1 ORDER BY \"" + projection.ParentHashKeyColumnName + "\"",
        commandText,
        StringComparison.Ordinal);
    Assert.DoesNotContain(" AS \"target\"", commandText, StringComparison.Ordinal);
    Assert.DoesNotContain(" AS \"ranked\"", commandText, StringComparison.Ordinal);
  }

  [Fact]
  public void OracleReadCommandsPrefetchLobsAndUseLargerFetchBuffer() {
    var command = new OracleLikeReadCommand();
    var configureMethod = typeof(OracleDataVaultReadStrategy).GetMethod(
        "ConfigureReadCommand",
        BindingFlags.Instance | BindingFlags.NonPublic);

    configureMethod!.Invoke(new OracleDataVaultReadStrategy(), [command]);

    Assert.Equal(-1, command.InitialLOBFetchSize);
    Assert.Equal(1024L * 1024L, command.FetchSize);
  }

  [Fact]
  public void OracleChunkSizeUsesLargerCapWhenArrayBindingIsAvailable() {
    var chunkSize = InvokeOracleIntFactory(
        "CalculateOracleInsertChunkSize",
        6,
        true);

    Assert.Equal(5000, chunkSize);
  }

  [Fact]
  public void OracleChunkSizeKeepsInsertAllCapWithoutArrayBinding() {
    var chunkSize = InvokeOracleIntFactory(
        "CalculateOracleInsertChunkSize",
        6,
        false);

    Assert.Equal(250, chunkSize);
  }

  private static string InvokeOracleCommandTextFactory(string methodName, params object[] arguments) {
    var strategyType = typeof(DVaultOracleServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.OracleDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .Single(method => string.Equals(method.Name, methodName, StringComparison.Ordinal));

    return Assert.IsType<string>(method.Invoke(null, arguments));
  }

  private static int InvokeOracleIntFactory(string methodName, params object[] arguments) {
    var strategyType = typeof(DVaultOracleServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.OracleDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .Single(method => string.Equals(method.Name, methodName, StringComparison.Ordinal));

    return Assert.IsType<int>(method.Invoke(null, arguments));
  }

  private static string InvokeOracleBulkPath(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    var strategyType = typeof(DVaultOracleServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.OracleDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .Single(method => string.Equals(method.Name, "SelectOracleBulkSavePath", StringComparison.Ordinal));
    var result = method.Invoke(null, [providerName, hasPendingTrackedChanges, requests]);

    Assert.NotNull(result);

    return result.ToString()!;
  }

  private static OracleStagedBulkDecisionResult InvokeOracleStagedBulkDecision(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    var strategyType = typeof(DVaultOracleServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.OracleDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .Single(method => string.Equals(method.Name, "SelectOracleStagedBulkDecision", StringComparison.Ordinal));
    var result = method.Invoke(null, [providerName, hasPendingTrackedChanges, requests]);

    Assert.NotNull(result);

    return new OracleStagedBulkDecisionResult(
        ReadProperty(result, "SelectedPath").ToString()!,
        Assert.IsType<bool>(ReadProperty(result, "UsesStagedBulk")),
        Assert.IsType<string>(ReadProperty(result, "Reason")));
  }

  private static object ReadProperty(object instance, string propertyName) {
    var property = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

    Assert.NotNull(property);

    return property.GetValue(instance) ??
        throw new InvalidOperationException("Oracle staged bulk decision property '" + propertyName + "' was null.");
  }

  private static IReadOnlyList<DataVaultSaveRequest> CreateHubBatch(int operationCount) {
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return
    [
        new DataVaultSaveRequest(
            LoadTimestamp,
            "oracle-boundary",
            Enumerable.Range(0, operationCount)
                .Select(index => new DataVaultHubSaveOperation(
                    hub,
                    [new("Customer Id", "C-" + index.ToString("00000", CultureInfo.InvariantCulture))]))
                .ToArray(),
        []),
    ];
  }

  private static IReadOnlyList<DataVaultSaveRequest> CreateMixedHubLinkAndSatelliteBatch() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var contact = new DataVaultSatelliteMetadata("Contact", customer.ToReference(), ["Email Address"]);
    var customerIds = Enumerable.Range(0, 20)
        .Select(index => "C-" + index.ToString("00000", CultureInfo.InvariantCulture))
        .ToArray();
    var orderIds = Enumerable.Range(0, 20)
        .Select(index => "O-" + index.ToString("00000", CultureInfo.InvariantCulture))
        .ToArray();
    var customerHashKeys = Enumerable.Range(0, 20)
        .Select(index => "customer-hash-" + index.ToString("00000", CultureInfo.InvariantCulture))
        .ToArray();
    var orderHashKeys = Enumerable.Range(0, 20)
        .Select(index => "order-hash-" + index.ToString("00000", CultureInfo.InvariantCulture))
        .ToArray();

    return
    [
        new DataVaultSaveRequest(
            LoadTimestamp,
            "oracle-boundary",
            customerIds
                .Select(customerId => new DataVaultHubSaveOperation(customer, [new("Customer Id", customerId)]))
                .Concat(orderIds.Select(orderId => new DataVaultHubSaveOperation(order, [new("Order Id", orderId)])))
                .ToArray(),
            Enumerable.Range(0, 20)
                .Select(index => new DataVaultLinkSaveOperation(
                    customerOrder,
                    [
                        new("Customer", customerHashKeys[index]),
                        new("Order", orderHashKeys[index]),
                    ]))
                .ToArray(),
            Enumerable.Range(0, 3)
                .Select(index => new DataVaultSatelliteSaveOperation(
                    contact,
                    customerHashKeys[index],
                    [new("Email Address", "customer-" + index.ToString("00000", CultureInfo.InvariantCulture) + "@example.test")],
                    "contact-hash-" + index.ToString("00000", CultureInfo.InvariantCulture)))
                .ToArray()),
    ];
  }

  private static IReadOnlyList<DataVaultSaveRequest> CreateSatelliteBatch(
      int operationCount,
      bool multiActive) {
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var satellite = multiActive
        ? new DataVaultSatelliteMetadata("Profile", hub.ToReference(), ["Status"], ["Region"])
        : new DataVaultSatelliteMetadata("Profile", hub.ToReference(), ["Status"]);
    var satelliteOperations = Enumerable.Range(0, operationCount)
        .Select(index => CreateSatelliteOperation(satellite, index, multiActive))
        .ToArray();

    return
    [
        new DataVaultSaveRequest(
            LoadTimestamp,
            "oracle-boundary",
            [],
            [],
            satelliteOperations),
    ];
  }

  private static DataVaultSatelliteSaveOperation CreateSatelliteOperation(
      DataVaultSatelliteMetadata satellite,
      int index,
      bool multiActive) {
    var parentHashKey = "parent-" + index.ToString("00000", CultureInfo.InvariantCulture);
    var payloadValues = new[]
    {
        new KeyValuePair<string, string>("Status", "Active"),
    };
    var hashDiff = "hash-" + index.ToString("00000", CultureInfo.InvariantCulture);

    if (multiActive) {
      return new DataVaultSatelliteSaveOperation(
          satellite,
          parentHashKey,
          [new("Region", "R-" + index.ToString("00000", CultureInfo.InvariantCulture))],
          payloadValues,
          hashDiff);
    }

    return new DataVaultSatelliteSaveOperation(
        satellite,
        parentHashKey,
        payloadValues,
        hashDiff);
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

  private sealed class OracleLikeReadCommand : System.Data.Common.DbCommand {
    private readonly Microsoft.Data.Sqlite.SqliteCommand _innerCommand = new();

    public int InitialLOBFetchSize { get; set; }

    public long FetchSize { get; set; }

    [System.Diagnostics.CodeAnalysis.AllowNull]
    public override string CommandText { get; set; } = string.Empty;

    public override int CommandTimeout { get; set; }

    public override System.Data.CommandType CommandType { get; set; }

    public override bool DesignTimeVisible { get; set; }

    public override System.Data.UpdateRowSource UpdatedRowSource { get; set; }

    protected override System.Data.Common.DbConnection? DbConnection { get; set; }

    protected override System.Data.Common.DbParameterCollection DbParameterCollection => _innerCommand.Parameters;

    protected override System.Data.Common.DbTransaction? DbTransaction { get; set; }

    public override void Cancel() {
    }

    public override int ExecuteNonQuery() {
      throw new NotSupportedException();
    }

    public override object? ExecuteScalar() {
      throw new NotSupportedException();
    }

    public override void Prepare() {
    }

    protected override System.Data.Common.DbParameter CreateDbParameter() {
      return _innerCommand.CreateParameter();
    }

    protected override System.Data.Common.DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior) {
      throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing) {
      if (disposing) {
        _innerCommand.Dispose();
      }

      base.Dispose(disposing);
    }
  }

  private sealed record OracleStagedBulkDecisionResult(
      string SelectedPath,
      bool UsesStagedBulk,
      string Reason);
}
