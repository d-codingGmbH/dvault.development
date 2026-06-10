namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class Db2IntegrationTestConfiguration {
  public const string ConnectionStringEnvironmentVariable = "DVAULT_TEST_DB2_CONNECTION_STRING";
  public const string MissingConfigurationSkipMessage =
      "DB2 integration tests are skipped because local DB2 configuration is missing. " +
      "Set DVAULT_TEST_DB2_CONNECTION_STRING to opt in; DB2 database provisioning is external to DVault.";

  private Db2IntegrationTestConfiguration(string? connectionString) {
    ConnectionString = connectionString;
  }

  public string? ConnectionString { get; }

  public bool IsConfigured => ConnectionString is not null;

  public static Db2IntegrationTestConfiguration FromEnvironment() {
    return FromEnvironment(Environment.GetEnvironmentVariable);
  }

  internal static Db2IntegrationTestConfiguration FromEnvironment(Func<string, string?> getEnvironmentVariable) {
    ArgumentNullException.ThrowIfNull(getEnvironmentVariable);

    var connectionString = Normalize(getEnvironmentVariable(ConnectionStringEnvironmentVariable));

    return new Db2IntegrationTestConfiguration(connectionString);
  }

  private static string? Normalize(string? value) {
    if (string.IsNullOrWhiteSpace(value)) {
      return null;
    }

    return value.Trim();
  }
}
