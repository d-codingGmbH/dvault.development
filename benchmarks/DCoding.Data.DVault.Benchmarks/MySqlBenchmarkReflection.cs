using System.Data.Common;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.Benchmarks;

internal static class MySqlBenchmarkReflection {
  private const string ProviderAssemblyName = "MySql.EntityFrameworkCore";
  private const string ConnectionTypeName = "MySql.Data.MySqlClient.MySqlConnection, MySql.Data";

  public static bool IsProviderDependencyAvailable() {
    return GetConnectionType() is not null && LoadProviderAssembly() is not null;
  }

  public static DbConnection CreateConnection(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var connectionType = GetConnectionType()
        ?? throw new InvalidOperationException("MySqlConnector is not available to the benchmark process.");

    return (DbConnection)Activator.CreateInstance(connectionType, connectionString)!;
  }

  public static Task<string?> TryOpenConnectionAsync(string connectionString, CancellationToken cancellationToken) {
    return DbConnectionAvailability.TryOpenConnectionAsync(CreateConnection, connectionString, cancellationToken);
  }

  public static void UseMySql(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var providerAssembly = LoadProviderAssembly()
        ?? throw new InvalidOperationException("MySql.EntityFrameworkCore is not available to the benchmark process.");
    var method = FindUseMySqlMethod(providerAssembly);
    if (method is null) {
      throw new InvalidOperationException("The MySQL EF Core provider does not expose the expected UseMYSql options extension.");
    }

    var parameters = method.GetParameters();
    var arguments = new object?[parameters.Length];
    arguments[0] = optionsBuilder;
    arguments[1] = connectionString;

    InvokeProviderMethod(method, arguments);
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

  private static MethodInfo? FindUseMySqlMethod(Assembly providerAssembly) {
    return providerAssembly
        .GetTypes()
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        .Where(method => {
          if (!string.Equals(method.Name, "UseMySQL", StringComparison.Ordinal) &&
              !string.Equals(method.Name, "UseMySql", StringComparison.Ordinal)) {
            return false;
          }

          var parameters = method.GetParameters();

          return parameters.Length >= 2 &&
              parameters[0].ParameterType == typeof(DbContextOptionsBuilder) &&
              parameters[1].ParameterType == typeof(string) &&
              parameters.Skip(2).All(parameter => parameter.HasDefaultValue || parameter.IsOptional);
        })
        .OrderBy(method => method.GetParameters().Length)
        .FirstOrDefault();
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
