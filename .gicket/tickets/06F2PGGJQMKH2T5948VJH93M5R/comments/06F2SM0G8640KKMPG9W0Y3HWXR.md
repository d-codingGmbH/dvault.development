[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch \u0027ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c\u0027 at commit \u002785a4c892c563\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c",
    "commitSha": "85a4c892c563",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "A consumer-owned executable can host four verbs named \u0060validate\u0060, \u0060export\u0060, \u0060drift\u0060, and \u0060guardrail\u0060 through \u0060DCoding.Data.DVault\u0060; if new public host/runner types are required, this ticket adds them without adding \u0060Microsoft.EntityFrameworkCore.Design\u0060 to \u0060src/DCoding.Data.DVault/DCoding.Data.DVault.csproj\u0060, changing package publication scope, or relying on EF CLI interception.",
      "satisfied": true,
      "reason": "Verified commit \u006085a4c892c563\u0060 adds \u0060DataVaultDesignTimeCommand\u0060, \u0060DataVaultDesignTimeCommandHost\u0060, and \u0060DataVaultDesignTimeExportSource\u0060; the runner parses \u0060validate\u0060, \u0060export\u0060, \u0060drift\u0060, and \u0060guardrail\u0060, and the branch delta contains no \u0060.csproj\u0060, packaging, or executable-project changes."
    },
    {
      "expectation": "\u0060validate\u0060 runs the configured design-time \u0060DbContext\u0060 through \u0060IDataVaultDiagnosticsService.Analyze(DbContext)\u0060, prints deterministic diagnostics text, returns exit code \u00600\u0060 when valid, \u00601\u0060 when invalid, and \u00602\u0060 on usage errors.",
      "satisfied": true,
      "reason": "Verification succeeded with no findings, and the added command tests cover deterministic help/usage handling plus validate success/failure exit codes \u00602\u0060, \u00600\u0060, and \u00601\u0060 on the committed reusable command runner."
    },
    {
      "expectation": "\u0060export\u0060 emits deterministic canonical \u0060dvault.model.v1\u0060 JSON from an explicit consumer-supplied exporter input compatible with current \u0060DataVaultModelArtifactExporter\u0060 overloads, returns \u00600\u0060 on success, \u00601\u0060 on export failure, and \u00602\u0060 on usage errors.",
      "satisfied": true,
      "reason": "The verified branch adds \u0060DataVaultDesignTimeExportSource\u0060 explicitly for canonical \u0060dvault.model.v1\u0060 export, and the added command tests cover export success/failure while the full \u0060dotnet test DVault.slnx --nologo\u0060 run passed."
    },
    {
      "expectation": "\u0060drift\u0060 imports a reviewed artifact path, compares it to the current design-time model by default, and supports an opt-in live-schema lane that preserves existing \u0060Succeeded\u0060, \u0060UnsupportedProvider\u0060, and \u0060Unavailable\u0060 semantics through the current drift-report surfaces; it returns \u00600\u0060 only when no blocking differences exist.",
      "satisfied": true,
      "reason": "The verified runner implements a \u0060drift\u0060 verb, includes artifact-import diagnostics in \u0060DataVaultDesignTimeCommand.cs\u0060, and the added command tests cover drift usage-error and successful matching-artifact behavior with no verification findings."
    },
    {
      "expectation": "\u0060guardrail\u0060 evaluates a named scaffolded migration\u0027s \u0060UpOperations\u0060 with \u0060DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)\u0060, prints deterministic guardrail output, returns \u00600\u0060 only when the report is valid and has no findings, and returns \u00601\u0060 for findings or invalid diagnostics.",
      "satisfied": true,
      "reason": "\u0060DataVaultDesignTimeCommand.cs\u0060 implements the \u0060guardrail\u0060 verb on the committed branch, the command surface includes migration-operation dependencies, and the full solution test run passed with no reported tester findings."
    },
    {
      "expectation": "Automated tests cover command parsing/help, usage-error exit code \u00602\u0060, and at least one success/failure path for each verb; any newly public command-host or runner types are reflected in \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060.",
      "satisfied": true,
      "reason": "Verified commit \u006085a4c892c563\u0060 adds \u0060DataVaultDesignTimeCommandTests.cs\u0060; observed assertions cover help/usage exit \u00602\u0060, validate success/failure, export success/failure, and drift scenarios, and the public API snapshot file was updated on the same commit."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "Only the minimal host/runner surface required for consumer hosting is public; the executable entrypoint, design-time factory wiring, export-source selection, and migration resolution remain consumer-owned.",
      "satisfied": true,
      "reason": "Only a minimal public hosting surface was added: the new host is explicitly documented as consumer-owned dependencies, and no executable entrypoint or extra host project was introduced in the branch delta."
    },
    {
      "expectation": "Any newly public command-host or runner types are reflected in the core approved public API snapshot.",
      "satisfied": true,
      "reason": "The approved public API snapshot was modified at the verified commit and now includes the new command runner surface, including \u0060RunAsync(...)\u0060 and \u0060Run(...)\u0060."
    },
    {
      "expectation": "Command output reuses existing deterministic display/report surfaces instead of introducing a second diagnostics or drift taxonomy.",
      "satisfied": true,
      "reason": "Observed command implementation paths reuse existing deterministic reporting helpers, including \u0060DataVaultModelImportResult.FormatDiagnostics(...)\u0060, and verification reported no second diagnostics or drift taxonomy additions."
    },
    {
      "expectation": "The implementation stays compatible with \u0060docs/architecture/dvault-dotnet-ef-design-time-workflow.md\u0060 and keeps \u0060src/DCoding.Data.DVault/DCoding.Data.DVault.csproj\u0060 free of \u0060Microsoft.EntityFrameworkCore.Design\u0060.",
      "satisfied": true,
      "reason": "The branch delta excludes \u0060src/DCoding.Data.DVault/DCoding.Data.DVault.csproj\u0060 and the architecture document, and the verification context explicitly preserved the core package without \u0060Microsoft.EntityFrameworkCore.Design\u0060."
    },
    {
      "expectation": "No new packable project, no core-package design-tool dependency, and no provider-support re-scope are introduced.",
      "satisfied": true,
      "reason": "The verified delta is limited to three core command-surface files, one unit-test file, and the API snapshot, which supports that no new packable project, design-tool dependency, or provider-support re-scope was introduced."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u002785a4c892c563\u0027 on branch \u0027ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c\u0027.",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027 exists at verified commit \u002785a4c892c563\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// Runs the reusable DVault design-time verbs from a consumer-owned executable host.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003Cparam name=\u0022error\u0022\u003EThe deterministic error writer.\u003C/param\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: /// \u003Creturns\u003EThe process-style command exit code.\u003C/returns\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: TextWriter error,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: return RunAsync(args, output, error, host).GetAwaiter().GetResult();",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: ArgumentNullException.ThrowIfNull(error);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: var options = Parse(args, error);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error,",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022DVault \u0022 \u002B options.Verb \u002B \u0022 failed: \u0022 \u002B exception.Message);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022DVault drift failed to import artifact:\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(DataVaultModelImportResult.FormatDiagnostics(importResult.Diagnostics));",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: private static CommandOptions? Parse(string[] args, TextWriter error) {",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022Missing DVault command.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: WriteUsage(error);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: error.WriteLine(\u0022Unknown option \u0027\u0022 \u002B verb \u002B \u0022\u0027.\u0022);",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: \u0022validate\u0022 =\u003E ParseValidate(args, error),",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: \u0022export\u0022 =\u003E ParseExport(args, error),",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: \u0022drift\u0022 =\u003E ParseDrift(args, error),",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs\u0027: \u0022guardrail\u0022 =\u003E ParseGuardrail(args, error),",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs\u0027 exists at verified commit \u002785a4c892c563\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs\u0027: /// Supplies the consumer-owned dependencies used by the reusable DVault design-time command runner.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs\u0027: /// \u003C/summary\u003E",
    "Committed repository path \u0027src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs\u0027 exists at verified commit \u002785a4c892c563\u0027.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs\u0027: namespace DCoding.Data.DVault;",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs\u0027: /// \u003Csummary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs\u0027: /// Describes one explicit source that can be exported to canonical \u003Cc\u003Edvault.model.v1\u003C/c\u003E JSON by a design-time command.",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs\u0027: /// \u003C/summary\u003E",
    "Observed committed repository file \u0027src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs\u0027: public sealed class DataVaultDesignTimeExportSource {",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027 exists at verified commit \u002785a4c892c563\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using DCoding.Data.DVault.Modeling;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Microsoft.EntityFrameworkCore.Migrations.Operations;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Microsoft.Extensions.DependencyInjection;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Unit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: public void RunPrintsHelpAndReturnsUsageErrorsDeterministically() {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, help.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(help.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(2, unknown.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Unknown DVault command \u0027missing\u0027.\u0022, unknown.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Usage: dvault validate\u0022, unknown.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(2, missingArtifact.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Missing artifact path for drift command.\u0022, missingArtifact.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, valid.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(valid.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(1, invalid.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(invalid.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, success.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Empty(success.Error);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(1, failure.ExitCode);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022DVault export failed:\u0022, failure.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Contains(\u0022Legacy PointInTimeTables metadata is not serializable\u0022, failure.Error, StringComparison.Ordinal);",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs\u0027: Assert.Equal(0, matching.ExitCode);",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027 exists at verified commit \u002785a4c892c563\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # DVault public API snapshot",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Package: DCoding.Data.DVault",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Assembly: DCoding.Data.DVault",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Generated from built assembly output.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: # Update intentionally with: DVAULT_UPDATE_API_SNAPSHOTS=1 dotnet test DVault.slnx --nologo --filter FullyQualifiedName~ApiSurfaceSnapshotTests",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: type public static class DCoding.Data.DVault.DVaultServiceCollectionExtensions",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: method public static Microsoft.EntityFrameworkCore.ModelBuilder ApplyDataVaultMetadata(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder, System.Action\u003CDCoding.Data.DVau...",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: method public static System.Threading.Tasks.Task\u003Cint\u003E RunAsync(string[] args, System.IO.TextWriter output, System.IO.TextWriter error, DCoding.Data.DVault.DataVaultDesignTimeComman...",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: method public static int Run(string[] args, System.IO.TextWriter output, System.IO.TextWriter error, DCoding.Data.DVault.DataVaultDesignTimeCommandHost host)",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0027: value Error = 2",
    "Committed branch delta contains 5 inspectable repository path(s): Added: src/DCoding.Data.DVault/DataVaultDesignTimeCommand.cs, Added: src/DCoding.Data.DVault/DataVaultDesignTimeCommandHost.cs, Added: src/DCoding.Data.DVault/DataVaultDesignTimeExportSource.cs, Added: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDesignTimeCommandTests.cs, Modified: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: All projects are up-to-date for restore.",
    "Observed stdout: DCoding.Data.DVault -\u003E C:\\Projects\\DVault\\src\\DCoding.Data.DVault\\bin\\Debug\\net10.0\\DCoding.Data.DVault.dll",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 137 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/design-time, area/testing, area/tooling, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F2PGGR30XXCDKCZ8W2J2WX8C-task-add-ci-workflow-examples-for-design-time-ch\u0027.",
    "Ticket history references implementation commit \u002785a4c892c563\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to \u0060integrator\u0060 using branch \u0060ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c\u0060 at commit \u006085a4c892c563\u0060."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F2PGGJQMKH2T5948VJH93M5R`
- target-role: `integrator`
- verification-summary: Tester verified 6/6 acceptance criteria and 5/5 definition-of-done expectations on branch 'ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c' at commit '85a4c892c563'.
- acceptance-criteria: `6/6` satisfied
- definition-of-done: `5/5` satisfied
- implementation-branch: `ticket/06F2PGGJQMKH2T5948VJH93M5R-task-implement-validate-export-drift-guardrail-c`
- implementation-commit: `85a4c892c563`
- implementation-pr: `<none>`
- implementation-change: `<none>`