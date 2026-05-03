namespace DCoding.Data.DVault.PackageVerification;

public sealed class PackageVerificationResult {
  public PackageVerificationResult(IReadOnlyList<PackageVerificationIssue> issues) {
    Issues = issues;
  }

  public IReadOnlyList<PackageVerificationIssue> Issues { get; }

  public bool Succeeded => Issues.Count == 0;
}
