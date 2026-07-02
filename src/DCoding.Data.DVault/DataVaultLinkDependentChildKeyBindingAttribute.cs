namespace DCoding.Data.DVault;

/// <summary>
/// Binds one ordered source member to one exact dependent child key name for generated link mappings.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public sealed class DataVaultLinkDependentChildKeyBindingAttribute : Attribute {
  /// <summary>
  /// Initializes a new link dependent child key binding declaration.
  /// </summary>
  /// <param name="order">The zero-based logical binding order.</param>
  /// <param name="dependentChildKeyName">The exact dependent child key name declared by the link metadata.</param>
  /// <param name="sourceMemberName">The source string property or field name that supplies the dependent child key value.</param>
  public DataVaultLinkDependentChildKeyBindingAttribute(
      int order,
      string dependentChildKeyName,
      string sourceMemberName) {
    Order = order;
    DependentChildKeyName = dependentChildKeyName;
    SourceMemberName = sourceMemberName;
  }

  /// <summary>
  /// Gets the zero-based logical binding order.
  /// </summary>
  public int Order { get; }

  /// <summary>
  /// Gets the exact dependent child key name declared by the link metadata.
  /// </summary>
  public string DependentChildKeyName { get; }

  /// <summary>
  /// Gets the source string property or field name that supplies the dependent child key value.
  /// </summary>
  public string SourceMemberName { get; }
}
