[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F1XPV0YJ8Z9HQVT6BYR397Q8\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027 and commit \u0027a826ca3708a3\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027 from source \u0027a826ca3708a3\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027.",
    "Evidence: git log shows current branch contains implementation commit a826ca3708a3 followed by orchestration commits; git show --name-only a826ca3708a3 lists only DataVaultDiagnosticCatalog.cs, DataVaultMigrationOperationDiagnostics.cs, DataVaultDiagnosticsIntegrationTests.cs, and DataVaultMigrationOperationDiagnosticsTests.cs as implementation files.",
    "Evidence: git diff --name-status develop...a826ca3708a3 for required test paths shows M tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs and A tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs; tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs is absent from the diff.",
    "Evidence: git grep for MigrationOperation/AddColumnOperation/etc. in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs at a826ca3708a3 returned no matches.",
    "Evidence: DataVaultMigrationOperationDiagnostics.cs derives owned entities from baseline.Explain, handles AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn, and appends DataVaultDiagnosticsIssue entries to DataVaultDiagnosticsResult.Issues.",
    "Evidence: DataVaultDiagnosticCatalog.cs contains DVM2001-DVM2006 entries with severities error,error,error,warning,warning,error.",
    "Evidence: DataVaultMigrationOperationDiagnosticsTests.cs asserts safe matrix no issues, deterministic finding order, code, severity, exact path, invariant text, and remediation lookup.",
    "Evidence: DataVaultDiagnosticsIntegrationTests.cs adds AnalyzeSqliteDbContextMigrationOperationsSurfacesFindingsThroughResultIssues for DVM2006 through result.Issues.",
    "Evidence: git diff --name-only develop...a826ca3708a3 -- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi src/DCoding.Data.DVault returned only source implementation files and no public API snapshot files.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/migrations, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
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
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027.",
    "Evidence: Ticket history references implementation commit \u0027a826ca3708a3\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Safe examples produce no issues: satellite payload AddColumn, satellite non-key payload DropColumn, non-DVault DropTable, satellite payload RenameColumn, supplemental non-DVault CreateIndex, and non-key satellite payload AlterColumn. (Safe examples for AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn are present and assert Assert.Empty(result.Issues).).",
    "AC check passed: Finding examples emit deterministic issues using these expectations: AddColumn to Hub* or Link* descriptive payload emits DVM2001 error; Drop or Alter LoadTimestamp, RecordSource, or HashDiff emits DVM2002 error; Drop or Alter hash-key, participant, parent, or driving-key shape emits DVM2003 error; wrong DVault default index semantics emit DVM2004 warning; rename of a DVault-owned produced column emits DVM2005 warning; drop of a DVault-produced table emits DVM2006 error. (Finding examples assert DVM2001, DVM2002, DVM2003, DVM2004, DVM2005, and DVM2006 with expected severities and deterministic order; implementation maps those codes in DataVaultMigrationOperationDiagnostics.cs.).",
    "AC check passed: Issue Path values use the stable migration/{OperationType}/{Target}/{Member?} format so tests can assert exact location strings. (CreatePath emits migration/{OperationType}/{Target}/{Member?}, and tests assert exact paths such as migration/AddColumn/HubCustomer/CustomerStatus and migration/DropTable/HubCustomer.).",
    "AC check passed: Findings surface through DataVaultDiagnosticsResult.Issues and tests assert code, severity, path, and catalog remediation by code lookup without adding new public fields. (Findings are appended to DataVaultDiagnosticsResult.Issues; unit and integration tests assert code, severity, path, and remediation lookup through DataVaultDiagnosticCatalog.GetMigrationOperationDefinition.).",
    "AC check passed: Public API snapshot remains unchanged for IDataVaultDiagnosticsService, DataVaultDiagnosticsIssue, and DataVaultDiagnosticsResult. (git diff against develop shows no changes to DataVaultDiagnostics.cs or the approved public API snapshot paths.).",
    "AC check passed: The ticket contract no longer states that 06F1XPS7KGKBP5SVMQPJC49J2G blocks this ticket. (The ticket description states the stale blocks relation was removed, and no relation file for 06F1XPS7KGKBP5SVMQPJC49J2G blocking this ticket exists at a826ca3708a3.).",
    "DoD check passed: Migration diagnostic catalog entries DVM2001 through DVM2006 exist with stable code-to-severity and remediation mappings. (DataVaultDiagnosticCatalog.cs defines DVM2001 through DVM2006 with error/warning severity mappings and remediation text.).",
    "DoD check passed: No public diagnostics API or approved API snapshot changes are required for this ticket. (No public diagnostics service, issue/result shape, or approved public API snapshot changes were observed.).",
    "DoD check passed: Live relation state remains consistent with the contract: this ticket is no longer blocked by 06F1XPS7KGKBP5SVMQPJC49J2G. (git ls-tree/grep at a826ca3708a3 found no live blocks relation from 06F1XPS7KGKBP5SVMQPJC49J2G to this ticket, and the contract text says it was removed.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Fixtures cover AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn with at least one safe and one finding-producing example each, and every finding example names the governing invariant and expected DVM code. (The new sibling file tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs covers the six-operation matrix, but the required repository output path tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs was not changed and contains no migration-operation fixtures.).",
    "DoD check failed: Implementation contains the migration-operation validator, deterministic fixtures, and tests for the six supported operations and named invariants. (The validator and tests exist, but the required unit diagnostics test artifact path was not delivered; migration fixtures live only in the new DataVaultMigrationOperationDiagnosticsTests.cs file.).",
    "DoD check failed: Repository tests covering diagnostics and schema expectations pass with exact assertions over issue ordering, code, severity, path, and remediation lookup. (dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run in this read-only session; the review also found a required-path delivery blocker before legacy verification would be sufficient.).",
    "Blocking required artifact mismatch: the authoritative required output path tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs was not changed and contains no migration-operation fixtures; the implementation placed the unit proof in a new sibling file instead.",
    "Policy verification remains unproven in this read-only review: dotnet test DVault.slnx --nologo and bash tools/check-format.sh require the supported executable verification environment."
  ],
  "evidence": [
    "git log shows current branch contains implementation commit a826ca3708a3 followed by orchestration commits; git show --name-only a826ca3708a3 lists only DataVaultDiagnosticCatalog.cs, DataVaultMigrationOperationDiagnostics.cs, DataVaultDiagnosticsIntegrationTests.cs, and DataVaultMigrationOperationDiagnosticsTests.cs as implementation files.",
    "git diff --name-status develop...a826ca3708a3 for required test paths shows M tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs and A tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs; tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs is absent from the diff.",
    "git grep for MigrationOperation/AddColumnOperation/etc. in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs at a826ca3708a3 returned no matches.",
    "DataVaultMigrationOperationDiagnostics.cs derives owned entities from baseline.Explain, handles AddColumn, DropColumn, DropTable, RenameColumn, CreateIndex, and AlterColumn, and appends DataVaultDiagnosticsIssue entries to DataVaultDiagnosticsResult.Issues.",
    "DataVaultDiagnosticCatalog.cs contains DVM2001-DVM2006 entries with severities error,error,error,warning,warning,error.",
    "DataVaultMigrationOperationDiagnosticsTests.cs asserts safe matrix no issues, deterministic finding order, code, severity, exact path, invariant text, and remediation lookup.",
    "DataVaultDiagnosticsIntegrationTests.cs adds AnalyzeSqliteDbContextMigrationOperationsSurfacesFindingsThroughResultIssues for DVM2006 through result.Issues.",
    "git diff --name-only develop...a826ca3708a3 -- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi src/DCoding.Data.DVault returned only source implementation files and no public API snapshot files.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/migrations, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.2].",
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
    "Ticket history references implementation branch \u0027ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu\u0027.",
    "Ticket history references implementation commit \u0027a826ca3708a3\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Move or integrate the migration-operation unit fixtures into tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, or update the authoritative required output contract before test handoff.",
    "After the required-path blocker is fixed, run deterministic legacy verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu",
  "commitSha": "a826ca3708a3"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F1XPV0YJ8Z9HQVT6BYR397Q8`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F1XPV0YJ8Z9HQVT6BYR397Q8-task-validate-migration-operations-and-report-gu`