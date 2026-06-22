using DCoding.Data.DVault;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Privacy;

/// <summary>
/// Provides startup registration extensions for the optional DVault privacy package.
/// </summary>
public static class DVaultPrivacyServiceCollectionExtensions {
  /// <summary>
  /// Adds the provider-neutral DVault defaults plus the opt-in privacy extension proof.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultPrivacy(this IServiceCollection services) {
    return AddDVaultPrivacy(services, static _ => {
    });
  }

  /// <summary>
  /// Adds the provider-neutral DVault defaults plus the opt-in privacy extension proof and applies privacy options.
  /// </summary>
  /// <param name="services">The service collection used by the application startup pipeline.</param>
  /// <param name="configure">The optional privacy configuration callback.</param>
  /// <returns>The same service collection so startup configuration can continue fluently.</returns>
  public static IServiceCollection AddDVaultPrivacy(
      this IServiceCollection services,
      Action<DataVaultPrivacyOptions> configure) {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(configure);

    services.AddDVault();

    var options = new DataVaultPrivacyOptions();
    configure(options);
    options.Apply(services);

    return services;
  }
}
