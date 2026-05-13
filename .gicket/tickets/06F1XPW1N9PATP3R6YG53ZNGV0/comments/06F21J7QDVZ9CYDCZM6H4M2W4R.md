[gicket-bot] integrator-handoff-v1

```json
{
  "sourceRole": "test",
  "targetRole": "integrator",
  "summary": "Tester verified 4/4 acceptance criteria and 4/4 definition-of-done expectations on branch \u0027ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w\u0027 at commit \u00271e302f658912\u0027.",
  "implementationReference": {
    "branchName": "ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w",
    "commitSha": "1e302f658912",
    "pullRequestReference": null,
    "changeReference": null
  },
  "acceptanceCriteria": [
    {
      "expectation": "A focused unit test added under \u0060tests/DCoding.Data.DVault.Tests/Unit/\u0060 and included in \u0060DVault.slnx\u0060 imports a valid \u0060dvault.model.v1\u0060 artifact with \u0060DataVaultModelArtifactImporter.ImportJson\u0060, configures a \u0060DbContext\u0060 with SQLite in-memory provider selection and \u0060UseDataVaultMetadata(importResult)\u0060, and verifies \u0060DataVaultModelDriftReporter.Compare(importResult, context)\u0060 reports no blocking drift differences.",
      "satisfied": true,
      "reason": "Verification found the added unit test file under tests/DCoding.Data.DVault.Tests/Unit, the test project remains in the existing solution context, and the observed test evidence shows the workflow uses DataVaultModelArtifactImporter.ImportJson, a SQLite-backed DbContext with UseDataVaultMetadata(importResult), and DataVaultModelDriftReporter.Compare(importResult, context); dotnet test DVault.slnx --nologo succeeded."
    },
    {
      "expectation": "Companion invalid-artifact coverage asserts \u0060DataVaultModelImportResult.Diagnostics\u0060 exposes unsupported \u0060schemaVersion\u0060 evidence with code \u0060DMV1002\u0060, category \u0060schema-version\u0060, the chosen logical source path, and JSON Pointer \u0060/schemaVersion\u0060.",
      "satisfied": true,
      "reason": "Verification evidence shows the companion workflow test uses the logical source path models/sales-vault.json and documents/asserts unsupported schemaVersion behavior with DMV1002, category schema-version, and JSON Pointer /schemaVersion."
    },
    {
      "expectation": "\u0060docs/model-first-governance.md\u0060 names the exact repo-root \u0060dotnet test DVault.slnx --nologo --filter ...\u0060 command for the added workflow coverage and states the expected valid and invalid outcomes.",
      "satisfied": true,
      "reason": "docs/model-first-governance.md was verified at the committed revision and explicitly includes the repo-root command \u0060dotnet test DVault.slnx --nologo --filter FullyQualifiedName~DataVaultModelFirstDesignTimeWorkflowTests\u0060, plus separate valid and invalid outcome descriptions."
    },
    {
      "expectation": "The workflow remains design-time-only: it uses SQLite only to choose EF metadata and provider behavior and does not require a live database, external service, new package, or consumer-facing example app.",
      "satisfied": true,
      "reason": "The verified branch delta is limited to documentation, a bounded JSON fixture, and unit-test coverage; no new package, consumer-facing example app, or runtime production code changes were part of the delivered work, which supports the design-time-only constraint."
    }
  ],
  "definitionOfDone": [
    {
      "expectation": "The added workflow coverage runs inside the existing repository test projects and solution.",
      "satisfied": true,
      "reason": "The added workflow coverage lives in the existing repository test project under tests/DCoding.Data.DVault.Tests/Unit, the solution-level test command succeeded, and formatting verification also succeeded."
    },
    {
      "expectation": "\u0060docs/model-first-governance.md\u0060 clearly distinguishes valid drift-clean workflow evidence from invalid \u0060DMV1002\u0060 diagnostic evidence and points reviewers to the exact reproduction command.",
      "satisfied": true,
      "reason": "The governance document explicitly separates the valid drift-clean workflow from the invalid DMV1002 diagnostic case and provides the exact reproduction command reviewers should run."
    },
    {
      "expectation": "The implementation relies on visible public model-first branch surfaces rather than internal helpers or undocumented shortcuts.",
      "satisfied": true,
      "reason": "The structured verification evidence references the visible public surfaces called for by the contract: DataVaultModelArtifactImporter.ImportJson, UseDataVaultMetadata(importResult), DataVaultModelDriftReporter.Compare, and DataVaultModelImportResult.Diagnostics; no evidence indicates reliance on internal helpers or undocumented shortcuts."
    },
    {
      "expectation": "Default runtime behavior for non-model-first consumers remains unchanged.",
      "satisfied": true,
      "reason": "No source/runtime implementation files were changed in the verified branch delta; the committed changes are limited to docs, a model fixture, and tests, which supports unchanged default runtime behavior for non-model-first consumers."
    }
  ],
  "evidence": [
    "Verified repository HEAD commit \u00271e302f658912\u0027 on branch \u0027ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w\u0027.",
    "Committed repository path \u0027docs/model-first-governance.md\u0027 exists at verified commit \u00271e302f658912\u0027.",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: # Model-First Governance Workflow",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Status: v0.7.0 branch documentation",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: This guide describes how teams should use governed \u0060dvault.model.v1\u0060 JSON artifacts alongside the existing Code-First and metadata-first DVault paths. The v0.6.0 release notes rema...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: ## Choose A Declaration Path",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Use Code-First declarations when the Data Vault model is local to one EF model and fits the implemented fluent surface for hubs, hub-parent satellites, multi-active driving keys, a...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Use metadata-first registry-backed metadata when one shared authoritative \u0060DataVaultMetadataModel\u0060 or \u0060DataVaultMetadataRegistry\u0060 should drive EF projection, explicit save requests...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Canonical v1 JSON uses the stable top-level declaration categories \u0060hubs\u0060, \u0060links\u0060, \u0060satellites\u0060, \u0060pits\u0060, and \u0060bridges\u0060, with \u0060naming.policy\u0060 defaulting to \u0060default\u0060 and \u0060loadTimes...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: \u0022loadTimestampStorage\u0022: \u0022provider-default\u0022,",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Store the canonical JSON artifact in source control and review changes like source code. Reviewers should check the exact \u0060schemaVersion\u0060, \u0060naming.policy\u0060, \u0060loadTimestampStorage\u0060, ...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Import the artifact with \u0060DataVaultModelArtifactImporter.ImportJson\u0060 and treat \u0060DataVaultModelImportResult.Diagnostics\u0060 as validation evidence. A valid import exposes \u0060MetadataMode...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Keep \u0060dvault.model.v1\u0060 strict and additive only through an explicit future contract. Current v1 artifacts must use the exact \u0060schemaVersion\u0060, the \u0060default\u0060 naming policy, one of th...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Use model-first governance when the authoritative model should be a reviewed, versioned \u0060dvault.model.v1\u0060 JSON artifact. This path is intended for source-controlled artifact review...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: ## Review Workflow",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Environment.NewLine,",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Export canonical JSON from fluent Code-First declarations or already-materialized metadata with \u0060DataVaultModelArtifactExporter.ExportJson\u0060. The exporter accepts a Code-First decla...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Compare the expected artifact or metadata model against generated/current EF metadata with \u0060DataVaultModelDriftReporter.Compare\u0060. Use the structured differences and \u0060ToDisplayStrin...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: ## Workflow Test Evidence",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: Run the focused design-time workflow coverage from the repository root with:",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: dotnet test DVault.slnx --nologo --filter FullyQualifiedName~DataVaultModelFirstDesignTimeWorkflowTests",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: The valid workflow imports the representative \u0060models/sales-vault.json\u0060 \u0060dvault.model.v1\u0060 fixture with \u0060DataVaultModelArtifactImporter.ImportJson\u0060, configures a SQLite-backed desig...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: The invalid workflow imports the same logical source path, \u0060models/sales-vault.json\u0060, with unsupported \u0060schemaVersion\u0060 value \u0060dvault.model.v2\u0060. The expected invalid outcome is one ...",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: The seeded v1 baseline is the importer/projection family below, in ascending code order. All current entries are \u0060error\u0060 severity.",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: | \u0060DMV1501\u0060 | \u0060capability\u0060 | Unsupported metadata capability | Use only supported \u0060dvault.model.v1\u0060 capabilities or split the model into declarations the current runtime can map. |",
    "Observed committed repository file \u0027docs/model-first-governance.md\u0027: | \u0060DMV1801\u0060 | \u0060projection\u0060 | Artifact projection failed | Review the projection error, adjust the affected declaration, and retry the import before applying metadata. |",
    "Committed repository path \u0027models/sales-vault.json\u0027 exists at verified commit \u00271e302f658912\u0027.",
    "Observed committed repository file \u0027models/sales-vault.json\u0027: {",
    "Observed committed repository file \u0027models/sales-vault.json\u0027: \u0022schemaVersion\u0022: \u0022dvault.model.v1\u0022,",
    "Observed committed repository file \u0027models/sales-vault.json\u0027: \u0022hubs\u0022: [",
    "Observed committed repository file \u0027models/sales-vault.json\u0027: \u0022name\u0022: \u0022Customer\u0022,",
    "Observed committed repository file \u0027models/sales-vault.json\u0027: \u0022businessKeys\u0022: [\u0022CustomerId\u0022, \u0022RegionCode\u0022]",
    "Committed repository path \u0027tests/DCoding.Data.DVault\u0027 exists at verified commit \u00271e302f658912\u0027.",
    "Committed repository path \u0027tests/DCoding.Data.DVault\u0027 is a tracked directory.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault\u0027 contains \u0027tests/DCoding.Data.DVault/README.md\u0027.",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 exists at verified commit \u00271e302f658912\u0027.",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 is a tracked directory.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Unit/ConventionFirstEntryPointCoverageTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultBridgeReadServiceTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstLinkTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultCodeFirstMetadataTranslationTests.cs\u0027.",
    "Observed committed repository directory \u0027tests/DCoding.Data.DVault.Tests/Unit\u0027 contains \u0027tests/DCoding.Data.DVault.Tests/Unit/DCoding.Data.DVault.Tests.Unit.csproj\u0027.",
    "Committed repository path \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs\u0027 exists at verified commit \u00271e302f658912\u0027.",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs\u0027: using Microsoft.EntityFrameworkCore;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs\u0027: using Xunit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs\u0027: namespace DCoding.Data.DVault.Tests.Unit;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs\u0027: public sealed class DataVaultModelFirstDesignTimeWorkflowTests {",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs\u0027: private const string LogicalSourcePath = \u0022models/sales-vault.json\u0022;",
    "Observed committed repository file \u0027tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs\u0027: [Fact]",
    "Committed branch delta contains 3 inspectable repository path(s): Modified: docs/model-first-governance.md, Added: models/sales-vault.json, Added: tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelFirstDesignTimeWorkflowTests.cs.",
    "Test command \u0060dotnet test DVault.slnx --nologo\u0060 succeeded (exit code 0).",
    "Observed stdout: Determining projects to restore...",
    "Observed stdout: Restored C:\\Projects\\DVault\\tools\\DCoding.Data.DVault.PackageVerification\\DCoding.Data.DVault.PackageVerification.csproj (in 101 ms).",
    "Observed stdout: Restored C:\\Projects\\DVault\\src\\DCoding.Data\\DCoding.Data.csproj (in 101 ms).",
    "Test command \u0060bash tools/check-format.sh\u0060 succeeded (exit code 0).",
    "Observed stdout: One-member-per-file check passed for 122 packable source files.",
    "Observed stdout: Formatting check passed.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/design-time, area/examples, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F1XPS7KGKBP5SVMQPJC49J2G-story-establish-stable-dvault-diagnostic-codes\u0027.",
    "Ticket history references implementation commit \u00271e302f658912\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "findings": [],
  "nextSteps": [
    "Hand off to the integrator role for final acceptance using branch ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w at commit 1e302f658912."
  ]
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-integrator`
- transaction-point: `TP4`
- ticket-id: `06F1XPW1N9PATP3R6YG53ZNGV0`
- target-role: `integrator`
- verification-summary: Tester verified 4/4 acceptance criteria and 4/4 definition-of-done expectations on branch 'ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w' at commit '1e302f658912'.
- acceptance-criteria: `4/4` satisfied
- definition-of-done: `4/4` satisfied
- implementation-branch: `ticket/06F1XPW1N9PATP3R6YG53ZNGV0-task-wire-design-time-validation-into-a-sample-w`
- implementation-commit: `1e302f658912`
- implementation-pr: `<none>`
- implementation-change: `<none>`