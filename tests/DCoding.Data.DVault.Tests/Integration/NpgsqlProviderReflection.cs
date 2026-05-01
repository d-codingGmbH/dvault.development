using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class NpgsqlProviderReflection {
  private const string ProviderAssemblyName = "Npgsql.EntityFrameworkCore.PostgreSQL";
  private const string MissingProviderSkipMessage =
      "Postgres integration tests are configured, but the Npgsql EF Core provider is not available. " +
      "Run dotnet test with DVAULT_TEST_POSTGRES_CONNECTION_STRING set so the conditional test package can be restored.";

  public static void UseNpgsql(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var method = FindUseNpgsqlMethod(LoadProviderAssemblyOrSkip());
    if (method is null) {
      throw new InvalidOperationException("The Npgsql EF Core provider does not expose the expected UseNpgsql options extension.");
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

  private static MethodInfo? FindUseNpgsqlMethod(Assembly providerAssembly) {
    return providerAssembly
        .GetTypes()
        .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
        .SingleOrDefault(method => {
          if (method.Name != "UseNpgsql") {
            return false;
          }

          var parameters = method.GetParameters();

          return parameters.Length == 3 &&
              parameters[0].ParameterType == typeof(DbContextOptionsBuilder) &&
              parameters[1].ParameterType == typeof(string);
        });
  }
}
