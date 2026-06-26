using DCoding.Data.DVault;
using DCoding.Data.DVault.Quickstarts.Shared;
using DCoding.Data.DVault.Privacy;
using DCoding.Data.DVault.SqliteQuickstart;
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
services.AddDVaultPrivacy(options => options
    .RegisterEncryptedPayloadAlias(SqlitePrivacyQuickstartFlow.CustomerProfileEmailEncryptedPayloadAlias)
    .UseCallerOwnedKeyProvider(new SqliteDemoEncryptedPayloadKeyProvider()));
services.AddDVaultSqlite();
services.AddDbContext<SqliteQuickstartVaultContext>(
    options => options
        .UseSqlite(connectionString)
        .UseDataVaultMetadata());
services.AddScoped<QuickstartVaultContext>(
    provider => provider.GetRequiredService<SqliteQuickstartVaultContext>());

using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
Console.WriteLine("SQLite database: " + databasePath);
await QuickstartHistoryFlow.RunAsync(serviceProvider, "SQLite").ConfigureAwait(false);
await SqlitePrivacyQuickstartFlow.RunAsync(serviceProvider).ConfigureAwait(false);
