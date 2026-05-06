namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultRecordSourceResolver : IDataVaultRecordSourceResolver {
  public static DefaultDataVaultRecordSourceResolver Instance { get; } = new();

  public string? ResolveRecordSource(DataVaultRecordSourceResolutionContext context) {
    ArgumentNullException.ThrowIfNull(context);

    return context.Request.RecordSource;
  }
}
