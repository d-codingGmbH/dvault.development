using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

internal static class SqlServerProviderReflection {
  private const string ProviderAssemblyName = "Microsoft.EntityFrameworkCore.SqlServer";
  private const string MissingProviderSkipMessage =
      "SQL Server integration tests are configured, but the SQL Server EF Core provider is not available. " +
      "Run dotnet test with DVAULT_TEST_SQLSERVER_CONNECTION_STRING set so the conditional test package can be restored.";

  public static void UseSqlServer(DbContextOptionsBuilder optionsBuilder, string connectionString) {
    ArgumentNullException.ThrowIfNull(optionsBuilder);
    ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

    var method = FindUseSqlServerMethod(LoadProviderAssemblyOrSkip());
    if (method is null) {
      throw new InvalidOperationException("The SQL Server EF Core provider does not expose the expected UseSqlServer options extension.");
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

  private static MethodInfo? FindUseSqlServerMethod(Assembly providerAssembly) {
    return providerAssembly
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
  }
}
