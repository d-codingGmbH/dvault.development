using DCoding.Data.DVault;
using DCoding.Data.DVault.Quickstarts.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var databasePath = Path.Combine(
    Path.GetTempPath(),
    "dvault-sqlite-quickstart-" + Guid.NewGuid().ToString("N") + ".db");
var connectionString = "Data Source=" + databasePath + ";Pooling=False";

var services = new ServiceCollection();
services.AddDVault(options => options
    .UseBinaryFirstProfile()
    .UseMetadataModel(QuickstartHistoryFlow.MetadataModel));
services.AddDVaultSqlite();
services.AddDbContext<QuickstartVaultContext>(
    options => options
        .UseSqlite(connectionString)
        .UseDataVaultMetadata());

using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
Console.WriteLine("SQLite database: " + databasePath);
await QuickstartHistoryFlow.RunAsync(serviceProvider, "SQLite").ConfigureAwait(false);
