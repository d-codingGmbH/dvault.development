namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultLoadTimestampResolver : IDataVaultLoadTimestampResolver {
  public static DefaultDataVaultLoadTimestampResolver Instance { get; } = new();

  public DateTimeOffset? ResolveLoadTimestamp(DataVaultLoadTimestampResolutionContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return context.Request.LoadTimestamp;
  }
}
