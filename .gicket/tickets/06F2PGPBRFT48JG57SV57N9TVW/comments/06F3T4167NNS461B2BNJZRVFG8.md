[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F2PGPBRFT48JG57SV57N9TVW\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F2PGPBRFT48JG57SV57N9TVW-story-add-pit-maintenance-service\u0027 and commit \u002739155f4ce85a\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F2PGPBRFT48JG57SV57N9TVW-story-add-pit-maintenance-service\u0027 from source \u002739155f4ce85a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F2PGPBRFT48JG57SV57N9TVW-story-add-pit-maintenance-service\u0027.",
    "Evidence: \u0060git diff --name-status develop...39155f4ce85a\u0060 adds \u0060src/DCoding.Data.DVault/DataVaultPitMaintenanceService.cs\u0060, \u0060src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs\u0060, PIT request/result types, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs\u0060, and \u0060docs/plans/pit-maintenance-service-v1-contract.md\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs\u0060 adds \u0060TryAddSingleton(services, typeof(IDataVaultPitMaintenanceService), typeof(DefaultDataVaultPitMaintenanceService));\u0060 beside the existing save/read registrations.",
    "Evidence: \u0060src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs\u0060 rebuilds PIT rows by collecting satellite timestamps, ordering distinct timestamps ascending, setting each \u0060\u003CSatellite\u003ELoadTimestamp\u0060 to the latest visible satellite row at or before the PIT timestamp, deleting old PIT rows, and inserting the regenerated set.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs\u0060 asserts rebuild deletes one stale PIT row and writes three deterministic PIT rows with null snapshot handling, and asserts parent-scoped maintenance rewrites only one parent while correcting late-arriving profile history.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs\u0060 verifies parent-key deduplication, empty-parent no-op, unsupported link/multi-active PIT rejection, and missing snapshot-property failure, but contains no unit assertion for deterministic PIT row generation.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 includes new public API entries for the maintenance interface and PIT request/result types.",
    "Evidence: \u0060docs/plans/pit-maintenance-service-v1-contract.md\u0060 was added and names related tickets \u006006F2PGPKXWRFXNPFA1JR0X67XC\u0060, \u006006F2PGPRGN0EVGD6RY5KY9M56W\u0060, and \u006006F2PGPXVAYRBC94RQ7X5V4DVG\u0060; \u0060docs/releases/v0.7.0.md\u0060 already states PIT-backed reads expect already materialized PIT rows and do not maintain or refresh them.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/maintenance, area/persistence, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis\u0027.",
    "Evidence: Ticket history references implementation commit \u002739155f4ce85a\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The core package exposes an additive explicit PIT maintenance surface for one DataVaultPitMetadata target without hiding PIT refresh behind SaveChanges, interceptors, or background automation. (The branch adds \u0060IDataVaultPitMaintenanceService\u0060 plus additive request/result types and registers the service in \u0060AddDVault()\u0060 beside the existing explicit save/read services; the diff does not modify save-service, read-service, interceptor, or background-automation files.).",
    "AC check passed: Full rebuild deletes and regenerates the complete PIT contents for one declared PIT table using the authoritative row-generation rule from docs/plans/pit-maintenance-service-v1-contract.md. (\u0060DefaultDataVaultPitMaintenanceService.RebuildAsync(...)\u0060 reads satellite rows, builds PIT rows from distinct ascending satellite \u0060LoadTimestamp\u0060 values, deletes existing PIT rows for the target PIT table, and inserts the regenerated set; the SQLite rebuild test asserts stale-row replacement and the expected deterministic PIT rows.).",
    "AC check passed: Incremental maintenance accepts explicit parent hash keys, treats empty input as a no-op, recomputes complete PIT history for only those parents, and replaces existing PIT rows for those parents so late-arriving satellite rows correct prior PIT history. (\u0060MaintainParentsAsync(...)\u0060 accepts explicit parent hash keys, returns a no-op for empty input, deletes existing PIT rows only for the requested parents, and rewrites complete PIT history for those parents; unit coverage verifies the empty-input no-op and SQLite coverage verifies late-arriving correction for one parent without rewriting another parent.).",
    "AC check passed: The supported v1 shape is enforced before writes: hub-parent DataVaultPitMetadata only, non-multi-active hub-attached satellites only, unique declared satellites, and the translated PIT entity and columns must match the existing ParentHashKey, LoadTimestamp, and \u003CSatellite\u003ELoadTimestamp contract. (\u0060ValidatePitShape(...)\u0060, generated-entity/property validation, and satellite parent checks enforce hub-parent PITs, unique non-multi-active satellites, and the expected generated PIT/satellite column contract before writes; unit coverage also verifies failure on unsupported PIT shapes and a missing snapshot-reference property.).",
    "AC check passed: The result remains compatible with the existing PIT read contract so downstream tickets can assume maintained PIT tables without redefining PIT row-population semantics. (The diff leaves \u0060IDataVaultReadService\u0060 and \u0060DataVaultPitReadPipeline\u0060 unchanged, and the SQLite tests read maintained PIT rows back through \u0060IDataVaultReadService.ReadPitRowsAsync(...)\u0060, showing the new maintenance surface stays compatible with the existing PIT read contract.).",
    "DoD check passed: Public DI registration, request and result types, and any convenience adapters are additive, snapshot-covered as needed, and do not break the existing explicit save and read surface. (Public DI wiring and public API snapshot entries were added for \u0060IDataVaultPitMaintenanceService\u0060, \u0060DataVaultPitRebuildRequest\u0060, \u0060DataVaultPitParentMaintenanceRequest\u0060, and \u0060DataVaultPitMaintenanceResult\u0060 without changing the existing explicit save/read interfaces.).",
    "DoD check passed: SQLite integration tests cover full rebuild, bounded parent maintenance, missing satellite snapshots, and late-arriving satellite corrections. (\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs\u0060 covers full rebuild, bounded parent maintenance, missing satellite snapshots, and late-arriving satellite corrections, and \u0060ProviderIntegrationCategoryDiscoveryTests\u0060 now includes the new SQLite integration class in required local provider coverage.).",
    "DoD check passed: The implementation leaves clear handoff evidence for 06F2PGPKXWRFXNPFA1JR0X67XC, 06F2PGPRGN0EVGD6RY5KY9M56W, and 06F2PGPXVAYRBC94RQ7X5V4DVG without reopening PIT maintenance semantics. (\u0060docs/plans/pit-maintenance-service-v1-contract.md\u0060 was added with the related downstream ticket IDs and explicit cross-ticket boundaries, and \u0060docs/plans/README.md\u0060 now indexes that contract for handoff traceability.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Unit tests cover validation failures and deterministic row-generation behavior. (\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs\u0060 covers request validation, additive DI wiring, empty-input no-op, unsupported PIT shapes, and missing generated snapshot properties, but it does not unit-test deterministic PIT row-generation ordering or snapshot backfill behavior; that behavior is only exercised in SQLite integration tests.).",
    "Definition of done item 2 is not met: deterministic PIT row-generation behavior is covered only in \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs\u0060, not in unit tests as the persisted contract requires.",
    "No executable verification was run in this read-only review, so \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 still need deterministic execution after rework."
  ],
  "evidence": [
    "\u0060git diff --name-status develop...39155f4ce85a\u0060 adds \u0060src/DCoding.Data.DVault/DataVaultPitMaintenanceService.cs\u0060, \u0060src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs\u0060, PIT request/result types, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs\u0060, and \u0060docs/plans/pit-maintenance-service-v1-contract.md\u0060.",
    "\u0060src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs\u0060 adds \u0060TryAddSingleton(services, typeof(IDataVaultPitMaintenanceService), typeof(DefaultDataVaultPitMaintenanceService));\u0060 beside the existing save/read registrations.",
    "\u0060src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs\u0060 rebuilds PIT rows by collecting satellite timestamps, ordering distinct timestamps ascending, setting each \u0060\u003CSatellite\u003ELoadTimestamp\u0060 to the latest visible satellite row at or before the PIT timestamp, deleting old PIT rows, and inserting the regenerated set.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs\u0060 asserts rebuild deletes one stale PIT row and writes three deterministic PIT rows with null snapshot handling, and asserts parent-scoped maintenance rewrites only one parent while correcting late-arriving profile history.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs\u0060 verifies parent-key deduplication, empty-parent no-op, unsupported link/multi-active PIT rejection, and missing snapshot-property failure, but contains no unit assertion for deterministic PIT row generation.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 includes new public API entries for the maintenance interface and PIT request/result types.",
    "\u0060docs/plans/pit-maintenance-service-v1-contract.md\u0060 was added and names related tickets \u006006F2PGPKXWRFXNPFA1JR0X67XC\u0060, \u006006F2PGPRGN0EVGD6RY5KY9M56W\u0060, and \u006006F2PGPXVAYRBC94RQ7X5V4DVG\u0060; \u0060docs/releases/v0.7.0.md\u0060 already states PIT-backed reads expect already materialized PIT rows and do not maintain or refresh them.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/maintenance, area/persistence, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 3 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F2PGPKXWRFXNPFA1JR0X67XC-story-improve-current-and-as-of-query-apis\u0027.",
    "Ticket history references implementation commit \u002739155f4ce85a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add a unit-level PIT maintenance test that directly proves deterministic row generation from distinct ascending satellite timestamps and snapshot backfill selection, instead of relying only on SQLite integration coverage.",
    "After adding the missing unit coverage, run \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 through the supported verification path."
  ],
  "branchName": "ticket/06F2PGPBRFT48JG57SV57N9TVW-story-add-pit-maintenance-service",
  "commitSha": "39155f4ce85a"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F2PGPBRFT48JG57SV57N9TVW`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F2PGPBRFT48JG57SV57N9TVW-story-add-pit-maintenance-service`