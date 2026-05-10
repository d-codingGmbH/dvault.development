namespace DCoding.Data.DVault;

/// <summary>
/// Maps one caller-owned source value to one registry-backed Data Vault satellite save operation.
/// </summary>
/// <typeparam name="TSource">The source DTO or domain type mapped by the implementation.</typeparam>
/// <remarks>
/// Implementations are expected to be side-effect-free, reject null source values immediately, and return satellite operations
/// identified by exact parent reference plus exact logical satellite metadata name. Outputs include the explicit parent hash key,
/// payload values keyed by exact payload names, a caller-supplied hash diff, and optional multi-active driving-key values keyed by
/// exact driving-key names. Both hub-parent and link-parent satellites use the same contract. Load timestamp and record source
/// stay outside row mappers and are supplied when creating <see cref="DataVaultRegistrySaveRequest" /> or
/// <see cref="DataVaultRegistryBulkSaveRequest" />.
/// </remarks>
public interface IDataVaultSatelliteMapper<in TSource>
    where TSource : notnull {
  /// <summary>
  /// Maps one source value to one registry-backed satellite save operation.
  /// </summary>
  /// <param name="source">The non-null source value to map.</param>
  /// <returns>A registry-backed save operation for one satellite row.</returns>
  /// <exception cref="ArgumentNullException">Thrown by implementations when <paramref name="source" /> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when mapped parent, driving-key, payload, or hash-diff values are invalid.</exception>
  DataVaultRegistrySatelliteSaveOperation Map(TSource source);
}
