using System.Data.Common;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.Benchmarks;

internal static class OracleBenchmarkReflection {
  private const string ProviderAssemblyName = "Oracle.EntityFrameworkCore";
  private const string ConnectionTypeName = "Oracle.ManagedDataAccess.Client.OracleConnection, Oracle.ManagedDataAccess";

  public static bool IsProviderDependencyAvailable() {
    return GetConnectionType() is not null && LoadProviderAssembly() is not null;
  }

  public static DbConnection CreateConnection(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var connectionType = GetConnectionType()
        ?? throw new InvalidOperationException("Oracle.ManagedDataAccess is not available to the benchmark process.");

    return (DbConnection)Activator.CreateInstance(connectionType, connectionString)!;
  }

  public static Task<string?> TryOpenConnectionAsync(string connectionString, CancellationToken cancellationToken) {
    return DbConnectionAvailability.TryOpenConnectionAsync(CreateConnection, connectionString, cancellationToken);
  }

  public static void UseOracle(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var providerAssembly = LoadProviderAssembly()
        ?? throw new InvalidOperationException("Oracle.EntityFrameworkCore is not available to the benchmark process.");
    var method = providerAssembly
        .GetTypes()
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        .SingleOrDefault(method => {
          if (method.Name != "UseOracle") {
            return false;
          }

          var parameters = method.GetParameters();

          return parameters.Length == 3 &&
              parameters[0].ParameterType == typeof(DbContextOptionsBuilder) &&
              parameters[1].ParameterType == typeof(string);
        });

    if (method is null) {
      throw new InvalidOperationException("The Oracle EF Core provider does not expose the expected UseOracle options extension.");
    }

    InvokeProviderMethod(method, [optionsBuilder, connectionString, null]);
  }

  private static Assembly? LoadProviderAssembly() {
    try {
      return Assembly.Load(new AssemblyName(ProviderAssemblyName));
    }
    catch (FileNotFoundException) {
      return null;
    }
  }

  private static Type? GetConnectionType() {
    var connectionType = Type.GetType(ConnectionTypeName, throwOnError: false);
    if (connectionType is null || !typeof(DbConnection).IsAssignableFrom(connectionType)) {
      return null;
    }

    return connectionType;
  }

  private static object? InvokeProviderMethod(MethodInfo method, object?[] arguments) {
    try {
      return method.Invoke(null, arguments);
    }
    catch (TargetInvocationException exception) when (exception.InnerException is not null) {
      ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
      throw;
    }
  }
}
