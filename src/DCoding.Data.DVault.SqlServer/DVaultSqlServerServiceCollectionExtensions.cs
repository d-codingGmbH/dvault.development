using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides startup registration extensions for SQL Server-specific DVault services.
/// </summary>
public static class DVaultSqlServerServiceCollectionExtensions {
  /// <summary>
  /// Adds DVault defaults plus the SQL Server optimized save and latest-satellite/PIT/bridge read strategies.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultSqlServer(this IServiceCollection services) {
    ArgumentNullException.ThrowIfNull(services);

    DataVaultProviderCapabilityProfileSelection.Register(
        SqlServerDataVaultSaveStrategy.SqlServerProviderName,
        DataVaultProviderCapabilityProfiles.SqlServer);
    services.AddDVault();
    services.Replace(ServiceDescriptor.Singleton<IDataVaultPitMaintenanceService, SqlServerDataVaultPitMaintenanceService>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBehavior, SqlServerDataVaultProviderBehavior>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderSaveStrategy, SqlServerDataVaultSaveStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderReadStrategy, SqlServerDataVaultReadStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderPitReadStrategy, SqlServerDataVaultReadStrategy>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IDataVaultProviderBridgeReadStrategy, SqlServerDataVaultReadStrategy>());

    return services;
  }

  /// <summary>
  /// Adds one explicit SQL Server Always Encrypted selection for a provider-owned privacy diagnostics lane.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <param name="encryptedPayloadAlias">The provider-neutral encrypted-payload alias selected by the application.</param>
  /// <param name="callerOwnedPrerequisiteProofNames">Redaction-safe names for caller-owned Always Encrypted prerequisites.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultSqlServerAlwaysEncryptedSelection(
      this IServiceCollection services,
      string encryptedPayloadAlias,
      params string[] callerOwnedPrerequisiteProofNames) {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentException.ThrowIfNullOrWhiteSpace(encryptedPayloadAlias);
    ArgumentNullException.ThrowIfNull(callerOwnedPrerequisiteProofNames);

    if (callerOwnedPrerequisiteProofNames.Any(string.IsNullOrWhiteSpace)) {
      throw new ArgumentException(
          "Caller-owned prerequisite proof names must be non-empty when supplied.",
          nameof(callerOwnedPrerequisiteProofNames));
    }

    if (services.Any(descriptor =>
        descriptor.ServiceType == typeof(IDataVaultProviderNativeCryptoSelectionProvider) &&
        descriptor.ImplementationInstance is SqlServerAlwaysEncryptedDataVaultProviderNativeCryptoSelectionProvider provider &&
        string.Equals(provider.EncryptedPayloadAlias, encryptedPayloadAlias, StringComparison.Ordinal))) {
      throw new InvalidOperationException(
          "SQL Server Always Encrypted selection for encrypted payload alias '" +
          encryptedPayloadAlias +
          "' has already been registered.");
    }

    services.AddDVaultSqlServer();
    services.AddSingleton<IDataVaultProviderNativeCryptoSelectionProvider>(
        new SqlServerAlwaysEncryptedDataVaultProviderNativeCryptoSelectionProvider(
            encryptedPayloadAlias,
            [.. callerOwnedPrerequisiteProofNames]));

    return services;
  }
}
