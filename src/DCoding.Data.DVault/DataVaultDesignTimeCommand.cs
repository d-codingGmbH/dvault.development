using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DCoding.Data.DVault;

/// <summary>
/// Runs the reusable DVault design-time verbs from a consumer-owned executable host.
/// </summary>
public static class DataVaultDesignTimeCommand {
  /// <summary>
  /// Runs one DVault design-time command synchronously.
  /// </summary>
  /// <param name="args">The command-line arguments to parse.</param>
  /// <param name="output">The deterministic output writer.</param>
  /// <param name="error">The deterministic error writer.</param>
  /// <param name="host">The consumer-owned command host.</param>
  /// <returns>The process-style command exit code.</returns>
  public static int Run(
      string[] args,
      TextWriter output,
      TextWriter error,
      DataVaultDesignTimeCommandHost host) {
    return RunAsync(args, output, error, host).GetAwaiter().GetResult();
  }

  /// <summary>
  /// Runs one DVault design-time command asynchronously.
  /// </summary>
  /// <param name="args">The command-line arguments to parse.</param>
  /// <param name="output">The deterministic output writer.</param>
  /// <param name="error">The deterministic error writer.</param>
  /// <param name="host">The consumer-owned command host.</param>
  /// <param name="cancellationToken">A token used to observe cancellation during live-schema reads.</param>
  /// <returns>The process-style command exit code.</returns>
  public static async Task<int> RunAsync(
      string[] args,
      TextWriter output,
      TextWriter error,
      DataVaultDesignTimeCommandHost host,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(args);
    ArgumentNullException.ThrowIfNull(output);
    ArgumentNullException.ThrowIfNull(error);
    ArgumentNullException.ThrowIfNull(host);

    var options = Parse(args, error);
    if (options is null) {
      return 2;
    }

    if (options.ShowHelp) {
      WriteUsage(output);
      return 0;
    }

    try {
      return options.Verb switch {
        "validate" => RunValidate(output, host),
        "export" => RunExport(output, host, options.OutputPath),
        "drift" => await RunDriftAsync(
            output,
            error,
            host,
            options.ArtifactPath!,
            options.UseLiveSchema,
            cancellationToken).ConfigureAwait(false),
        "guardrail" => RunGuardrail(output, host, options.MigrationName!),
        _ => throw new InvalidOperationException("Unsupported parsed DVault command '" + options.Verb + "'."),
      };
    }
    catch (Exception exception) when (IsCommandFailureException(exception)) {
      error.WriteLine("DVault " + options.Verb + " failed: " + exception.Message);
      return 1;
    }
  }

  private static int RunValidate(TextWriter output, DataVaultDesignTimeCommandHost host) {
    using var dbContext = CreateRequiredDbContext(host);
    var result = host.Diagnostics.Analyze(dbContext);

    output.WriteLine(result.ToDisplayString());
    return result.Validation.IsValid ? 0 : 1;
  }

  private static int RunExport(
      TextWriter output,
      DataVaultDesignTimeCommandHost host,
      string? outputPath) {
    var json = host.ExportSource.ExportJson();
    if (string.IsNullOrWhiteSpace(outputPath)) {
      output.Write(json);
      return 0;
    }

    File.WriteAllText(outputPath, json);
    output.WriteLine("Exported DVault model artifact to '" + outputPath + "'.");
    return 0;
  }

  private static async Task<int> RunDriftAsync(
      TextWriter output,
      TextWriter error,
      DataVaultDesignTimeCommandHost host,
      string artifactPath,
      bool useLiveSchema,
      CancellationToken cancellationToken) {
    var importResult = DataVaultModelArtifactImporter.ImportJson(
        File.ReadAllText(artifactPath),
        artifactPath);
    if (!importResult.IsValid) {
      error.WriteLine("DVault drift failed to import artifact:");
      error.WriteLine(DataVaultModelImportResult.FormatDiagnostics(importResult.Diagnostics));
      return 1;
    }

    using var dbContext = CreateRequiredDbContext(host);
    DataVaultModelDriftReport report;
    if (useLiveSchema) {
      report = host.LiveSchemaReader is null
          ? await DataVaultLiveSchemaDriftReporter.CompareAsync(
              importResult,
              dbContext,
              cancellationToken).ConfigureAwait(false)
          : await DataVaultLiveSchemaDriftReporter.CompareAsync(
              importResult,
              dbContext,
              host.LiveSchemaReader,
              cancellationToken).ConfigureAwait(false);
    }
    else {
      report = DataVaultModelDriftReporter.Compare(importResult, dbContext);
    }

    output.WriteLine(report.ToDisplayString());
    return report.HasBlockingDifferences ? 1 : 0;
  }

  private static int RunGuardrail(
      TextWriter output,
      DataVaultDesignTimeCommandHost host,
      string migrationName) {
    using var dbContext = CreateRequiredDbContext(host);
    var operations = host.ResolveMigrationOperations(migrationName) ??
        throw new InvalidOperationException(
            "The configured migration resolver returned no operations for migration '" + migrationName + "'.");
    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
        host.Diagnostics,
        dbContext,
        operations);

    output.WriteLine(report.ToDisplayString());
    return report.IsValid && !report.HasFindings ? 0 : 1;
  }

  private static DbContext CreateRequiredDbContext(DataVaultDesignTimeCommandHost host) {
    return host.CreateDbContext() ??
        throw new InvalidOperationException("The configured design-time DbContext factory returned null.");
  }

  private static CommandOptions? Parse(string[] args, TextWriter error) {
    if (args.Length == 0) {
      error.WriteLine("Missing DVault command.");
      WriteUsage(error);
      return null;
    }

    var verb = args[0];
    if (IsHelpOption(verb)) {
      return CommandOptions.Help;
    }

    if (verb.StartsWith("-", StringComparison.Ordinal)) {
      error.WriteLine("Unknown option '" + verb + "'.");
      WriteUsage(error);
      return null;
    }

    return verb switch {
      "validate" => ParseValidate(args, error),
      "export" => ParseExport(args, error),
      "drift" => ParseDrift(args, error),
      "guardrail" => ParseGuardrail(args, error),
      _ => UnknownCommand(verb, error),
    };
  }

  private static CommandOptions? ParseValidate(string[] args, TextWriter error) {
    if (args.Length == 1) {
      return new CommandOptions("validate");
    }

    var arg = args[1];
    if (IsHelpOption(arg)) {
      return CommandOptions.Help;
    }

    error.WriteLine("Unexpected argument '" + arg + "'.");
    WriteUsage(error);
    return null;
  }

  private static CommandOptions? ParseExport(string[] args, TextWriter error) {
    string? outputPath = null;
    for (var index = 1; index < args.Length; index++) {
      var arg = args[index];
      if (IsHelpOption(arg)) {
        return CommandOptions.Help;
      }

      if (string.Equals(arg, "-o", StringComparison.Ordinal) ||
          string.Equals(arg, "--output", StringComparison.Ordinal)) {
        if (!TryReadOptionValue(args, ref index, arg, error, out outputPath)) {
          return null;
        }
      }
      else {
        error.WriteLine("Unexpected argument '" + arg + "'.");
        WriteUsage(error);
        return null;
      }
    }

    return new CommandOptions("export", OutputPath: outputPath);
  }

  private static CommandOptions? ParseDrift(string[] args, TextWriter error) {
    string? artifactPath = null;
    var useLiveSchema = false;
    for (var index = 1; index < args.Length; index++) {
      var arg = args[index];
      if (IsHelpOption(arg)) {
        return CommandOptions.Help;
      }

      if (string.Equals(arg, "--live-schema", StringComparison.Ordinal)) {
        useLiveSchema = true;
      }
      else if (string.Equals(arg, "-a", StringComparison.Ordinal) ||
          string.Equals(arg, "--artifact", StringComparison.Ordinal)) {
        if (!TryReadOptionValue(args, ref index, arg, error, out artifactPath)) {
          return null;
        }
      }
      else if (arg.StartsWith("-", StringComparison.Ordinal)) {
        error.WriteLine("Unknown option '" + arg + "'.");
        WriteUsage(error);
        return null;
      }
      else if (artifactPath is null) {
        artifactPath = arg;
      }
      else {
        error.WriteLine("Unexpected argument '" + arg + "'.");
        WriteUsage(error);
        return null;
      }
    }

    if (artifactPath is null) {
      error.WriteLine("Missing artifact path for drift command.");
      WriteUsage(error);
      return null;
    }

    return new CommandOptions("drift", ArtifactPath: artifactPath, UseLiveSchema: useLiveSchema);
  }

  private static CommandOptions? ParseGuardrail(string[] args, TextWriter error) {
    string? migrationName = null;
    for (var index = 1; index < args.Length; index++) {
      var arg = args[index];
      if (IsHelpOption(arg)) {
        return CommandOptions.Help;
      }

      if (string.Equals(arg, "-m", StringComparison.Ordinal) ||
          string.Equals(arg, "--migration", StringComparison.Ordinal)) {
        if (!TryReadOptionValue(args, ref index, arg, error, out migrationName)) {
          return null;
        }
      }
      else if (arg.StartsWith("-", StringComparison.Ordinal)) {
        error.WriteLine("Unknown option '" + arg + "'.");
        WriteUsage(error);
        return null;
      }
      else if (migrationName is null) {
        migrationName = arg;
      }
      else {
        error.WriteLine("Unexpected argument '" + arg + "'.");
        WriteUsage(error);
        return null;
      }
    }

    if (migrationName is null) {
      error.WriteLine("Missing migration name for guardrail command.");
      WriteUsage(error);
      return null;
    }

    return new CommandOptions("guardrail", MigrationName: migrationName);
  }

  private static CommandOptions? UnknownCommand(string verb, TextWriter error) {
    error.WriteLine("Unknown DVault command '" + verb + "'.");
    WriteUsage(error);
    return null;
  }

  private static bool TryReadOptionValue(
      string[] args,
      ref int index,
      string optionName,
      TextWriter error,
      out string? value) {
    if (index + 1 >= args.Length) {
      error.WriteLine("Missing value for " + optionName + ".");
      WriteUsage(error);
      value = null;
      return false;
    }

    value = args[++index];
    if (string.IsNullOrWhiteSpace(value)) {
      error.WriteLine("Missing value for " + optionName + ".");
      WriteUsage(error);
      value = null;
      return false;
    }

    return true;
  }

  private static void WriteUsage(TextWriter writer) {
    writer.WriteLine("Usage: dvault validate");
    writer.WriteLine("       dvault export [--output <path>]");
    writer.WriteLine("       dvault drift [--live-schema] (--artifact <path>|<path>)");
    writer.WriteLine("       dvault guardrail (--migration <name>|<name>)");
  }

  private static bool IsHelpOption(string value) {
    return string.Equals(value, "-h", StringComparison.Ordinal) ||
        string.Equals(value, "--help", StringComparison.Ordinal);
  }

  private static bool IsCommandFailureException(Exception exception) {
    return exception is ArgumentException or
        InvalidOperationException or
        IOException or
        NotSupportedException or
        UnauthorizedAccessException;
  }

  private sealed record CommandOptions(
      string Verb,
      bool ShowHelp = false,
      string? OutputPath = null,
      string? ArtifactPath = null,
      bool UseLiveSchema = false,
      string? MigrationName = null) {
    public static CommandOptions Help { get; } = new("help", ShowHelp: true);
  }
}
