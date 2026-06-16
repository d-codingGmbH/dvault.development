using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal abstract class SharedExternalBenchmarkDatabase : IBenchmarkDatabase {
  private static readonly string[] ProducedTableNames = [
      "BridgeSalesRegionHierarchy",
      "PitCustomerProfileStatus",
      "SatOrderProductFulfillment",
      "SatCustomerStatus",
      "SatCustomerStatu",
      "SatCustomerProfile",
      "LinkSalesRegionParentChild",
      "LinkOrderProduct",
      "HubSalesRegion",
      "HubOrder",
      "HubProduct",
      "HubCustomer",
      "CustomerProfileBulkHistory",
  ];

  public abstract DbContextOptions<TContext> CreateOptions<TContext>()
      where TContext : DbContext;

  public virtual Task InitializeAsync(DbContext context, CancellationToken cancellationToken) {
    return CleanupAsync(context, cancellationToken);
  }

  public virtual Task EnsureCreatedAsync(DbContext context, CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(context);

    return context.Database.EnsureCreatedAsync(cancellationToken);
  }

  public abstract Task CleanupAsync(DbContext context, CancellationToken cancellationToken);

  public void Dispose() {
    NpgsqlReflection.ClearAllPools();
  }

  protected static IReadOnlyList<string> GetProducedTableNames() {
    return ProducedTableNames;
  }
}
