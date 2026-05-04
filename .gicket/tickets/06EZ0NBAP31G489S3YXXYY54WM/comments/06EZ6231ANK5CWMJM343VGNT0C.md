[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EZ0NBAP31G489S3YXXYY54WM\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil\u0027 and commit \u0027695bf4083de7\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil\u0027 from source \u0027695bf4083de7\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil\u0027.",
    "Evidence: \u0060git rev-parse 695bf4083de7\u0060 resolved to \u0060695bf4083de753367ac88317a2ffdd2bf6391e50\u0060, and \u0060git diff --name-only 695bf4083de7..HEAD\u0060 listed only \u0060.gicket/...\u0060 paths, so the claimed commit still matches the branch\u0027s repository implementation.",
    "Evidence: \u0060git diff --stat develop...695bf4083de7 -- README.md docs src tests\u0060 reported 15 repository files changed with \u0060714 insertions(\u002B), 12 deletions(-)\u0060, including \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060, \u0060src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs\u0060, \u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060, and the targeted test files.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs\u0060 defines \u0060DataVaultProviderCapabilityProfiles.Oracle\u0060 with profile name \u0060oracle-v1\u0060, \u0060NoneInV1Unsupported\u0060 SQL/concurrency baselines, and mappings using \u0060VARCHAR2(64 CHAR)\u0060, \u0060TIMESTAMP WITH TIME ZONE\u0060, \u0060VARCHAR2(255 CHAR)\u0060, and \u0060CLOB\u0060 for the required logical property kinds.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060 keeps \u0060UseDataVault()\u0060 and \u0060ApplyDataVaultMetadata(metadataModel)\u0060 on the SQLite default while adding provider-aware overloads that store \u0060ProviderProfile\u0060 and call the translator with the selected profile; the unit tests assert both \u0060sqlite-v1\u0060 and \u0060oracle-v1\u0060 outcomes.",
    "Evidence: \u0060src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs\u0060 registers \u0060OracleDataVaultSaveStrategy\u0060 as \u0060IDataVaultProviderSaveStrategy\u0060, and \u0060src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj\u0060 contains only \u0060Microsoft.Extensions.DependencyInjection.Abstractions\u0060 and a project reference to \u0060DCoding.Data.DVault\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060 gates \u0060CanSave\u0060 by exact provider name \u0060Oracle.EntityFrameworkCore\u0060, a clean change tracker, and request batches with zero satellite operations; \u0060src/DCoding.Data.DVault/DataVaultSaveService.cs\u0060 falls back to the core writer whenever no registered strategy returns true.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs\u0060 contains \u0060AddDVaultOracleDeclinesSqliteContextAndFallsBackThroughCoreWriter\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0060 plus \u0060tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs\u0060 cover Oracle package-boundary and API snapshot expectations.",
    "Evidence: This read-only review did not execute \u0060dotnet test DVault.slnx --nologo\u0060 or \u0060bash tools/check-format.sh\u0060, so passing-state evidence for automated coverage, package verification, and unchanged fallback behavior was not directly observed.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/oracle, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil\u0027.",
    "Evidence: Ticket history references implementation commit \u0027695bf4083de7\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: The shared capability-profile surface exposes an Oracle profile that declares mappings for HashKey, HashDiff, LoadTimestamp, RecordSource, ParticipantReference, BusinessKey, and PayloadText, plus explicit unsupported SQL-function and concurrency baselines. (\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs\u0060 defines \u0060DataVaultProviderCapabilityProfiles.Oracle\u0060 with the required seven logical-property mappings and explicit \u0060NoneInV1Unsupported\u0060 SQL-function and concurrency baselines, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs\u0060 asserts that contract.).",
    "AC check passed: There is a supported Oracle model-configuration path that results in Oracle profile annotations and Oracle-native storage metadata on translated properties, while the existing default path still emits the current SQLite baseline. (\u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060 adds provider-aware \u0060UseDataVault(...)\u0060 and \u0060ApplyDataVaultMetadata(..., providerCapabilities)\u0060 overloads while the existing overloads still flow through the SQLite default; \u0060DataVaultModelBuilderExtensionsTests\u0060 and \u0060DataVaultEfMetadataTranslationTests\u0060 assert both the \u0060sqlite-v1\u0060 baseline and Oracle-specific annotations/storage types.).",
    "AC check passed: AddDVaultOracle() wires Oracle provider capability registration through the shared contract, and the Oracle package does not introduce a dependency on any non-Oracle DVault provider package or non-Oracle database provider package. (\u0060src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs\u0060 registers \u0060OracleDataVaultSaveStrategy\u0060 through \u0060IDataVaultProviderSaveStrategy\u0060, and \u0060src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj\u0060 is limited to \u0060Microsoft.Extensions.DependencyInjection.Abstractions\u0060 plus the core \u0060DCoding.Data.DVault\u0060 project reference; \u0060PackageVerifierTests\u0060 also asserts the non-Oracle dependency boundary.).",
    "AC check passed: When the current DbContext or ordered request batch falls outside the Oracle strategy\u0027s supported shape, the strategy declines selection and the dispatcher completes the save through the existing provider-neutral IDataVaultSaveService path. (\u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060 gates selection by exact Oracle provider identity, a clean \u0060DbContext\u0060, and a whole-batch supported-shape check, while \u0060src/DCoding.Data.DVault/DataVaultSaveService.cs\u0060 falls back to the provider-neutral writer when no strategy accepts the batch; \u0060DataVaultSaveStrategySelectionTests\u0060 includes \u0060AddDVaultOracleDeclinesSqliteContextAndFallsBackThroughCoreWriter\u0060.).",
    "DoD check passed: Relevant unit, smoke, or contract tests are added or updated for core and Oracle projects, including the existing assertions that currently treat Oracle as compatibility-only. (Relevant core and Oracle-facing tests were added or updated in \u0060DataVaultProviderCapabilityProfileTests\u0060, \u0060DataVaultEfMetadataTranslationTests\u0060, \u0060DataVaultModelBuilderExtensionsTests\u0060, \u0060ExplicitDataVaultSaveServiceTests\u0060, \u0060DataVaultSaveStrategySelectionTests\u0060, and the integration test project now references \u0060src/DCoding.Data.DVault.Oracle\u0060.).",
    "DoD check passed: Any new public core API surface required for provider selection has approved API snapshot updates and XML documentation. (The new public core surface is reflected in \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060, and the added public members in \u0060DataVaultModelBuilderExtensions.cs\u0060, \u0060DataVaultProviderCapabilities.cs\u0060, and related annotation/constants sources carry XML documentation comments.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Automated coverage proves Oracle profile contents, Oracle registration and selection behavior, fallback behavior, and package or API verification expectations. (The repository contains targeted coverage and snapshot/package-verifier assertions, but this read-only review did not execute \u0060dotnet test DVault.slnx --nologo\u0060 or \u0060bash tools/check-format.sh\u0060, so the coverage is present rather than directly proven passing.).",
    "DoD check failed: Package verification still passes for DCoding.Data.DVault.Oracle, including README, XML documentation, symbol-package, and core-version alignment expectations. (\u0060PackageVerifierTests\u0060 and package metadata files show the Oracle README/XML-docs/symbol/core-version expectations are modeled, but this session did not run package verification, so pass status was not directly observed.).",
    "DoD check failed: Existing SQLite optimized-path behavior and provider-neutral fallback behavior remain unchanged and covered by passing tests. (The repository still contains SQLite optimized-path and provider-neutral fallback coverage, but this definition-of-done item requires passing tests; that passing-state was not directly verified in this read-only review.).",
    "No structural mismatch was found in the required output paths \u0060src/DCoding.Data.DVault\u0060 and \u0060src/DCoding.Data.DVault.Oracle\u0060; the current blocker is verification evidence, not missing implementation wiring.",
    "Acceptance criterion 5 and definition-of-done items 3 and 4 remain unproven until deterministic legacy verification runs the policy commands in a supported environment."
  ],
  "evidence": [
    "\u0060git rev-parse 695bf4083de7\u0060 resolved to \u0060695bf4083de753367ac88317a2ffdd2bf6391e50\u0060, and \u0060git diff --name-only 695bf4083de7..HEAD\u0060 listed only \u0060.gicket/...\u0060 paths, so the claimed commit still matches the branch\u0027s repository implementation.",
    "\u0060git diff --stat develop...695bf4083de7 -- README.md docs src tests\u0060 reported 15 repository files changed with \u0060714 insertions(\u002B), 12 deletions(-)\u0060, including \u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060, \u0060src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs\u0060, \u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060, and the targeted test files.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs\u0060 defines \u0060DataVaultProviderCapabilityProfiles.Oracle\u0060 with profile name \u0060oracle-v1\u0060, \u0060NoneInV1Unsupported\u0060 SQL/concurrency baselines, and mappings using \u0060VARCHAR2(64 CHAR)\u0060, \u0060TIMESTAMP WITH TIME ZONE\u0060, \u0060VARCHAR2(255 CHAR)\u0060, and \u0060CLOB\u0060 for the required logical property kinds.",
    "\u0060src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs\u0060 keeps \u0060UseDataVault()\u0060 and \u0060ApplyDataVaultMetadata(metadataModel)\u0060 on the SQLite default while adding provider-aware overloads that store \u0060ProviderProfile\u0060 and call the translator with the selected profile; the unit tests assert both \u0060sqlite-v1\u0060 and \u0060oracle-v1\u0060 outcomes.",
    "\u0060src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs\u0060 registers \u0060OracleDataVaultSaveStrategy\u0060 as \u0060IDataVaultProviderSaveStrategy\u0060, and \u0060src/DCoding.Data.DVault.Oracle/DCoding.Data.DVault.Oracle.csproj\u0060 contains only \u0060Microsoft.Extensions.DependencyInjection.Abstractions\u0060 and a project reference to \u0060DCoding.Data.DVault\u0060.",
    "\u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060 gates \u0060CanSave\u0060 by exact provider name \u0060Oracle.EntityFrameworkCore\u0060, a clean change tracker, and request batches with zero satellite operations; \u0060src/DCoding.Data.DVault/DataVaultSaveService.cs\u0060 falls back to the core writer whenever no registered strategy returns true.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs\u0060 contains \u0060AddDVaultOracleDeclinesSqliteContextAndFallsBackThroughCoreWriter\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/PackageVerifierTests.cs\u0060 plus \u0060tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs\u0060 cover Oracle package-boundary and API snapshot expectations.",
    "This read-only review did not execute \u0060dotnet test DVault.slnx --nologo\u0060 or \u0060bash tools/check-format.sh\u0060, so passing-state evidence for automated coverage, package verification, and unchanged fallback behavior was not directly observed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/oracle, area/performance, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil\u0027.",
    "Ticket history references implementation commit \u0027695bf4083de7\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Invoke \u0060request-legacy-verification\u0060 for \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 against commit \u0060695bf4083de7\u0060.",
    "If legacy verification passes, rerun the tester gate toward integrator; if it fails, return the failing command evidence to dev."
  ],
  "branchName": "ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil",
  "commitSha": "695bf4083de7"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EZ0NBAP31G489S3YXXYY54WM`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EZ0NBAP31G489S3YXXYY54WM-task-implement-oracle-provider-capability-profil`