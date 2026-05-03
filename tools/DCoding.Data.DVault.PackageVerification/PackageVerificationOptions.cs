namespace DCoding.Data.DVault.PackageVerification;

public sealed record PackageVerificationOptions(string PackageDirectory, bool ShowHelp = false) {
  public const string DefaultPackageDirectory = "artifacts/packages";
}
