namespace DCoding.Data.DVault;

/// <summary>
/// Binds one ordered source member to one exact produced link participant name for generated link mappings.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public sealed class DataVaultLinkParticipantBindingAttribute : Attribute {
  /// <summary>
  /// Initializes a new link participant binding declaration.
  /// </summary>
  /// <param name="order">The zero-based logical binding order.</param>
  /// <param name="participantHubName">The exact produced participant name. Use the hub name for ordinary links or the explicit role name for repeated same-hub links.</param>
  /// <param name="sourceMemberName">The source string property or field name that supplies the participant hash key.</param>
  public DataVaultLinkParticipantBindingAttribute(int order, string participantHubName, string sourceMemberName) {
    Order = order;
    ParticipantHubName = participantHubName;
    SourceMemberName = sourceMemberName;
  }

  /// <summary>
  /// Gets the zero-based logical binding order.
  /// </summary>
  public int Order { get; }

  /// <summary>
  /// Gets the exact produced participant name. This is the hub name for ordinary links or the explicit role name for
  /// repeated same-hub links.
  /// </summary>
  public string ParticipantHubName { get; }

  /// <summary>
  /// Gets the source string property or field name that supplies the participant hash key.
  /// </summary>
  public string SourceMemberName { get; }
}
