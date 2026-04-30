using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

/// <summary>
/// Provides Entity Framework Core model configuration extensions for DVault conventions.
/// </summary>
public static class DataVaultModelBuilderExtensions {
  private const string ConventionsAnnotationName = "DCoding.Data.DVault:Conventions";

  /// <summary>
  /// Records the provider-neutral DVault default conventions on the Entity Framework model.
  /// </summary>
  /// <param name="modelBuilder">The Entity Framework model builder to configure.</param>
  /// <returns>The same model builder so Entity Framework model configuration can continue fluently.</returns>
  public static ModelBuilder UseDataVault(this ModelBuilder modelBuilder) {
    ArgumentNullException.ThrowIfNull(modelBuilder);

    modelBuilder.Model.SetAnnotation(ConventionsAnnotationName, DataVaultConventions.Default);

    return modelBuilder;
  }
}
