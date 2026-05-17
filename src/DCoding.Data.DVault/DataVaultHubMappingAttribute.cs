namespace DCoding.Data.DVault;

/// <summary>
/// Declares that a source type has a compile-time generated mapper for one logical Data Vault hub.
/// </summary>
/// <remarks>
/// Pair this attribute with one or more <see cref="DataVaultBusinessKeyBindingAttribute" /> declarations on the same
/// source type. Binding order is defined by each binding attribute's <c>order</c> constructor argument.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class DataVaultHubMappingAttribute : Attribute {
  /// <summary>
  /// Initializes a new hub mapping declaration.
  /// </summary>
  /// <param name="hubName">The exact logical hub metadata name.</param>
  public DataVaultHubMappingAttribute(string hubName) {
    HubName = hubName;
  }

  /// <summary>
  /// Gets the exact logical hub metadata name.
  /// </summary>
  public string HubName { get; }
}
