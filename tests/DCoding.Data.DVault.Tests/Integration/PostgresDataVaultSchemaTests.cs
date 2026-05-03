using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.PostgresProvider)]
public sealed class PostgresDataVaultSchemaTests {
  [Fact]
  public async Task ApplyDataVaultMetadataCreatesExpectedPostgresSchemaWhenConfigured() {
    var configuration = PostgresIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(PostgresIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var schemaName = "dvault_test_" + Guid.NewGuid().ToString("N");
    var options = CreatePostgresOptions(configuration.ConnectionString!);

    await using var context = new TranslatedDataVaultSchemaContext(options, schemaName);
    await context.Database.ExecuteSqlRawAsync("CREATE SCHEMA " + QuoteIdentifier(schemaName) + ";");

    try {
      await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());

      var tableNames = await context.Database
          .SqlQueryRaw<string>(
              "SELECT table_name AS \"Value\" " +
              "FROM information_schema.tables " +
              "WHERE table_schema = " + SqlLiteral(schemaName) + " " +
              "ORDER BY table_name;")
          .ToListAsync();

      Assert.Equal(
          ["HubCustomer", "HubOrder", "LinkCustomerOrder", "SatCustomerContact", "SatCustomerOrderState"],
          tableNames);
    }
    finally {
      await context.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS " + QuoteIdentifier(schemaName) + " CASCADE;");
    }
  }

  private static DbContextOptions<TranslatedDataVaultSchemaContext> CreatePostgresOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<TranslatedDataVaultSchemaContext>();

    NpgsqlProviderReflection.UseNpgsql(optionsBuilder, connectionString);

    return optionsBuilder.Options;
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    return new DataVaultMetadataModel(
        [
            new DataVaultHubMetadata("Customer", ["Customer Id"]),
            new DataVaultHubMetadata("Order", ["Order Id"]),
        ],
        [
            new DataVaultLinkMetadata(
                "CustomerOrder",
                [DataVaultMetadataReference.Hub("Customer"), DataVaultMetadataReference.Hub("Order")]),
        ],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                DataVaultMetadataReference.Hub("Customer"),
                ["Email Address"]),
            new DataVaultSatelliteMetadata(
                "State",
                DataVaultMetadataReference.Link("CustomerOrder"),
                ["State Code"]),
        ]);
  }

  private static string QuoteIdentifier(string value) {
    return "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string SqlLiteral(string value) {
    return "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
  }

  private sealed class TranslatedDataVaultSchemaContext(
      DbContextOptions<TranslatedDataVaultSchemaContext> options,
      string schemaName) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.HasDefaultSchema(schemaName);
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());
    }
  }
}
