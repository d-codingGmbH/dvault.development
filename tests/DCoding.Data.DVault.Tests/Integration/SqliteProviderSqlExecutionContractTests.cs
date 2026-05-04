using DCoding.Data.DVault;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class SqliteProviderSqlExecutionContractTests {
  [Fact]
  public async Task OptimizedSqlitePathBindsCommandParameters() {
    using var fixture = new SqliteProviderSqlExecutionContractFixture();

    await fixture.Contract.PersistsValuesThroughParametersAsync();
  }

  [Fact]
  public async Task OptimizedSqlitePathParticipatesInCurrentTransaction() {
    using var fixture = new SqliteProviderSqlExecutionContractFixture();

    await fixture.Contract.ParticipatesInCurrentTransactionAsync();
  }

  [Fact]
  public async Task OptimizedSqlitePathPropagatesCancellationToken() {
    using var fixture = new SqliteProviderSqlExecutionContractFixture();

    await fixture.Contract.PropagatesCancellationTokenAsync();
  }

  [Fact]
  public async Task OptimizedSqlitePathDeclinesPendingTrackedChanges() {
    using var fixture = new SqliteProviderSqlExecutionContractFixture();

    await fixture.Contract.DeclinesUnsupportedTrackedChangesAsync();
  }

  private sealed class SqliteProviderSqlExecutionContractFixture : IDisposable {
    private readonly SqliteTestDatabase _database;
    private readonly DbContextOptions<SqliteProviderSqlExecutionContractContext> _options;
    private readonly ServiceProvider _serviceProvider;

    public SqliteProviderSqlExecutionContractFixture() {
      _database = SqliteTestDatabase.CreateTemporaryFile();
      _options = new DbContextOptionsBuilder<SqliteProviderSqlExecutionContractContext>()
          .UseSqlite("Data Source=" + Assert.IsType<string>(_database.DatabasePath) + ";Pooling=False")
          .Options;

      var services = new ServiceCollection();
      services.AddDVaultSqlite();

      _serviceProvider = services.BuildServiceProvider(validateScopes: true);

      Contract = new ProviderSqlExecutionContract<SqliteProviderSqlExecutionContractContext>(
          CreateContext,
          _serviceProvider.GetRequiredService<IDataVaultSaveService>(),
          Assert.Single(_serviceProvider.GetServices<IDataVaultProviderSaveStrategy>()),
          context => context.Add(new ProviderSqlExecutionContractTrackedEntity {
            Name = "pending-change",
          }));
    }

    public ProviderSqlExecutionContract<SqliteProviderSqlExecutionContractContext> Contract { get; }

    public void Dispose() {
      _serviceProvider.Dispose();
      _database.Dispose();
    }

    private SqliteProviderSqlExecutionContractContext CreateContext() {
      return new SqliteProviderSqlExecutionContractContext(_options);
    }
  }

  private sealed class SqliteProviderSqlExecutionContractContext(
      DbContextOptions<SqliteProviderSqlExecutionContractContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      ProviderSqlExecutionContract.ApplyModel(modelBuilder);
    }
  }
}
