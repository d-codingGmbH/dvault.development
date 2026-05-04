using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class MySqlProviderReflection {
  private const string ProviderAssemblyName = "Pomelo.EntityFrameworkCore.MySql";
  private const string ServerVersionTypeName = "Microsoft.EntityFrameworkCore.ServerVersion";
  private const string MissingProviderSkipMessage =
      "MySQL integration tests are configured, but the Pomelo EF Core MySQL provider is not available. " +
      "Run dotnet test with DVAULT_TEST_MYSQL_CONNECTION_STRING set so the conditional test package can be restored.";

  public static void UseMySql(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var providerAssembly = LoadProviderAssemblyOrSkip();
    var serverVersion = AutoDetectServerVersion(providerAssembly, connectionString);
    var method = FindUseMySqlMethod(providerAssembly, serverVersion.GetType());
    if (method is null) {
      throw new InvalidOperationException(
          "The Pomelo EF Core MySQL provider does not expose the expected UseMySql options extension.");
    }

    var parameters = method.GetParameters();
    var arguments = parameters.Length == 4
        ? new[] { optionsBuilder, connectionString, serverVersion, null }
        : new[] { optionsBuilder, connectionString, serverVersion };

    InvokeProviderMethod(method, arguments);
  }

  private static Assembly LoadProviderAssemblyOrSkip() {
    try {
      return Assembly.Load(new AssemblyName(ProviderAssemblyName));
    }
    catch (FileNotFoundException) {
      Assert.Skip(MissingProviderSkipMessage);
      throw new InvalidOperationException(MissingProviderSkipMessage);
    }
  }

  private static object AutoDetectServerVersion(Assembly providerAssembly, string connectionString) {
    var serverVersionType = providerAssembly.GetType(ServerVersionTypeName);
    if (serverVersionType is null) {
      throw new InvalidOperationException(
          "The Pomelo EF Core MySQL provider does not expose the expected ServerVersion type.");
    }

    var autoDetectMethod = serverVersionType
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .FirstOrDefault(method => {
          if (method.Name != "AutoDetect") {
            return false;
          }

          var parameters = method.GetParameters();

          return parameters.Length == 1 &&
              parameters[0].ParameterType == typeof(string);
        });
    if (autoDetectMethod is null) {
      throw new InvalidOperationException(
          "The Pomelo EF Core MySQL provider does not expose the expected ServerVersion.AutoDetect(string) method.");
    }

    return InvokeProviderMethod(autoDetectMethod, [connectionString]) ??
        throw new InvalidOperationException("Pomelo ServerVersion.AutoDetect(string) returned null.");
  }

  private static MethodInfo? FindUseMySqlMethod(Assembly providerAssembly, Type detectedServerVersionType) {
    return providerAssembly
        .GetTypes()
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        .Where(method => {
          if (method.Name != "UseMySql") {
            return false;
          }

          var parameters = method.GetParameters();

          return (parameters.Length == 3 || parameters.Length == 4) &&
              parameters[0].ParameterType == typeof(DbContextOptionsBuilder) &&
              parameters[1].ParameterType == typeof(string) &&
              parameters[2].ParameterType.IsAssignableFrom(detectedServerVersionType);
        })
        .OrderByDescending(method => method.GetParameters().Length)
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
