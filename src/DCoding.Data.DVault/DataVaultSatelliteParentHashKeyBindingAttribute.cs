namespace DCoding.Data.DVault;

/// <summary>
/// Binds the source member that supplies a generated hub-parent satellite's parent hub hash key.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class DataVaultSatelliteParentHashKeyBindingAttribute : Attribute {
  /// <summary>
  /// Initializes a new satellite parent hash-key binding declaration.
  /// </summary>
  /// <param name="sourceMemberName">The source string property or field name that supplies the parent hub hash key.</param>
  public DataVaultSatelliteParentHashKeyBindingAttribute(string sourceMemberName) {
    SourceMemberName = sourceMemberName;
  }

  /// <summary>
  /// Gets the source string property or field name that supplies the parent hub hash key.
  /// </summary>
  public string SourceMemberName { get; }
}
