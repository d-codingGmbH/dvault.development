namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class SqlServerIntegrationTestConfiguration {
  public const string ConnectionStringEnvironmentVariable = "DVAULT_TEST_SQLSERVER_CONNECTION_STRING";
  public const string MissingConfigurationSkipMessage =
      "SQL Server integration tests are skipped because local SQL Server configuration is missing. " +
      "Set DVAULT_TEST_SQLSERVER_CONNECTION_STRING to opt in; database provisioning is external to DVault.";

  private SqlServerIntegrationTestConfiguration(string? connectionString) {
    ConnectionString = connectionString;
  }

  public string? ConnectionString { get; }

  public bool IsConfigured => ConnectionString is not null;

  public static SqlServerIntegrationTestConfiguration FromEnvironment() {
    return FromEnvironment(Environment.GetEnvironmentVariable);
  }

  internal static SqlServerIntegrationTestConfiguration FromEnvironment(Func<string, string?> getEnvironmentVariable) {
    ArgumentNullException.ThrowIfNull(getEnvironmentVariable);

    var connectionString = Normalize(getEnvironmentVariable(ConnectionStringEnvironmentVariable));

    return new SqlServerIntegrationTestConfiguration(connectionString);
  }

  private static string? Normalize(string? value) {
    if (string.IsNullOrWhiteSpace(value)) {
      return null;
    }

    return value.Trim();
  }
}
