[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F5Q9102970H1VQN16QWRGQX0\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F5Q9102970H1VQN16QWRGQX0-story-support-pit-over-multi-active-satellites\u0027 and commit \u0027fb551d98db5a\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F5Q9102970H1VQN16QWRGQX0-story-support-pit-over-multi-active-satellites\u0027 from source \u0027fb551d98db5a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F5Q9102970H1VQN16QWRGQX0-story-support-pit-over-multi-active-satellites\u0027.",
    "Evidence: git diff --name-only develop...fb551d98db5a shows coordinated PIT changes across README.md, docs/plans/pit-backed-as-of-read-api-contract.md, docs/plans/pit-maintenance-service-v1-contract.md, docs/production-adoption-checklist.md, docs/releases/v0.20.0.md, src/DCoding.Data.DVault* PIT translation/maintenance/read files, and PIT-related tests/snapshots.",
    "Evidence: src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs adds PIT driving-key columns and widens the PIT primary key, but tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs still asserts Assert.Empty(pitEntity.GetIndexes()) for the multi-active PIT entity PitCustomerContactStatus.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs adds PitMaintenanceRebuildsAndReadsMultiActiveTupleRowsThroughSqliteFallback, and that test asserts ProviderNeutralFallback diagnostics plus tuple-specific Contact Type projection.",
    "Evidence: The only SQLite MaintainParentsAsync(...) integration tests in tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs are PitMaintenanceMaintainsOnlyRequestedParentsAndCorrectsLateArrivingSatelliteHistoryThroughSqlite and RegistryBackedPitMaintenanceMaintainsParentsByClrMappingThroughSqlite, both ordinary PIT cases.",
    "Evidence: A repository search over tests/DCoding.Data.DVault.Tests/Integration found no SQLite PIT integration case asserting incompatible multi-active rejection text such as do not match multi-active satellite.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs still validates the ordinary PIT read shape PitCustomerProfileStatus; there is no tuple-aware assertion for pitDrivingKeyProjection, referenced-satellite DrivingKeyColumnNames, or the tuple-aware PIT explanation text.",
    "Evidence: No executable build/test/format commands were run in this read-only review session; the decision is based on branch diff and file inspection.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/ef-core, area/maintenance, area/modeling, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F5Q9102970H1VQN16QWRGQX0-story-support-pit-over-multi-active-satellites\u0027.",
    "Evidence: Ticket history references implementation commit \u0027fb551d98db5a\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Rebuild and \u0060MaintainParentsAsync(...)\u0060 compute PIT history per \u0060(parentHashKey, drivingKeyTuple)\u0060 using the current distinct-timestamp and carry-forward rule: tuple-qualified multi-active rows participate only in their own tuple series, ordinary satellites remain parent-wide, and no tuple series is collapsed into another. (DefaultDataVaultPitMaintenanceService now routes tuple-aware PITs through CreateTupleAwarePitRows(...), and DataVaultPitMaintenanceRowGenerationTests plus the SQLite rebuild/read test show distinct-timestamp carry-forward per (parentHashKey, drivingKeyTuple) with ordinary satellites remaining parent-wide.).",
    "AC check passed: PIT-backed reads keep the existing parent-hash-key request surface but return every visible tuple row for the requested parents at the \u0060asOf\u0060 cutoff; each read record and typed PIT projection exposes the canonical driving-key values so same-parent results remain unambiguous. (DataVaultPitReadPipeline/DataVaultPitReadRecord now expose canonical driving-key values, and PitMaintenanceRebuildsAndReadsMultiActiveTupleRowsThroughSqliteFallback verifies parent-only requests return separate tuple rows and typed PIT projections can read Contact Type.).",
    "AC check passed: Before translation, maintenance, or read execution, deterministic failures identify unsupported or ambiguous shapes such as link-parent PITs, duplicate satellite references, multi-active references with incompatible driving-key sets or order, reference metadata that contradicts the resolved satellite metadata, and any shape that would require cross-product tuple semantics. (Translation, maintenance, and read paths now fail deterministically for contradictory IsMultiActive metadata, duplicate references, reserved-name collisions, link-parent shapes, and incompatible shared driving-key families; unit coverage was added in DataVaultEfMetadataTranslationTests, DataVaultPitMaintenanceServiceTests, and DataVaultPitReadServiceTests.).",
    "DoD check passed: Public PIT read and maintenance surfaces, typed PIT projection helpers, and approval snapshots are updated additively without regressing ordinary PIT callers. (Public PIT read surfaces were extended additively with DrivingKeyValues, tuple-aware PIT projection access, and updated API snapshots without removing ordinary PIT behavior.).",
    "DoD check passed: README, PIT maintenance and PIT read guidance, production-adoption documentation, and active release notes no longer describe multi-active PITs as unsupported for the bounded shared-driving-key baseline while preserving explicit exclusions for link-parent PITs, automatic orchestration, and provider-specific optimization. (README.md, the PIT read and maintenance contract docs, docs/production-adoption-checklist.md, and docs/releases/v0.20.0.md now document the bounded shared-driving-key multi-active PIT baseline while preserving the stated exclusions.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: When a PIT references multi-active hub-parent satellites that all resolve to the same canonical driving-key names and order, the generated PIT entity includes those driving-key columns between \u0060ParentHashKey\u0060 and \u0060LoadTimestamp\u0060, and the PIT primary-key and baseline traversal index expand to \u0060(ParentHashKey, \u003CDrivingKey...\u003E, LoadTimestamp)\u0060. (PIT driving-key columns and the widened primary key were added, but the PIT translation still creates no secondary/traversal index: DataVaultEfMetadataTranslationTests.ApplyDataVaultMetadataProjectsMultiActivePitDrivingKeysInCanonicalOrder ends with Assert.Empty(pitEntity.GetIndexes()).).",
    "AC check failed: Unit tests, SQLite integration tests, diagnostics and explain coverage, public API snapshot updates, and documentation updates prove both the preserved ordinary PIT baseline and the new multi-active tuple baseline. (The repo adds multi-active row-generation and fallback read coverage, but the required proof is incomplete: the SQLite PIT integration suite still lacks tuple-aware MaintainParentsAsync(...) coverage and incompatible-shape rejection coverage, and tuple-aware explain/diagnostic coverage is still partial.).",
    "DoD check failed: SQLite integration coverage demonstrates tuple-aware rebuild, tuple-aware targeted parent maintenance, mixed ordinary-plus-multi-active PIT behavior, and deterministic rejection of incompatible multi-active shapes. (SQLite integration coverage demonstrates tuple-aware rebuild/read fallback for one mixed ordinary-plus-multi-active PIT, but it still has no tuple-aware MaintainParentsAsync(...) integration case and no SQLite rejection case for incompatible multi-active shapes.).",
    "DoD check failed: Explain and diagnostic outputs describe tuple-aware PIT row identity, filters, and projected columns consistently with the implemented maintenance and read behavior. (DataVaultDiagnostics.cs now emits tuple-aware row-identity/projected-column metadata, but DataVaultDiagnosticsTests still exercise only the ordinary PIT shape and the new tests do not directly assert tuple-aware projected columns or referenced-satellite driving-key diagnostics.).",
    "Acceptance criterion 1 is not fully met because the multi-active PIT translation still leaves the generated PIT entity without the requested traversal/secondary index; the multi-active translation test explicitly asserts no indexes.",
    "Acceptance criterion 5 and definition-of-done items 3-4 are not met because the added SQLite coverage stops at rebuild/read fallback and does not prove tuple-aware MaintainParentsAsync(...), incompatible-shape rejection in SQLite integration, or tuple-aware explain/diagnostic projections."
  ],
  "evidence": [
    "git diff --name-only develop...fb551d98db5a shows coordinated PIT changes across README.md, docs/plans/pit-backed-as-of-read-api-contract.md, docs/plans/pit-maintenance-service-v1-contract.md, docs/production-adoption-checklist.md, docs/releases/v0.20.0.md, src/DCoding.Data.DVault* PIT translation/maintenance/read files, and PIT-related tests/snapshots.",
    "src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs adds PIT driving-key columns and widens the PIT primary key, but tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs still asserts Assert.Empty(pitEntity.GetIndexes()) for the multi-active PIT entity PitCustomerContactStatus.",
    "tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs adds PitMaintenanceRebuildsAndReadsMultiActiveTupleRowsThroughSqliteFallback, and that test asserts ProviderNeutralFallback diagnostics plus tuple-specific Contact Type projection.",
    "The only SQLite MaintainParentsAsync(...) integration tests in tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs are PitMaintenanceMaintainsOnlyRequestedParentsAndCorrectsLateArrivingSatelliteHistoryThroughSqlite and RegistryBackedPitMaintenanceMaintainsParentsByClrMappingThroughSqlite, both ordinary PIT cases.",
    "A repository search over tests/DCoding.Data.DVault.Tests/Integration found no SQLite PIT integration case asserting incompatible multi-active rejection text such as do not match multi-active satellite.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs still validates the ordinary PIT read shape PitCustomerProfileStatus; there is no tuple-aware assertion for pitDrivingKeyProjection, referenced-satellite DrivingKeyColumnNames, or the tuple-aware PIT explanation text.",
    "No executable build/test/format commands were run in this read-only review session; the decision is based on branch diff and file inspection.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/ef-core, area/maintenance, area/modeling, area/read-models, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q9102970H1VQN16QWRGQX0-story-support-pit-over-multi-active-satellites\u0027.",
    "Ticket history references implementation commit \u0027fb551d98db5a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add the missing PIT traversal index for multi-active PIT entities, or update the contract explicitly if no secondary index is intended.",
    "Add SQLite integration coverage for tuple-aware MaintainParentsAsync(...) on a mixed ordinary-plus-multi-active PIT and for deterministic rejection of incompatible multi-active shapes.",
    "Add tuple-aware diagnostics/explain assertions for RowIdentityColumns, projected driving-key columns, and referenced-satellite DrivingKeyColumnNames, then rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh through the supported verification path."
  ],
  "branchName": "ticket/06F5Q9102970H1VQN16QWRGQX0-story-support-pit-over-multi-active-satellites",
  "commitSha": "fb551d98db5a"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F5Q9102970H1VQN16QWRGQX0`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F5Q9102970H1VQN16QWRGQX0-story-support-pit-over-multi-active-satellites`