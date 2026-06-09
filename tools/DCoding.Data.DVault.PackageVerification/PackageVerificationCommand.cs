namespace DCoding.Data.DVault.PackageVerification;

public static class PackageVerificationCommand {
  public static int Run(string[] args, TextWriter output, TextWriter error) {
    var options = Parse(args, error);
    if (options is null) {
      return 2;
    }

    if (options.ShowHelp) {
      WriteUsage(output);
      return 0;
    }

    var result = new PackageVerifier().Verify(options);
    if (result.Succeeded) {
      output.WriteLine(
          "Verified DVault packages in '" +
          options.PackageDirectory +
          "': exactly seven .nupkg files, six .snupkg files, metadata, dual-line README guidance, XML docs, symbols, analyzer assets, provider dependencies, and net8.0/net10.0 EF dependency groups are valid.");
      return 0;
    }

    error.WriteLine("DVault package verification failed for '" + options.PackageDirectory + "':");
    foreach (var issue in result.Issues) {
      error.WriteLine("- " + issue);
    }

    return 1;
  }

  private static PackageVerificationOptions? Parse(string[] args, TextWriter error) {
    var packageDirectory = PackageVerificationOptions.DefaultPackageDirectory;
    var packageDirectorySet = false;
    var showHelp = false;

    for (var index = 0; index < args.Length; index++) {
      var arg = args[index];
      if (string.Equals(arg, "-h", StringComparison.Ordinal) ||
          string.Equals(arg, "--help", StringComparison.Ordinal)) {
        showHelp = true;
      }
      else if (string.Equals(arg, "-p", StringComparison.Ordinal) ||
          string.Equals(arg, "--package-directory", StringComparison.Ordinal)) {
        if (index + 1 >= args.Length) {
          error.WriteLine("Missing value for " + arg + ".");
          WriteUsage(error);
          return null;
        }

        packageDirectory = args[++index];
        packageDirectorySet = true;
      }
      else if (arg.StartsWith("-", StringComparison.Ordinal)) {
        error.WriteLine("Unknown option '" + arg + "'.");
        WriteUsage(error);
        return null;
      }
      else if (!packageDirectorySet) {
        packageDirectory = arg;
        packageDirectorySet = true;
      }
      else {
        error.WriteLine("Unexpected argument '" + arg + "'.");
        WriteUsage(error);
        return null;
      }
    }

    return new PackageVerificationOptions(packageDirectory, showHelp);
  }

  private static void WriteUsage(TextWriter writer) {
    writer.WriteLine("Usage: dotnet run --project tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj -- [--package-directory artifacts/packages]");
    writer.WriteLine("       dotnet run --project tools/DCoding.Data.DVault.PackageVerification/DCoding.Data.DVault.PackageVerification.csproj -- [artifacts/packages]");
  }
}
