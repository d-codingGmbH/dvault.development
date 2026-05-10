namespace DCoding.Data.DVault;

/// <summary>
/// Maps one caller-owned source value to one registry-backed Data Vault link save operation.
/// </summary>
/// <typeparam name="TSource">The source DTO or domain type mapped by the implementation.</typeparam>
/// <remarks>
/// Implementations are expected to be side-effect-free, reject null source values immediately, and return link operations
/// identified by exact logical link metadata name. Participant hash keys are keyed by exact participant hub metadata names.
/// V1 supports only links whose participant hub metadata names are unique by <c>StringComparer.Ordinal</c>; repeated
/// same-hub and ordinary self-link typed mappings are unsupported because the registry-backed link operation rejects duplicate
/// participant names. Load timestamp and record source stay outside row mappers and are supplied when creating
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
