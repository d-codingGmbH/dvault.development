namespace DCoding.Data.DVault.PackageVerification;

public sealed record PackageVerificationIssue(string PackageId, string Message) {
  public override string ToString() {
    return string.IsNullOrWhiteSpace(PackageId)
        ? Message
        : PackageId + ": " + Message;
  }
}
