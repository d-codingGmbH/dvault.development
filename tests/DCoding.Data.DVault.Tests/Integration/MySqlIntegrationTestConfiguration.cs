namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class MySqlIntegrationTestConfiguration {
  public const string ConnectionStringEnvironmentVariable = "DVAULT_TEST_MYSQL_CONNECTION_STRING";
  public const string MissingConfigurationSkipMessage =
      "MySQL integration tests are skipped because local MySQL configuration is missing. " +
      "Set DVAULT_TEST_MYSQL_CONNECTION_STRING to opt in; Docker and database provisioning are external to DVault.";

  private MySqlIntegrationTestConfiguration(string? connectionString) {
    ConnectionString = connectionString;
  }

  public string? ConnectionString { get; }

  public bool IsConfigured => ConnectionString is not null;

  public static MySqlIntegrationTestConfiguration FromEnvironment() {
    return FromEnvironment(Environment.GetEnvironmentVariable);
  }

  internal static MySqlIntegrationTestConfiguration FromEnvironment(Func<string, string?> getEnvironmentVariable) {
    ArgumentNullException.ThrowIfNull(getEnvironmentVariable);

    var connectionString = Normalize(getEnvironmentVariable(ConnectionStringEnvironmentVariable));

    return new MySqlIntegrationTestConfiguration(connectionString);
  }

  private static string? Normalize(string? value) {
    if (string.IsNullOrWhiteSpace(value)) {
      return null;
    }

    return value.Trim();
  }
}
