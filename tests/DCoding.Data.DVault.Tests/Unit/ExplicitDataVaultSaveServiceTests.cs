using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class ExplicitDataVaultSaveServiceTests {
  [Fact]
  public void AddDVaultProvidesDefaultExplicitSaveServiceWithoutSaveChangesInterceptor() {
    var services = new ServiceCollection();

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.NotNull(provider.GetRequiredService<IDataVaultSaveService>());
    Assert.Empty(provider.GetServices<ISaveChangesInterceptor>());
  }

  [Fact]
  public void AddDVaultPreservesCallerExplicitSaveServiceOverride() {
    var replacement = new ReplacementDataVaultSaveService();
    var services = new ServiceCollection();
    services.AddSingleton<IDataVaultSaveService>(replacement);

    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);

    Assert.Same(replacement, provider.GetRequiredService<IDataVaultSaveService>());
  }

  [Fact]
  public void SaveRequestKeepsExplicitMetadataBoundaryDeterministic() {
    var suppliedTimestamp = new DateTimeOffset(2026, 4, 29, 12, 15, 0, TimeSpan.FromHours(2));
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var request = new DataVaultSaveRequest(
        suppliedTimestamp,
        "crm-import",
        [new DataVaultHubSaveOperation(hub, [new("Customer Id", "C-100")])],
        []);

    Assert.Equal(new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero), request.LoadTimestamp);
    Assert.Equal("crm-import", request.RecordSource);
    Assert.Single(request.HubOperations);
    Assert.Empty(request.LinkOperations);
  }

  [Fact]
  public void BulkSaveRequestKeepsCallerSuppliedOrder() {
    var first = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero),
        "first-source",
        [],
        []);
    var second = new DataVaultSaveRequest(
        new DateTimeOffset(2026, 4, 29, 11, 15, 0, TimeSpan.Zero),
        "second-source",
        [],
        []);
    var bulkRequest = new DataVaultBulkSaveRequest([first, second]);

    Assert.Equal([first, second], bulkRequest.Requests);
  }

  [Fact]
  public void BulkSaveRequestRejectsNullRequests() {
    Assert.Throws<ArgumentNullException>(() => new DataVaultBulkSaveRequest(null!));
    Assert.Throws<ArgumentException>(() => new DataVaultBulkSaveRequest([null!]));
  }

  [Fact]
  public void SaveOperationsRequireNamedValuesWithoutDuplicates() {
    var hub = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new("Customer Id", "C-100"), new("Customer Id", "C-101")]));
    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new(" ", "C-100")]));
    Assert.Throws<ArgumentException>(() => new DataVaultHubSaveOperation(hub, [new("Customer Id", null!)]));
  }

  private sealed class ReplacementDataVaultSaveService : IDataVaultSaveService {
    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }

    public Task<DataVaultSaveResult> SaveAsync(
        DbContext dbContext,
        DataVaultBulkSaveRequest request,
        CancellationToken cancellationToken = default) {
      throw new NotSupportedException();
    }
  }
}
