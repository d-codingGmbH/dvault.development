using System.Data.Common;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed record BenchmarkExternalProviderDefinition(
    string ProviderName,
    string ConnectionStringEnvironmentVariable,
    string PackageName,
    Func<string, BenchmarkDatabaseProvider> CreateProvider,
    Func<bool> IsProviderDependencyAvailable,
    Func<string, CancellationToken, Task<string?>> TryOpenConnectionAsync);
