using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DCoding.Data.DVault;

internal static class DataVaultProviderCapabilityProfileSelection {
  private static readonly object SyncRoot = new();
  private static readonly IReadOnlyDictionary<string, DataVaultProviderCapabilityProfile> BuiltInProfilesByProviderName =
      new Dictionary<string, DataVaultProviderCapabilityProfile>(StringComparer.Ordinal) {
        ["Microsoft.EntityFrameworkCore.Sqlite"] = DataVaultProviderCapabilityProfiles.Sqlite,
        ["Microsoft.EntityFrameworkCore.SqlServer"] = DataVaultProviderCapabilityProfiles.SqlServer,
        ["Npgsql.EntityFrameworkCore.PostgreSQL"] = DataVaultProviderCapabilityProfiles.Postgres,
        ["Oracle.EntityFrameworkCore"] = DataVaultProviderCapabilityProfiles.Oracle,
        ["IBM.EntityFrameworkCore"] = DataVaultProviderCapabilityProfiles.Db2,
        ["MySql.EntityFrameworkCore"] = DataVaultProviderCapabilityProfiles.MySql,
        ["Pomelo.EntityFrameworkCore.MySql"] = DataVaultProviderCapabilityProfiles.MySql,
      };
  private static readonly Dictionary<string, DataVaultProviderCapabilityProfile> ProfilesByProviderName = new(StringComparer.Ordinal);

  public static DataVaultProviderCapabilityProfile Select(ModelBuilder modelBuilder) {
    ArgumentNullException.ThrowIfNull(modelBuilder);

    return Select(TryGetActiveProviderName(modelBuilder));
  }

  public static void Register(string providerName, DataVaultProviderCapabilityProfile providerCapabilities) {
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);
    ArgumentNullException.ThrowIfNull(providerCapabilities);

    lock (SyncRoot) {
      ProfilesByProviderName[providerName] = providerCapabilities;
    }
  }

  public static void Reset() {
    lock (SyncRoot) {
      ProfilesByProviderName.Clear();
    }
  }

  internal static DataVaultProviderCapabilityProfile Select(string? providerName) {
    if (!string.IsNullOrWhiteSpace(providerName)) {
      lock (SyncRoot) {
        if (ProfilesByProviderName.TryGetValue(providerName, out var providerCapabilities)) {
          return providerCapabilities;
        }
      }

      if (BuiltInProfilesByProviderName.TryGetValue(providerName, out var builtInProviderCapabilities)) {
        return builtInProviderCapabilities;
      }
    }

    return DataVaultProviderCapabilityProfiles.Sqlite;
  }

  internal static bool TrySelectRegistered(
      string? providerName,
      out DataVaultProviderCapabilityProfile? providerCapabilities) {
    if (!string.IsNullOrWhiteSpace(providerName)) {
      lock (SyncRoot) {
        if (ProfilesByProviderName.TryGetValue(providerName, out providerCapabilities)) {
          return true;
        }
      }

      if (BuiltInProfilesByProviderName.TryGetValue(providerName, out providerCapabilities)) {
        return true;
      }
    }

    providerCapabilities = null;
    return false;
  }

  private static string? TryGetActiveProviderName(ModelBuilder modelBuilder) {
    try {
      var conventionModelBuilder = ((IInfrastructure<IConventionModelBuilder>)modelBuilder).Instance;
      var model = conventionModelBuilder.Metadata;
      var registeredProviderNames = GetRegisteredProviderNames();

      foreach (var databaseProvider in TryGetDatabaseProviders(model)) {
        var providerName = TryGetStringMemberValue(databaseProvider, "Name");
        if (!string.IsNullOrWhiteSpace(providerName)) {
          return providerName;
        }
      }

      foreach (var providerName in TryGetProviderNamesFromModelConventions(model, registeredProviderNames)) {
        return providerName;
      }
    }
    catch (InvalidOperationException) {
      return null;
    }
    catch (NotSupportedException) {
      return null;
    }
    catch (TargetInvocationException) {
      return null;
    }

    return null;
  }

  private static IReadOnlyCollection<string> GetRegisteredProviderNames() {
    lock (SyncRoot) {
      return ProfilesByProviderName.Keys
          .Concat(BuiltInProfilesByProviderName.Keys)
          .Distinct(StringComparer.Ordinal)
          .ToArray();
    }
  }

  private static IEnumerable<object> TryGetDatabaseProviders(object model) {
    foreach (var dependenciesMemberName in new[] { "ModelDependencies", "ScopedModelDependencies" }) {
      var dependencies = TryGetMemberValue(model, dependenciesMemberName);
      var databaseProviders = TryGetMemberValue(dependencies, "DatabaseProviders") as IEnumerable;
      if (databaseProviders is null) {
        continue;
      }

      foreach (var databaseProvider in databaseProviders) {
        if (databaseProvider is not null) {
          yield return databaseProvider;
        }
      }
    }
  }

  private static IEnumerable<string> TryGetProviderNamesFromModelConventions(
      object model,
      IReadOnlyCollection<string> registeredProviderNames) {
    if (registeredProviderNames.Count == 0) {
      yield break;
    }

    var modelFinalizedConventions = TryGetMemberValue(model, "_modelFinalizedConventions") as IEnumerable;
    if (modelFinalizedConventions is null) {
      yield break;
    }

    foreach (var convention in modelFinalizedConventions) {
      if (convention is null) {
        continue;
      }

      var assemblyName = convention.GetType().Assembly.GetName().Name;
      if (assemblyName is null) {
        continue;
      }

      foreach (var providerName in registeredProviderNames) {
        if (ProviderNameMatchesAssemblyName(providerName, assemblyName)) {
          yield return providerName;
          break;
        }
      }
    }
  }

  private static bool ProviderNameMatchesAssemblyName(string providerName, string assemblyName) {
    return string.Equals(assemblyName, providerName, StringComparison.Ordinal) ||
        assemblyName.StartsWith(providerName + ".", StringComparison.Ordinal) ||
        providerName.StartsWith(assemblyName + ".", StringComparison.Ordinal);
  }

  private static string? TryGetStringMemberValue(object instance, string memberName) {
    return TryGetMemberValue(instance, memberName) as string;
  }

  private static object? TryGetMemberValue(object? instance, string memberName) {
    if (instance is null) {
      return null;
    }

    for (var type = instance.GetType(); type is not null; type = type.BaseType) {
      var property = type.GetProperty(
          memberName,
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (property is not null) {
        return property.GetValue(instance);
      }

      var field = type.GetField(
          memberName,
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (field is not null) {
        return field.GetValue(instance);
      }

      field = type.GetField(
          "<" + memberName + ">k__BackingField",
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (field is not null) {
        return field.GetValue(instance);
      }
    }

    return null;
  }
}
