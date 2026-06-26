namespace DCoding.Data.DVault;

/// <summary>
/// Maps one caller-owned source value to one registry-backed Data Vault link save operation.
/// </summary>
/// <typeparam name="TSource">The source DTO or domain type mapped by the implementation.</typeparam>
/// <remarks>
/// Implementations are expected to be side-effect-free, reject null source values immediately, and return link operations
/// identified by exact logical link metadata name. Participant hash keys are keyed by exact produced participant names: hub
/// names for ordinary links and explicit role names for repeated same-hub links. V1 supports links whose produced
/// participant names are unique by <c>StringComparer.Ordinal</c>; ambiguous repeated same-hub mappings that reuse the same
/// produced participant name remain invalid. Load timestamp and record source stay outside row mappers and are supplied when creating
/// <see cref="DataVaultRegistrySaveRequest" /> or <see cref="DataVaultRegistryBulkSaveRequest" />.
/// </remarks>
public interface IDataVaultLinkMapper<in TSource>
    where TSource : notnull {
  /// <summary>
  /// Maps one source value to one registry-backed link save operation.
  /// </summary>
  /// <param name="source">The non-null source value to map.</param>
  /// <returns>A registry-backed save operation for one link row.</returns>
  /// <exception cref="ArgumentNullException">Thrown by implementations when <paramref name="source" /> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when mapped participant names or values are invalid.</exception>
  DataVaultRegistryLinkSaveOperation Map(TSource source);
}
