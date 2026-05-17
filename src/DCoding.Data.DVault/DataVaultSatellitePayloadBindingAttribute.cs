namespace DCoding.Data.DVault;

/// <summary>
/// Binds one ordered source member to one exact logical satellite payload name for generated satellite mappings.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public sealed class DataVaultSatellitePayloadBindingAttribute : Attribute {
  /// <summary>
  /// Initializes a new satellite payload binding declaration.
  /// </summary>
  /// <param name="order">The zero-based logical binding order.</param>
  /// <param name="payloadName">The exact logical satellite payload metadata name.</param>
  /// <param name="sourceMemberName">The source string property or field name that supplies the payload value.</param>
  public DataVaultSatellitePayloadBindingAttribute(int order, string payloadName, string sourceMemberName) {
    Order = order;
    PayloadName = payloadName;
    SourceMemberName = sourceMemberName;
  }

  /// <summary>
  /// Gets the zero-based logical binding order.
  /// </summary>
  public int Order { get; }

  /// <summary>
  /// Gets the exact logical satellite payload metadata name.
  /// </summary>
  public string PayloadName { get; }

  /// <summary>
  /// Gets the source string property or field name that supplies the payload value.
  /// </summary>
  public string SourceMemberName { get; }
}
