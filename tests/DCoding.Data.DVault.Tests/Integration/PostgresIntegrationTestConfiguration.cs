namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class PostgresIntegrationTestConfiguration {
  public const string ConnectionStringEnvironmentVariable = "DVAULT_TEST_POSTGRES_CONNECTION_STRING";
  public const string MissingConfigurationSkipMessage =
      "Postgres integration tests are skipped because local Postgres configuration is missing. " +
      "Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to opt in; Docker and database provisioning are external to DVault.";

  private PostgresIntegrationTestConfiguration(string? connectionString) {
    ConnectionString = connectionString;
  }

  public string? ConnectionString { get; }

  public bool IsConfigured => ConnectionString is not null;

  public static PostgresIntegrationTestConfiguration FromEnvironment() {
    return FromEnvironment(Environment.GetEnvironmentVariable);
  }

  internal static PostgresIntegrationTestConfiguration FromEnvironment(Func<string, string?> getEnvironmentVariable) {
    ArgumentNullException.ThrowIfNull(getEnvironmentVariable);

    var connectionString = Normalize(getEnvironmentVariable(ConnectionStringEnvironmentVariable));

    return new PostgresIntegrationTestConfiguration(connectionString);
  }

  private static string? Normalize(string? value) {
    if (string.IsNullOrWhiteSpace(value)) {
      return null;
    }

    return value.Trim();
  }
}
