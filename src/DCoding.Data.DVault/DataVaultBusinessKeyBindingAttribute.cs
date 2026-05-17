namespace DCoding.Data.DVault;

/// <summary>
/// Binds one ordered source member to one exact logical hub business-key name for generated hub mappings.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public sealed class DataVaultBusinessKeyBindingAttribute : Attribute {
  /// <summary>
  /// Initializes a new hub business-key binding declaration.
  /// </summary>
  /// <param name="order">The zero-based logical binding order.</param>
  /// <param name="businessKeyName">The exact logical hub business-key metadata name.</param>
  /// <param name="sourceMemberName">The source string property or field name that supplies the value.</param>
  public DataVaultBusinessKeyBindingAttribute(int order, string businessKeyName, string sourceMemberName) {
    Order = order;
    BusinessKeyName = businessKeyName;
    SourceMemberName = sourceMemberName;
  }

  /// <summary>
  /// Gets the zero-based logical binding order.
  /// </summary>
  public int Order { get; }

  /// <summary>
  /// Gets the exact logical hub business-key metadata name.
  /// </summary>
  public string BusinessKeyName { get; }

  /// <summary>
  /// Gets the source string property or field name that supplies the value.
  /// </summary>
  public string SourceMemberName { get; }
}
