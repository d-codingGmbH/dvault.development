[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F2PGPGXMJ3W8FR9JZHH3PJT8\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service\u0027 and commit \u0027915efc9db1ce\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service\u0027 from source \u0027915efc9db1ce\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service\u0027.",
    "Evidence: git -C /mnt/c/Projects/DVault diff --name-only develop...915efc9db1ce -- README.md docs src tests lists README.md, docs/production-adoption-checklist.md, docs/releases/v0.7.0.md, the new bridge-maintenance source files, and the new bridge-maintenance tests/public API snapshot updates.",
    "Evidence: src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:28-30 registers IDataVaultBridgeMaintenanceService in AddDVault() beside IDataVaultSaveService and IDataVaultReadService.",
    "Evidence: src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:8-41 rebuilds by removing existing bridge rows and inserting desired rows, and lines 44-92 implement incremental insert/update maintenance without deletes.",
    "Evidence: src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:404-477 builds hierarchy closure by BFS and records every reached descendant, but there is no guard that skips ancestor==descendant rows.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs:13-64 verifies many-to-many rebuild/incremental maintenance and read-back, lines 68-130 verify hierarchy shortest-depth behavior and shorter-path updates, and lines 134-192 verify registry-backed resolution and missing-metadata failure.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:34-49,727-729,948-950 contains the new public maintenance request/result types, registry request, registry extensions, and IDataVaultBridgeMaintenanceService methods.",
    "Evidence: README.md:253-326 and docs/production-adoption-checklist.md:46-47,77 document explicit caller-invoked bridge maintenance and minimum-hop TraversalDepth semantics.",
    "Evidence: docs/releases/v0.7.0.md:27-28 and 57-61 were edited for bridge maintenance, while ls /mnt/c/Projects/DVault/docs/releases lists v0.5.0.md through v0.14.0.md only and no v0.15.0 release-note file is present.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/maintenance, area/modeling, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis\u0027.",
    "Evidence: Ticket history references implementation commit \u0027915efc9db1ce\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A new explicit public bridge-maintenance surface is added to DCoding.Data.DVault and registered through the normal AddDVault startup path, with naming and request patterns consistent with the existing explicit save and read services. (The branch adds IDataVaultBridgeMaintenanceService, request/result types, registry-backed adapters, AddDVault registration, and public API snapshot entries for the new explicit maintenance surface.).",
    "AC check passed: Full rebuild over a many-to-many bridge recomputes the bridge table from persisted source-link rows and leaves exactly one row per distinct endpoint pair required by the bridge metadata. (DefaultDataVaultBridgeMaintenanceService rebuilds many-to-many bridges from persisted source-link rows and deduplicates endpoint pairs by key, and the SQLite integration test verifies rebuild plus incremental maintenance against BridgeCustomerOrder rows.).",
    "AC check passed: Repeated rebuild or incremental execution over the same additive source state is idempotent, and rebuild and incremental maintenance converge to identical bridge contents for the same persisted source-link state. (The implementation derives deterministic desired-row sets from current persisted source-link state, and the SQLite tests show unchanged incremental reruns plus rebuild/incremental convergence on the exercised states.).",
    "AC check passed: Registry-backed callers can invoke bridge maintenance against the authoritative metadata registry by bridge name, with deterministic failure when the bridge metadata is missing or unsupported. (Registry-backed maintenance is exposed through DataVaultRegistryBridgeMaintenanceRequest and DataVaultBridgeMaintenanceServiceRegistryExtensions; missing bridge metadata is covered by a SQLite failure test and unsupported projection features fail deterministically in the service before maintenance proceeds.).",
    "AC check passed: Existing bridge read APIs continue to work against maintained tables without API regression, and public API snapshot coverage is updated for any new public maintenance types. (The SQLite maintenance tests read maintained bridge rows back through IDataVaultReadService, and the approved public API snapshot includes the new maintenance types and interface members.).",
    "AC check passed: Tests cover many-to-many and hierarchy rebuild and incremental flows, duplicate suppression, shortest-depth selection when multiple hierarchy paths reach the same pair, shorter-path updates, equal-or-longer-path no-ops, registry-backed resolution, and at least one SQLite integration path that proves bridge rows no longer require manual seeding by application code alone. (DataVaultBridgeMaintenanceServiceSqliteTests covers many-to-many and hierarchy rebuild/incremental flows, shortest-depth selection across multiple hierarchy paths, shorter-path updates, equal-depth no-ops, registry-backed resolution, and a SQLite integration path that no longer requires manual bridge-row seeding.).",
    "DoD check passed: Core package code, DI registration, and public API snapshots are updated for the bridge-maintenance surface. (Core package code, AddDVault DI wiring, and the public API snapshot were all updated for the bridge-maintenance surface.).",
    "DoD check passed: Repository documentation reflects the new explicit bridge-maintenance baseline, documents minimum-hop TraversalDepth semantics for hierarchy bridges, and no longer implies that bridge population is only manual once the service exists. (Repository documentation now describes explicit bridge maintenance, minimum-hop TraversalDepth behavior, and the end of the prior manual-only bridge-population limitation.).",
    "DoD check passed: The implementation leaves sibling PIT maintenance, query-API follow-up, provider-specific optimization, and broader adopter documentation scopes untouched except for required compatibility or handoff notes. (The non-ticket code diff is confined to bridge-maintenance source, tests, and related docs; no direct PIT-maintenance, broader query-API, or provider-specific optimization implementation changes were observed.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Full rebuild over a hierarchy bridge recomputes ancestor/descendant closure rows from persisted recursive link rows, persists exactly one row per distinct ancestor/descendant pair, stores positive integer TraversalDepth values equal to the minimum hop count across all currently materialized paths for that pair, treats direct edges as depth 1, and does not introduce effectivity or path-payload semantics. (Hierarchy closure generation records every reachable descendant but never excludes ancestor==descendant, so cyclic source-link data would materialize forbidden self rows and violate the clarified hierarchy bridge contract.).",
    "AC check failed: Incremental bridge maintenance can add missing bridge rows for newly relevant source-link data without requiring a full rebuild. For hierarchy bridges, when later source-link ingestion creates a shorter alternate path for an existing pair, maintenance updates the persisted TraversalDepth to that shorter minimum; equal or longer alternate paths do not change the stored row. (Incremental hierarchy maintenance reuses the same closure-generation path, so although shorter-path updates are implemented, cyclic additions can still create self rows that the contract says not to persist.).",
    "AC check failed: README and the v0.15.0 release-note delta are updated to replace the current read-only bridge limitation with the new explicit caller-invoked maintenance baseline while documenting the minimum-hop TraversalDepth rule for hierarchy bridges. (README.md was updated, but the branch changes docs/releases/v0.7.0.md instead of adding the required v0.15.0 release-note delta; docs/releases contains no v0.15.0 file.).",
    "DoD check failed: Unit and SQLite integration tests pass for both bridge kinds and both maintenance modes, including duplicate-path shortest-depth coverage and shorter-path incremental update coverage for hierarchy bridges. (Relevant unit and SQLite integration tests were added, but this read-only review did not directly execute dotnet test DVault.slnx --nologo, and the uncovered hierarchy self-row defect shows the current suite does not yet close the clarified contract risk.).",
    "Hierarchy closure can materialize forbidden self rows on cyclic source-link data because DefaultDataVaultBridgeMaintenanceService never filters out ancestor==descendant during closure generation (src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:404-477).",
    "Acceptance criterion 9 remains unmet: the branch updates docs/releases/v0.7.0.md, but there is no v0.15.0 release-note delta in docs/releases.",
    "Required executable verification was not directly observed in this read-only review; dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run here."
  ],
  "evidence": [
    "git -C /mnt/c/Projects/DVault diff --name-only develop...915efc9db1ce -- README.md docs src tests lists README.md, docs/production-adoption-checklist.md, docs/releases/v0.7.0.md, the new bridge-maintenance source files, and the new bridge-maintenance tests/public API snapshot updates.",
    "src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:28-30 registers IDataVaultBridgeMaintenanceService in AddDVault() beside IDataVaultSaveService and IDataVaultReadService.",
    "src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:8-41 rebuilds by removing existing bridge rows and inserting desired rows, and lines 44-92 implement incremental insert/update maintenance without deletes.",
    "src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs:404-477 builds hierarchy closure by BFS and records every reached descendant, but there is no guard that skips ancestor==descendant rows.",
    "tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs:13-64 verifies many-to-many rebuild/incremental maintenance and read-back, lines 68-130 verify hierarchy shortest-depth behavior and shorter-path updates, and lines 134-192 verify registry-backed resolution and missing-metadata failure.",
    "tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:34-49,727-729,948-950 contains the new public maintenance request/result types, registry request, registry extensions, and IDataVaultBridgeMaintenanceService methods.",
    "README.md:253-326 and docs/production-adoption-checklist.md:46-47,77 document explicit caller-invoked bridge maintenance and minimum-hop TraversalDepth semantics.",
    "docs/releases/v0.7.0.md:27-28 and 57-61 were edited for bridge maintenance, while ls /mnt/c/Projects/DVault/docs/releases lists v0.5.0.md through v0.14.0.md only and no v0.15.0 release-note file is present.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/maintenance, area/modeling, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis\u0027.",
    "Ticket history references implementation commit \u0027915efc9db1ce\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Fix hierarchy closure maintenance so cyclic recursive-link graphs do not materialize implicit self rows, and add a regression test that proves A-\u003EA rows are not emitted from cycles unless explicitly supported by contract.",
    "Add the required v0.15.0 release-note delta instead of rewriting the historical v0.7.0 notes.",
    "After rework, run deterministic verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported environment."
  ],
  "branchName": "ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service",
  "commitSha": "915efc9db1ce"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F2PGPGXMJ3W8FR9JZHH3PJT8`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service`