namespace DCoding.Data.DVault;

/// <summary>
/// Binds the source member that supplies a generated satellite mapping's caller-provided hash diff.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class DataVaultSatelliteHashDiffBindingAttribute : Attribute {
  /// <summary>
  /// Initializes a new satellite hash-diff binding declaration.
  /// </summary>
  /// <param name="sourceMemberName">The source string property or field name that supplies the hash diff.</param>
  public DataVaultSatelliteHashDiffBindingAttribute(string sourceMemberName) {
    SourceMemberName = sourceMemberName;
  }

  /// <summary>
  /// Gets the source string property or field name that supplies the hash diff.
  /// </summary>
  public string SourceMemberName { get; }
}
