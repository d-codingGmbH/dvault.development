namespace DCoding.Data.DVault;

/// <summary>
/// Maps one caller-owned source value to one registry-backed Data Vault hub save operation.
/// </summary>
/// <typeparam name="TSource">The source DTO or domain type mapped by the implementation.</typeparam>
/// <remarks>
/// Implementations are expected to be side-effect-free, reject null source values immediately, and return hub operations
/// identified by exact logical hub metadata name. Business-key values are keyed by exact hub business-key names, while
/// canonical ordering remains owned by the resolved metadata declaration. Load timestamp and record source stay outside
/// row mappers and are supplied when creating <see cref="DataVaultRegistrySaveRequest" /> or
/// <see cref="DataVaultRegistryBulkSaveRequest" />.
/// </remarks>
public interface IDataVaultHubMapper<in TSource>
    where TSource : notnull {
  /// <summary>
  /// Maps one source value to one registry-backed hub save operation.
  /// </summary>
  /// <param name="source">The non-null source value to map.</param>
  /// <returns>A registry-backed save operation for one hub row.</returns>
  /// <exception cref="ArgumentNullException">Thrown by implementations when <paramref name="source" /> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when mapped business-key names or values are invalid.</exception>
  DataVaultRegistryHubSaveOperation Map(TSource source);
}
