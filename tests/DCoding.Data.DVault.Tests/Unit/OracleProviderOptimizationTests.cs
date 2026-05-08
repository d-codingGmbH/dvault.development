using System.Reflection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class OracleProviderOptimizationTests {
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

  private static string InvokeOracleCommandTextFactory(string methodName, params object[] arguments) {
    var strategyType = typeof(DVaultOracleServiceCollectionExtensions).Assembly.GetType(
        "DCoding.Data.DVault.OracleDataVaultSaveStrategy",
        throwOnError: true);
    var method = strategyType!
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .Single(method => string.Equals(method.Name, methodName, StringComparison.Ordinal));

    return Assert.IsType<string>(method.Invoke(null, arguments));
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
}
