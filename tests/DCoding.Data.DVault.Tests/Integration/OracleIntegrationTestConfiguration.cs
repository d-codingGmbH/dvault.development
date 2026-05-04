namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class OracleIntegrationTestConfiguration {
  public const string ConnectionStringEnvironmentVariable = "DVAULT_TEST_ORACLE_CONNECTION_STRING";
  public const string MissingConfigurationSkipMessage =
      "Oracle integration tests are skipped because local Oracle configuration is missing. " +
      "Set DVAULT_TEST_ORACLE_CONNECTION_STRING to opt in; Oracle database provisioning is external to DVault.";

  private OracleIntegrationTestConfiguration(string? connectionString) {
    ConnectionString = connectionString;
  }

  public string? ConnectionString { get; }

  public bool IsConfigured => ConnectionString is not null;

  public static OracleIntegrationTestConfiguration FromEnvironment() {
    return FromEnvironment(Environment.GetEnvironmentVariable);
  }

  internal static OracleIntegrationTestConfiguration FromEnvironment(Func<string, string?> getEnvironmentVariable) {
    ArgumentNullException.ThrowIfNull(getEnvironmentVariable);

    var connectionString = Normalize(getEnvironmentVariable(ConnectionStringEnvironmentVariable));

    return new OracleIntegrationTestConfiguration(connectionString);
  }

  private static string? Normalize(string? value) {
    if (string.IsNullOrWhiteSpace(value)) {
      return null;
    }

    return value.Trim();
  }
}
