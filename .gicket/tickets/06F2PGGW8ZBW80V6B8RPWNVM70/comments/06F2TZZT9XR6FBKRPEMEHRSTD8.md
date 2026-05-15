[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F2PGGW8ZBW80V6B8RPWNVM70\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce\u0027 and commit \u0027d042533ff44d\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce\u0027 from source \u0027d042533ff44d\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce\u0027.",
    "Evidence: git diff --name-only develop...d042533ff44d -- . \u0027:(exclude).gicket\u0027 \u0027:(exclude).gicket-bot\u0027 returned no files; the parent story branch adds no non-ticket repository changes.",
    "Evidence: git merge-base develop ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce returned e0e98c0a9b53cf95f61032dffe1b87206876b136, and git show -s --format=%s e0e98c0a9b53cf95f61032dffe1b87206876b136 returned \u0027[06F2PGH42B6BT1708MYGMXP5GM] AUTO-INTEGRATION squash into develop\u0027.",
    "Evidence: src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs dispatches CreateTable, Add/Drop/Alter/RenameColumn, Create/Drop/RenameIndex, Add/DropPrimaryKey, and DropTable operations, and CreatePath builds migration/\u003COperation\u003E/\u003CTarget\u003E/\u003CMember?\u003E paths.",
    "Evidence: src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs defines only DVM2001 through DVM2006 for migration-guardrail diagnostics.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs contains quiet create-table coverage for HubCustomer, LinkCustomerOrder, SatCustomerContact, PitCustomerContact, BridgeCustomerOrder, and BridgeSalesRegionHierarchy plus deterministic finding-order assertions for create-table and mixed operation matrices.",
    "Evidence: rg -n \u0027RenameIndex\u0027 /mnt/c/Projects/DVault/tests /mnt/c/Projects/DVault/src matched only src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs; no test file matched RenameIndex.",
    "Evidence: git ls-files -- docs/architecture/dvault-dotnet-ef-design-time-workflow.md docs/production-adoption-checklist.md docs/releases/v0.11.0.md returned only the first two paths, and git ls-files docs/releases listed versions through v0.10.0.md only.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/diagnostics, area/ef-core, area/migrations, automation/bot-ready, type/story, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 2 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce\u0027.",
    "Evidence: Ticket history references implementation commit \u0027d042533ff44d\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "AC check passed: The existing consumer-owned \u0060guardrail\u0060 preflight can be used as a blocking CI step because migration diagnostics cover the current DVault structural invariants for \u0060CreateTableOperation\u0060, add/drop/alter/rename-column, default-index, primary-key, and drop-table operations. (DataVaultDesignTimeCommand still routes the existing guardrail verb through DataVaultMigrationOperationDiagnostics.AnalyzeReport(...), and DataVaultMigrationOperationDiagnostics dispatches create-table, column, index, primary-key, and drop-table operations across the claimed matrix.).",
    "AC check passed: Non-DVault tables are ignored, and a DVault migration operation set that matches the current explain baseline for hub, link, satellite, PIT, and bridge tables produces no guardrail findings. (DataVaultMigrationOperationDiagnosticsTests contains a quiet create-table matrix for hub, link, satellite, PIT, and bridge tables plus non-DVault input, and AnalyzeMigrationOperationsKeepsSafeMatrixQuiet keeps the non-finding lane empty for the exercised safe operations.).",
    "AC check passed: Finding-producing operations reuse the current stable \u0060DVM2001\u0060 through \u0060DVM2006\u0060 catalog instead of introducing a new public migration-diagnostic taxonomy. (DataVaultDiagnosticCatalog exposes only DVM2001 through DVM2006 for migration guardrails, and the catalog test asserts that exact code and severity set.).",
    "AC check passed: Guardrail findings keep deterministic \u0060migration/{Operation}/{Target}/{Member?}\u0060 paths and stable report ordering so CI and tests can assert exact output. (CreatePath emits migration/\u003COperation\u003E/\u003CTarget\u003E/\u003CMember?\u003E paths, DataVaultMigrationGuardrailReport preserves deterministic issue order, and the unit tests assert exact path, order, and display output for exercised cases.).",
    "DoD check passed: The story stays bounded to provider-neutral guardrail hardening; consumer-owned command hosting, exit-code behavior, and public command verbs remain unchanged. (git diff from develop to d042533ff44d is empty outside .gicket, and the existing guardrail verb and exit-code behavior in DataVaultDesignTimeCommand remain unchanged.).",
    "DoD check passed: The repository keeps one authoritative migration-guardrail taxonomy through \u0060DVM2001\u0060-\u0060DVM2006\u0060, with any wording updates kept consistent across code, tests, and focused docs. (Code and tests both use the single DVM2001-DVM2006 catalog, and the focused workflow docs still describe the same migration-guardrail family without introducing another taxonomy.).",
    "DoD check passed: Tests cover representative hub, link, satellite, PIT, and bridge cases and assert deterministic code, severity, path, and ordering. (DataVaultMigrationOperationDiagnosticsTests covers representative hub, link, satellite, PIT, and bridge cases and asserts exact code, severity, path, and ordering for the scenarios it exercises.).",
    "DoD check passed: Any documentation touch is limited to guardrail-specific wording or focused workflow guidance and does not duplicate the broader v0.11 documentation task. (The repository contains the focused workflow docs docs/architecture/dvault-dotnet-ef-design-time-workflow.md and docs/production-adoption-checklist.md, while docs/releases/v0.11.0.md is still absent, so this story has not duplicated the broader v0.11 release-note rollout.).",
    "DoD check passed: No additional child split is required for this story beyond the already-materialized child \u006006F2PGH42B6BT1708MYGMXP5GM\u0060 and the existing blocked docs follow-up \u006006F2PGHA0EXJRGDHM4GQM7NPYR\u0060. (git merge-base shows the parent branch sits on child integration commit e0e98c0a9..., and the parent branch adds no non-ticket repository changes, so there is no repository evidence of an additional split beyond the existing child lane.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Automated coverage proves quiet and finding cases for the create-table lane and the existing migration-operation matrix without changing the public command surface or diagnostics API shape. (The implemented matrix is wider than the proven matrix: DataVaultMigrationOperationDiagnostics analyzes RenameIndexOperation, but no test references RenameIndex, and the quiet matrix does not cover the non-finding side of drop-index, add-primary-key, or drop-primary-key lanes. That does not fully prove quiet and finding cases for the existing migration-operation matrix.).",
    "Blocking: tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs does not exercise RenameIndexOperation even though src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs analyzes it as part of the supported guardrail matrix.",
    "Blocking: the current quiet-side matrix does not prove non-finding behavior for the implemented drop-index, add-primary-key, and drop-primary-key lanes, so acceptance criterion 5\u0027s required quiet-and-finding proof is incomplete."
  ],
  "evidence": [
    "git diff --name-only develop...d042533ff44d -- . \u0027:(exclude).gicket\u0027 \u0027:(exclude).gicket-bot\u0027 returned no files; the parent story branch adds no non-ticket repository changes.",
    "git merge-base develop ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce returned e0e98c0a9b53cf95f61032dffe1b87206876b136, and git show -s --format=%s e0e98c0a9b53cf95f61032dffe1b87206876b136 returned \u0027[06F2PGH42B6BT1708MYGMXP5GM] AUTO-INTEGRATION squash into develop\u0027.",
    "src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs dispatches CreateTable, Add/Drop/Alter/RenameColumn, Create/Drop/RenameIndex, Add/DropPrimaryKey, and DropTable operations, and CreatePath builds migration/\u003COperation\u003E/\u003CTarget\u003E/\u003CMember?\u003E paths.",
    "src/DCoding.Data.DVault/DataVaultDiagnosticCatalog.cs defines only DVM2001 through DVM2006 for migration-guardrail diagnostics.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs contains quiet create-table coverage for HubCustomer, LinkCustomerOrder, SatCustomerContact, PitCustomerContact, BridgeCustomerOrder, and BridgeSalesRegionHierarchy plus deterministic finding-order assertions for create-table and mixed operation matrices.",
    "rg -n \u0027RenameIndex\u0027 /mnt/c/Projects/DVault/tests /mnt/c/Projects/DVault/src matched only src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs; no test file matched RenameIndex.",
    "git ls-files -- docs/architecture/dvault-dotnet-ef-design-time-workflow.md docs/production-adoption-checklist.md docs/releases/v0.11.0.md returned only the first two paths, and git ls-files docs/releases listed versions through v0.10.0.md only.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/diagnostics, area/ef-core, area/migrations, automation/bot-ready, type/story, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 2 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic.",
    "Ticket history references implementation branch \u0027ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce\u0027.",
    "Ticket history references implementation commit \u0027d042533ff44d\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Extend tests/DCoding.Data.DVault.Tests/Unit/DataVaultMigrationOperationDiagnosticsTests.cs with RenameIndexOperation cases and quiet cases for the drop-index, add-primary-key, and drop-primary-key lanes already implemented in source.",
    "After extending coverage, rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh in a writable verification environment before handing the story back to test."
  ],
  "branchName": "ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce",
  "commitSha": "d042533ff44d"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F2PGGW8ZBW80V6B8RPWNVM70`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F2PGGW8ZBW80V6B8RPWNVM70-story-harden-migration-guardrails-for-ci-enforce`