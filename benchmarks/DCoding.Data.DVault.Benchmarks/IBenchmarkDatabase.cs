using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal interface IBenchmarkDatabase : IDisposable {
  DbContextOptions<TContext> CreateOptions<TContext>()
      where TContext : DbContext;

  Task InitializeAsync(DbContext context, CancellationToken cancellationToken);

  Task CleanupAsync(DbContext context, CancellationToken cancellationToken);
}
