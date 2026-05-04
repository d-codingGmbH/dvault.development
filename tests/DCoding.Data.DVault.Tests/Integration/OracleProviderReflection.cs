using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class OracleProviderReflection {
  private const string ProviderAssemblyName = "Oracle.EntityFrameworkCore";
  private const string MissingProviderSkipMessage =
      "Oracle integration tests are configured, but the Oracle EF Core provider is not available. " +
      "Run dotnet test with DVAULT_TEST_ORACLE_CONNECTION_STRING set so the conditional test package can be restored.";

  public static void UseOracle(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var method = FindUseOracleMethod(LoadProviderAssemblyOrSkip());
    if (method is null) {
      throw new InvalidOperationException("The Oracle EF Core provider does not expose the expected UseOracle options extension.");
    }

    method.Invoke(null, [optionsBuilder, connectionString, null]);
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

  private static MethodInfo? FindUseOracleMethod(Assembly providerAssembly) {
    return providerAssembly
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
  }
}
