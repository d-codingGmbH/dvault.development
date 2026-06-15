using System.Data.Common;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class Db2BenchmarkReflection {
  private const string ProviderAssemblyName = "IBM.EntityFrameworkCore";
  private const string DVaultDb2AssemblyName = "DCoding.Data.DVault.Db2";
  private const string ConnectionTypeName = "IBM.Data.Db2.Core.DB2Connection, IBM.Data.Db2.Core";

  public static bool IsProviderDependencyAvailable() {
    return GetConnectionType() is not null &&
        LoadAssembly(ProviderAssemblyName) is not null &&
        LoadAssembly(DVaultDb2AssemblyName) is not null;
  }

  public static DbConnection CreateConnection(string connectionString) {
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var connectionType = GetConnectionType()
        ?? throw new InvalidOperationException("IBM.Data.Db2.Core is not available to the benchmark process.");

    return (DbConnection)Activator.CreateInstance(connectionType, connectionString)!;
  }

  public static Task<string?> TryOpenConnectionAsync(string connectionString, CancellationToken cancellationToken) {
    return DbConnectionAvailability.TryOpenConnectionAsync(CreateConnection, connectionString, cancellationToken);
  }

  public static void UseDb2(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var providerAssembly = LoadAssembly(ProviderAssemblyName)
        ?? throw new InvalidOperationException("IBM.EntityFrameworkCore is not available to the benchmark process.");
    var method = FindUseDb2Method(providerAssembly);
    if (method is null) {
      throw new InvalidOperationException("The IBM EF Core provider does not expose the expected UseDb2 options extension.");
    }

    InvokeProviderMethod(method, CreateArguments(method, optionsBuilder, connectionString));
  }

  public static void AddDVaultDb2(IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    var providerAssembly = LoadAssembly(DVaultDb2AssemblyName)
        ?? throw new InvalidOperationException("DCoding.Data.DVault.Db2 is not available to the benchmark process.");
    var type = providerAssembly.GetType(
        "DCoding.Data.DVault.DVaultDb2ServiceCollectionExtensions",
        throwOnError: true);
    var method = type!
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .SingleOrDefault(candidate => {
          if (!string.Equals(candidate.Name, "AddDVaultDb2", StringComparison.Ordinal)) {
            return false;
          }

          var parameters = candidate.GetParameters();

          return parameters.Length == 1 &&
              parameters[0].ParameterType == typeof(IServiceCollection);
        });

    if (method is null) {
      throw new InvalidOperationException("The DVault DB2 provider assembly does not expose AddDVaultDb2(IServiceCollection).");
    }

    InvokeProviderMethod(method, [services]);
  }

  private static Assembly? LoadAssembly(string assemblyName) {
    try {
      return Assembly.Load(new AssemblyName(assemblyName));
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

  private static MethodInfo? FindUseDb2Method(Assembly providerAssembly) {
    return GetLoadableTypes(providerAssembly)
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        .Where(method => {
          if (!string.Equals(method.Name, "UseDb2", StringComparison.Ordinal) &&
              !string.Equals(method.Name, "UseDB2", StringComparison.Ordinal)) {
            return false;
          }

          var parameters = method.GetParameters();

          return parameters.Length >= 2 &&
              parameters[0].ParameterType == typeof(DbContextOptionsBuilder) &&
              parameters[1].ParameterType == typeof(string);
        })
        .OrderBy(method => method.GetParameters().Length)
        .FirstOrDefault();
  }

  private static IEnumerable<Type> GetLoadableTypes(Assembly assembly) {
    try {
      return assembly.GetTypes();
    }
    catch (ReflectionTypeLoadException exception) {
      return exception.Types.OfType<Type>();
    }
  }

  private static object?[] CreateArguments(
      MethodInfo method,
      DbContextOptionsBuilder optionsBuilder,
      string connectionString) {
    var parameters = method.GetParameters();
    var arguments = new object?[parameters.Length];
    arguments[0] = optionsBuilder;
    arguments[1] = connectionString;

    for (var index = 2; index < parameters.Length; index++) {
      arguments[index] = parameters[index].HasDefaultValue
          ? parameters[index].DefaultValue
          : null;
    }

    return arguments;
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
