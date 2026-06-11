using System.Data.Common;
using System.Globalization;

namespace DCoding.Data.DVault.Benchmarks;

internal static class DbConnectionAvailability {
  public static async Task<string?> TryOpenConnectionAsync(
      Func<string, DbConnection> createConnection,
      string connectionString,
      CancellationToken cancellationToken) {
    try {
      await using var connection = createConnection(connectionString);
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
      await connection.CloseAsync().ConfigureAwait(false);

      return null;
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      return BenchmarkProviderDiagnostics.NormalizeExceptionMessage(exception);
    }
  }
}
