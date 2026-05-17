namespace DCoding.Data.DVault;

/// <summary>
/// Declares that a source type has a compile-time generated mapper for one logical Data Vault link.
/// </summary>
/// <remarks>
/// Pair this attribute with two or more <see cref="DataVaultLinkParticipantBindingAttribute" /> declarations on the same
/// source type. V1 generated link mappings support only unique participant hub names by <see cref="StringComparer.Ordinal" />.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class DataVaultLinkMappingAttribute : Attribute {
  /// <summary>
  /// Initializes a new link mapping declaration.
  /// </summary>
  /// <param name="linkName">The exact logical link metadata name.</param>
  public DataVaultLinkMappingAttribute(string linkName) {
    LinkName = linkName;
  }

  /// <summary>
  /// Gets the exact logical link metadata name.
  /// </summary>
  public string LinkName { get; }
}
