using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class MySqlProviderReflection {
  private const string ProviderAssemblyName = "MySql.EntityFrameworkCore";
  private const string MissingProviderSkipMessage =
      "MySQL integration tests are configured, but the official MySQL EF Core provider is not available. " +
      "Run dotnet test with DVAULT_TEST_MYSQL_CONNECTION_STRING set so the conditional test package can be restored.";

  public static void UseMySql(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var providerAssembly = LoadProviderAssemblyOrSkip();
    var method = FindUseMySqlMethod(providerAssembly);
    if (method is null) {
      throw new InvalidOperationException("The MySQL EF Core provider does not expose the expected UseMySQL options extension.");
    }

    var parameters = method.GetParameters();
    var arguments = new object?[parameters.Length];
    arguments[0] = optionsBuilder;
    arguments[1] = connectionString;

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
