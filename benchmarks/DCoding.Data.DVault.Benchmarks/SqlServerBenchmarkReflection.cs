using System.Data.Common;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault.Benchmarks;

internal static class SqlServerBenchmarkReflection {
  private const string ProviderAssemblyName = "Microsoft.EntityFrameworkCore.SqlServer";
  private const string ConnectionTypeName = "Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient";

  public static bool IsProviderDependencyAvailable() {
    var providerAssembly = LoadProviderAssembly();

    return GetConnectionType() is not null &&
        providerAssembly is not null &&
        IsEfCoreMajorVersionCompatible(providerAssembly);
  }

  public static DbConnection CreateConnection(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var connectionType = GetConnectionType()
        ?? throw new InvalidOperationException("Microsoft.Data.SqlClient is not available to the benchmark process.");

    return (DbConnection)Activator.CreateInstance(connectionType, connectionString)!;
  }

  public static Task<string?> TryOpenConnectionAsync(string connectionString, CancellationToken cancellationToken) {
    return DbConnectionAvailability.TryOpenConnectionAsync(CreateConnection, connectionString, cancellationToken);
  }

  public static void UseSqlServer(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var providerAssembly = LoadProviderAssembly()
        ?? throw new InvalidOperationException("Microsoft.EntityFrameworkCore.SqlServer is not available to the benchmark process.");
    var method = providerAssembly
        .GetTypes()
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        .SingleOrDefault(method => {
          if (method.Name != "UseSqlServer") {
            return false;
          }

          var parameters = method.GetParameters();

          return parameters.Length == 3 &&
              parameters[0].ParameterType == typeof(DbContextOptionsBuilder) &&
              parameters[1].ParameterType == typeof(string);
        });

    if (method is null) {
      throw new InvalidOperationException("The SQL Server EF Core provider does not expose the expected UseSqlServer options extension.");
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

  private static bool IsEfCoreMajorVersionCompatible(Assembly providerAssembly) {
    var currentEfCoreMajor = typeof(DbContext).Assembly.GetName().Version?.Major;
    var referencedEfCoreMajor = providerAssembly
        .GetReferencedAssemblies()
        .Where(reference => string.Equals(reference.Name, "Microsoft.EntityFrameworkCore.Relational", StringComparison.Ordinal))
        .Select(reference => reference.Version?.Major)
        .FirstOrDefault();

    return currentEfCoreMajor is not null &&
        referencedEfCoreMajor is not null &&
        currentEfCoreMajor == referencedEfCoreMajor;
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
