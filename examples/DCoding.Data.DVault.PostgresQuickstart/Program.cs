using DCoding.Data.DVault;
using DCoding.Data.DVault.Quickstarts.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

const string ConnectionStringEnvironmentVariable = "DVAULT_TEST_POSTGRES_CONNECTION_STRING";
const string MissingConnectionStringMessage =
    "Skipping PostgreSQL quickstart. Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to a developer-managed PostgreSQL connection string and rerun this example.";

var connectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariable);
if (string.IsNullOrWhiteSpace(connectionString)) {
  Console.WriteLine(MissingConnectionStringMessage);
  return;
}

var services = new ServiceCollection();
services.AddDVault(options => options
    .UseBinaryFirstProfile()
    .UseMetadataModel(QuickstartHistoryFlow.MetadataModel));
services.AddDVaultPostgres();
services.AddDbContext<QuickstartVaultContext>(
    options => options
        .UseNpgsql(connectionString)
        .UseDataVaultMetadata());

using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
await QuickstartHistoryFlow.RunAsync(serviceProvider, "PostgreSQL").ConfigureAwait(false);
