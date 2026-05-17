namespace DCoding.Data.DVault;

/// <summary>
/// Declares that a source type has a compile-time generated mapper for one logical hub-parent Data Vault satellite.
/// </summary>
/// <remarks>
/// Pair this attribute with one <see cref="DataVaultSatelliteParentHashKeyBindingAttribute" />, one
/// <see cref="DataVaultSatelliteHashDiffBindingAttribute" />, and one or more
/// <see cref="DataVaultSatellitePayloadBindingAttribute" /> declarations on the same source type. Optional
/// <see cref="DataVaultSatelliteDrivingKeyBindingAttribute" /> declarations make the generated output multi-active.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class DataVaultHubSatelliteMappingAttribute : Attribute {
  /// <summary>
  /// Initializes a new hub-parent satellite mapping declaration.
  /// </summary>
  /// <param name="parentHubName">The exact logical parent hub metadata name.</param>
  /// <param name="satelliteName">The exact logical satellite metadata name.</param>
  public DataVaultHubSatelliteMappingAttribute(string parentHubName, string satelliteName) {
    ParentHubName = parentHubName;
    SatelliteName = satelliteName;
  }

  /// <summary>
  /// Gets the exact logical parent hub metadata name.
  /// </summary>
  public string ParentHubName { get; }

  /// <summary>
  /// Gets the exact logical satellite metadata name.
  /// </summary>
  public string SatelliteName { get; }
}
