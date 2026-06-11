namespace DCoding.Data.DVault;

internal sealed class DefaultStableHashService : IStableHashService {
  public static DefaultStableHashService Instance { get; } = new();

  public string AlgorithmId => BuiltInStableHashService.Sha256.AlgorithmId;

  public StableHashDigest ComputeHash(string normalizedInput) {
    return BuiltInStableHashService.Sha256.ComputeHash(normalizedInput);
  }
}
