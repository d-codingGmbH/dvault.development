using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class Db2ProviderReflection {
  private const string ProviderAssemblyName = "IBM.EntityFrameworkCore";
  private const string DVaultDb2AssemblyName = "DCoding.Data.DVault.Db2";
  private const string MissingProviderSkipMessage =
      "DB2 integration tests are configured, but the IBM EF Core provider is not available. " +
      "Run dotnet test with DVAULT_TEST_DB2_CONNECTION_STRING set so the conditional test package can be restored.";
  private const string MissingDVaultDb2SkipMessage =
      "DB2 integration tests are configured, but the DVault DB2 provider extension assembly is not available. " +
      "Run dotnet test with DVAULT_TEST_DB2_CONNECTION_STRING set so the conditional project reference can be restored.";

  public static void UseDb2(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var providerAssembly = LoadAssemblyOrSkip(ProviderAssemblyName, MissingProviderSkipMessage);
    var method = FindUseDb2Method(providerAssembly);
    if (method is null) {
      throw new InvalidOperationException("The IBM EF Core provider does not expose the expected UseDb2 options extension.");
    }

    InvokeProviderMethod(method, CreateArguments(method, optionsBuilder, connectionString));
  }

  public static void AddDVaultDb2(IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    var providerAssembly = LoadAssemblyOrSkip(DVaultDb2AssemblyName, MissingDVaultDb2SkipMessage);
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

  private static Assembly LoadAssemblyOrSkip(string assemblyName, string skipMessage) {
    try {
      return Assembly.Load(new AssemblyName(assemblyName));
    }
    catch (FileNotFoundException) {
      Assert.Skip(skipMessage);
      throw new InvalidOperationException(skipMessage);
    }
  }

  private static MethodInfo? FindUseDb2Method(Assembly providerAssembly) {
    return providerAssembly
        .GetTypes()
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
