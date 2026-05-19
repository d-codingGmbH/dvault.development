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
    "Selected verification source branch \u0027ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service\u0027 and commit \u0027e863f196856b\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service\u0027 from source \u0027e863f196856b\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service\u0027.",
    "Evidence: git -C /mnt/c/Projects/DVault diff --name-only develop...e863f196856b lists README.md, docs/production-adoption-checklist.md, docs/releases/v0.7.0.md, bridge-maintenance source files, tests, and the public API snapshot; it does not list docs/releases/v0.15.0.md.",
    "Evidence: git -C /mnt/c/Projects/DVault ls-files docs/releases lists v0.5.0.md through v0.14.0.md only.",
    "Evidence: git -C /mnt/c/Projects/DVault show --stat --oneline --summary e863f196856b shows the latest handoff commit changes only src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs.",
    "Evidence: src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers IDataVaultBridgeMaintenanceService in AddDVault().",
    "Evidence: src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs rebuilds by removing existing bridge rows and inserting desired rows; incremental maintenance inserts missing rows and only lowers TraversalDepth when a shorter path is computed.",
    "Evidence: src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs seeds hierarchy traversal with the starting ancestor at depth 0 and returns only depths greater than 0, preventing implicit self rows.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs asserts many-to-many rebuild/incremental behavior, hierarchy shortest-depth and shorter-path update behavior, registry-backed resolution, and a cycle case with exactly four non-self rows.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs includes DataVaultBridgeMaintenanceServiceSqliteTests in RequiredLocalSqliteCoverageTypes.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt contains DataVaultBridgeMaintenanceRequest, DataVaultBridgeMaintenanceResult, DataVaultRegistryBridgeMaintenanceRequest, registry extensions, and IDataVaultBridgeMaintenanceService.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/maintenance, area/modeling, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis\u0027.",
    "Evidence: Ticket history references implementation commit \u0027e863f196856b\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: A new explicit public bridge-maintenance surface is added to DCoding.Data.DVault and registered through the normal AddDVault startup path, with naming and request patterns consistent with the existing explicit save and read services. (src/DCoding.Data.DVault/IDataVaultBridgeMaintenanceService.cs, the request/result types, registry extensions, and DVaultServiceCollectionExtensions.AddDVault() provide an explicit public bridge-maintenance surface and DI registration beside the save/read services.).",
    "AC check passed: Full rebuild over a many-to-many bridge recomputes the bridge table from persisted source-link rows and leaves exactly one row per distinct endpoint pair required by the bridge metadata. (DefaultDataVaultBridgeMaintenanceService.RebuildBridgeAsync() deletes existing bridge rows and inserts CreateManyToManyDesiredRows() output keyed by endpoint pair, and the SQLite maintenance test asserts one row per distinct customer/order pair.).",
    "AC check passed: Full rebuild over a hierarchy bridge recomputes ancestor/descendant closure rows from persisted recursive link rows, persists exactly one row per distinct ancestor/descendant pair, stores positive integer TraversalDepth values equal to the minimum hop count across all currently materialized paths for that pair, treats direct edges as depth 1, and does not introduce effectivity or path-payload semantics. (CreateHierarchyDesiredRows() and GetShortestDescendantDepths() compute minimum-hop closure, seed the starting ancestor at depth 0, and return only positive depths; the cycle regression test asserts only non-self rows with direct edges at depth 1 and indirect depth 2.).",
    "AC check passed: Incremental bridge maintenance can add missing bridge rows for newly relevant source-link data without requiring a full rebuild. For hierarchy bridges, when later source-link ingestion creates a shorter alternate path for an existing pair, maintenance updates the persisted TraversalDepth to that shorter minimum; equal or longer alternate paths do not change the stored row. (MaintainBridgeAsync() inserts missing rows and updates TraversalDepth only when the persisted value is greater than the newly computed minimum; the SQLite hierarchy test covers equal-depth no-op and shorter-path update behavior.).",
    "AC check passed: Repeated rebuild or incremental execution over the same additive source state is idempotent, and rebuild and incremental maintenance converge to identical bridge contents for the same persisted source-link state. (The many-to-many test reruns maintenance on unchanged source state and gets only RowsUnchanged, and the hierarchy test compares incremental contents with a subsequent rebuild and expects them to match.).",
    "AC check passed: Registry-backed callers can invoke bridge maintenance against the authoritative metadata registry by bridge name, with deterministic failure when the bridge metadata is missing or unsupported. (DataVaultBridgeMaintenanceServiceRegistryExtensions resolves bridge metadata from the authoritative registry, and the SQLite tests cover both successful name-based resolution and deterministic missing-metadata failure.).",
    "AC check passed: Existing bridge read APIs continue to work against maintained tables without API regression, and public API snapshot coverage is updated for any new public maintenance types. (The SQLite maintenance tests read maintained bridge rows through existing IDataVaultReadService APIs, and the public API snapshot contains the new maintenance interface and DTOs.).",
    "AC check passed: Tests cover many-to-many and hierarchy rebuild and incremental flows, duplicate suppression, shortest-depth selection when multiple hierarchy paths reach the same pair, shorter-path updates, equal-or-longer-path no-ops, registry-backed resolution, and at least one SQLite integration path that proves bridge rows no longer require manual seeding by application code alone. (The SQLite suite covers many-to-many and hierarchy rebuild/incremental flows, one-row-per-pair assertions, shortest-depth selection across multiple paths, equal-or-longer no-op, shorter-path updates, registry-backed resolution, and a cycle regression; ProviderIntegrationCategoryDiscoveryTests includes the suite in required SQLite coverage.).",
    "DoD check passed: Core package code, DI registration, and public API snapshots are updated for the bridge-maintenance surface. (Core bridge-maintenance code, AddDVault() registration, and the public API snapshot were all updated in the observed branch diff.).",
    "DoD check passed: The implementation leaves sibling PIT maintenance, query-API follow-up, provider-specific optimization, and broader adopter documentation scopes untouched except for required compatibility or handoff notes. (Observed source changes stay focused on bridge-maintenance service/types/tests plus compatibility documentation; no PIT-maintenance, query-API redesign, or provider-specific optimization source changes were observed.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: README and the v0.15.0 release-note delta are updated to replace the current read-only bridge limitation with the new explicit caller-invoked maintenance baseline while documenting the minimum-hop TraversalDepth rule for hierarchy bridges. (README.md now documents explicit bridge maintenance, but branch-diff and docs/releases listing show only docs/releases/v0.7.0.md was edited; no docs/releases/v0.15.0.md release-note delta exists.).",
    "DoD check failed: Unit and SQLite integration tests pass for both bridge kinds and both maintenance modes, including duplicate-path shortest-depth coverage and shorter-path incremental update coverage for hierarchy bridges. (Relevant unit and SQLite integration test files are present, but this read-only tester review did not execute dotnet test DVault.slnx --nologo, so passing verification is not directly confirmed.).",
    "DoD check failed: Repository documentation reflects the new explicit bridge-maintenance baseline, documents minimum-hop TraversalDepth semantics for hierarchy bridges, and no longer implies that bridge population is only manual once the service exists. (README.md reflects the new explicit bridge-maintenance baseline, but the required v0.15.0 release-note delta is absent and the branch edits historical docs/releases/v0.7.0.md instead.).",
    "Acceptance criterion 9 remains unmet: the branch still has no docs/releases/v0.15.0.md artifact, so editing docs/releases/v0.7.0.md does not satisfy the persisted v0.15.0 release-note requirement.",
    "Definition of done 2 is not yet closed from direct tester evidence because dotnet test DVault.slnx --nologo was not executed in this read-only interactive review path."
  ],
  "evidence": [
    "git -C /mnt/c/Projects/DVault diff --name-only develop...e863f196856b lists README.md, docs/production-adoption-checklist.md, docs/releases/v0.7.0.md, bridge-maintenance source files, tests, and the public API snapshot; it does not list docs/releases/v0.15.0.md.",
    "git -C /mnt/c/Projects/DVault ls-files docs/releases lists v0.5.0.md through v0.14.0.md only.",
    "git -C /mnt/c/Projects/DVault show --stat --oneline --summary e863f196856b shows the latest handoff commit changes only src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs.",
    "src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers IDataVaultBridgeMaintenanceService in AddDVault().",
    "src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs rebuilds by removing existing bridge rows and inserting desired rows; incremental maintenance inserts missing rows and only lowers TraversalDepth when a shorter path is computed.",
    "src/DCoding.Data.DVault/DefaultDataVaultBridgeMaintenanceService.cs seeds hierarchy traversal with the starting ancestor at depth 0 and returns only depths greater than 0, preventing implicit self rows.",
    "tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs asserts many-to-many rebuild/incremental behavior, hierarchy shortest-depth and shorter-path update behavior, registry-backed resolution, and a cycle case with exactly four non-self rows.",
    "tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs includes DataVaultBridgeMaintenanceServiceSqliteTests in RequiredLocalSqliteCoverageTypes.",
    "tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt contains DataVaultBridgeMaintenanceRequest, DataVaultBridgeMaintenanceResult, DataVaultRegistryBridgeMaintenanceRequest, registry extensions, and IDataVaultBridgeMaintenanceService.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/maintenance, area/modeling, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis\u0027.",
    "Ticket history references implementation commit \u0027e863f196856b\u0027.",
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
    "Add the required docs/releases/v0.15.0.md release-note delta with the explicit caller-invoked bridge-maintenance baseline and minimum-hop hierarchy TraversalDepth semantics instead of relying on the historical v0.7.0 notes.",
    "After the release-note artifact is corrected, rerun tester verification and execute dotnet test DVault.slnx --nologo plus bash tools/check-format.sh through the supported legacy verification path."
  ],
  "branchName": "ticket/06F2PGPGXMJ3W8FR9JZHH3PJT8-story-add-bridge-maintenance-service",
  "commitSha": "e863f196856b"
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