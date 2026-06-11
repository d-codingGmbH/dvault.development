using System.Data;
using System.Data.Common;
using System.Globalization;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides the built-in live database schema reader dispatch for the bounded Data Vault schema surface.
/// </summary>
public static class DataVaultLiveSchemaReader {
  internal const string SqliteProviderName = "Microsoft.EntityFrameworkCore.Sqlite";
  internal const string PostgresProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";
  internal const string SqlServerProviderName = "Microsoft.EntityFrameworkCore.SqlServer";
  internal const string OracleProviderName = "Oracle.EntityFrameworkCore";
  internal const string Db2ProviderName = "IBM.EntityFrameworkCore";
  internal const string MySqlProviderName = "MySql.EntityFrameworkCore";
  internal const string PomeloMySqlProviderName = "Pomelo.EntityFrameworkCore.MySql";
  private static readonly IDataVaultLiveSchemaReader SqliteReader = new SqliteDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader PostgresReader = new PostgresDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader SqlServerReader = new SqlServerDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader OracleReader = new OracleDataVaultLiveSchemaReader();
  private static readonly IDataVaultLiveSchemaReader Db2UnsupportedReader = new UnsupportedDataVaultLiveSchemaReader(Db2ProviderName);
  private static readonly IDataVaultLiveSchemaReader MySqlReader = new MySqlDataVaultLiveSchemaReader();
  private static readonly IReadOnlyDictionary<string, IDataVaultLiveSchemaReader> BuiltInReadersByProviderName =
      new Dictionary<string, IDataVaultLiveSchemaReader>(StringComparer.Ordinal) {
        [SqliteProviderName] = SqliteReader,
        [PostgresProviderName] = PostgresReader,
        [SqlServerProviderName] = SqlServerReader,
        [OracleProviderName] = OracleReader,
        [Db2ProviderName] = Db2UnsupportedReader,
        [MySqlProviderName] = MySqlReader,
        [PomeloMySqlProviderName] = MySqlReader,
      };

  /// <summary>
  /// Reads a live database schema snapshot for the supplied context using the built-in reader for the current provider.
  /// </summary>
  /// <param name="dbContext">The context whose provider, model, and connection identify the live schema to read.</param>
  /// <param name="cancellationToken">A token used to observe cancellation while reading schema metadata.</param>
  /// <returns>
  /// A classified live-schema read result. Recognized built-in providers return a snapshot when their database catalog is
  /// reachable; providers without a built-in reader return an unsupported-provider result instead of silently passing or
  /// throwing an unclassified failure.
  /// </returns>
  public static Task<DataVaultLiveSchemaReadResult> ReadAsync(
      DbContext dbContext,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var providerName = TryGetProviderName(dbContext);
    return providerName is not null && BuiltInReadersByProviderName.TryGetValue(providerName, out var reader)
        ? reader.ReadAsync(dbContext, cancellationToken)
        : Task.FromResult(DataVaultLiveSchemaReadResult.UnsupportedProvider(providerName));
  }

  internal static bool IsExplicitlyUnsupportedProviderName(string providerName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);

    return BuiltInReadersByProviderName.TryGetValue(providerName, out var reader) &&
        reader is UnsupportedDataVaultLiveSchemaReader;
  }

  private static string? TryGetProviderName(DbContext dbContext) {
    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
  }
}
