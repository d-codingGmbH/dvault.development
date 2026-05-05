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
