namespace DCoding.Data.DVault;

/// <summary>
/// Binds one ordered source member to one exact logical satellite driving-key name for generated multi-active mappings.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public sealed class DataVaultSatelliteDrivingKeyBindingAttribute : Attribute {
  /// <summary>
  /// Initializes a new satellite driving-key binding declaration.
  /// </summary>
  /// <param name="order">The zero-based logical binding order.</param>
  /// <param name="drivingKeyName">The exact logical satellite driving-key metadata name.</param>
  /// <param name="sourceMemberName">The source string property or field name that supplies the driving-key value.</param>
  public DataVaultSatelliteDrivingKeyBindingAttribute(int order, string drivingKeyName, string sourceMemberName) {
    Order = order;
    DrivingKeyName = drivingKeyName;
    SourceMemberName = sourceMemberName;
  }

  /// <summary>
  /// Gets the zero-based logical binding order.
  /// </summary>
  public int Order { get; }

  /// <summary>
  /// Gets the exact logical satellite driving-key metadata name.
  /// </summary>
  public string DrivingKeyName { get; }

  /// <summary>
  /// Gets the source string property or field name that supplies the driving-key value.
  /// </summary>
  public string SourceMemberName { get; }
}
