using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal static class NpgsqlReflection {
  private const string NpgsqlOptionsExtensionTypeName =
      "Microsoft.EntityFrameworkCore.NpgsqlDbContextOptionsBuilderExtensions, Npgsql.EntityFrameworkCore.PostgreSQL";
  private const string NpgsqlConnectionTypeName = "Npgsql.NpgsqlConnection, Npgsql";

  public static bool IsProviderDependencyAvailable() {
    return GetUseNpgsqlMethod() is not null && GetConnectionType() is not null;
  }

  public static DbConnection CreateConnection(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var connectionType = GetConnectionType();
    if (connectionType is null) {
      throw new InvalidOperationException("Npgsql is not available to the benchmark process.");
    }

    return (DbConnection)Activator.CreateInstance(connectionType, connectionString)!;
  }

  public static void UseNpgsql(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var method = GetUseNpgsqlMethod();
    if (method is null) {
      throw new InvalidOperationException("Npgsql.EntityFrameworkCore.PostgreSQL is not available to the benchmark process.");
    }

    var parameters = method.GetParameters();
    var arguments = new object?[parameters.Length];
    arguments[0] = optionsBuilder;
    arguments[1] = connectionString;

    method.Invoke(null, arguments);
  }

  public static void ClearAllPools() {
    var connectionType = GetConnectionType();
    var method = connectionType?.GetMethod(
        "ClearAllPools",
        BindingFlags.Public | BindingFlags.Static,
        Type.EmptyTypes);
    method?.Invoke(null, null);
  }

  private static Type? GetConnectionType() {
    var connectionType = Type.GetType(NpgsqlConnectionTypeName, throwOnError: false);
    if (connectionType is null || !typeof(DbConnection).IsAssignableFrom(connectionType)) {
      return null;
    }

    return connectionType;
  }

  private static MethodInfo? GetUseNpgsqlMethod() {
    var extensionType = Type.GetType(NpgsqlOptionsExtensionTypeName, throwOnError: false);
    if (extensionType is null) {
      return null;
    }

    return extensionType
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(method => string.Equals(method.Name, "UseNpgsql", StringComparison.Ordinal) && !method.IsGenericMethod)
        .Select(method => new {
          Method = method,
          Parameters = method.GetParameters(),
        })
        .Where(candidate =>
            candidate.Parameters.Length >= 2 &&
            candidate.Parameters[0].ParameterType.IsAssignableFrom(typeof(DbContextOptionsBuilder)) &&
            candidate.Parameters[1].ParameterType == typeof(string))
        .OrderBy(candidate => candidate.Parameters.Length)
        .Select(candidate => candidate.Method)
        .FirstOrDefault();
  }
}
