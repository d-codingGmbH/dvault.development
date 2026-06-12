[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F9GF60BKEW0CC9FCZRPVX0SR\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto\u0027 and commit \u0027f1ee3c7f7114\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto\u0027 from source \u0027f1ee3c7f7114\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto\u0027.",
    "Evidence: git diff --name-only develop...f1ee3c7f7114 -- tests/DCoding.Data.DVault.Tests returned only tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:18-399 contains the new HexString and Binary SQLite persistence, schema, read-shape, and negative coverage claimed in the developer delivery.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:401-559 defines new HashKeyStorageMetadata, HashKeyStorageProfileContext, and HashKeyStorageProfileModelCacheKeyFactory helper types.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs:8-35 adds HashKeyStorageProfileSqliteTests to required local SQLite discovery.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:486-542 adds the six-profile HexString/Binary store-type matrix.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs:72-82 already asserts that DB2 live-schema remains unsupported.",
    "Evidence: src/DCoding.Data.DVault.Sqlite/SqliteDataVaultSaveStrategy.cs:380-395 and src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs:248-406 now route binary hash-key parameters and result values through src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/ef-core, area/hashing, area/provider-support, area/schema, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto\u0027.",
    "Evidence: Ticket history references implementation commit \u0027f1ee3c7f7114\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A SQLite integration test proves HexString hash keys still persist and read as text, and a paired SQLite integration test proves Binary hash keys persist as blob or bytes while callers still save and read canonical lowercase hex strings. (tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:18-83 persists HexString hub and satellite hash keys as SQLite text and reads them back as canonical strings; tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:86-245 persists Binary hash keys as blob values and reads caller-facing hash keys back as lowercase hex strings.).",
    "AC check passed: Schema coverage proves hash-key and participant-reference columns on generated hub, link, satellite, PIT, and bridge artifacts size from the active stable-hash digest for both HexString and Binary without changing logical names or API shape. (tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:247-331 asserts hub, link, satellite, PIT, and bridge hash-key or participant-reference properties for both profiles, including store type, digest annotations, and string CLR/API shape; lines 57-71 and 172-179 also verify persisted value storage class and length.).",
    "AC check passed: Read-path coverage proves the Binary profile round-trips hash-key participation through latest/current, explicit as-of, PIT as-of, and bridge traversal request shapes. (tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:182-243 exercises Binary latest/current, explicit as-of, PIT as-of, and bridge traversal reads with round-tripped hash keys.).",
    "AC check passed: Provider-profile matrix coverage proves the finite built-in baseline projects the expected HexString and Binary store types; DB2 live-schema execution still reports unsupported-provider rather than silently passing. (tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:486-542 covers the six built-in provider profiles for HexString and Binary store types, and tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs:72-82 keeps DB2 live-schema on the explicit unsupported-provider path.).",
    "AC check passed: Negative coverage fails closed for malformed or incompatible hash-key storage facts at the boundaries exercised by this ticket. (tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:333-399 adds save/read boundary negatives for wrong digest length and malformed blob-backed bridge data, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs:537-558 already covers invalid model/provider conversion facts.).",
    "DoD check passed: Existing HexString baselines continue to pass unchanged, and the new Binary assertions make the storage-profile difference explicit in store type and round-trip behavior. (tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:18-83 keeps the HexString assertions explicit, and lines 86-331 make the Binary TEXT versus BLOB difference explicit while keeping string model boundaries.).",
    "DoD check passed: At least one executable Binary round-trip test covers save plus read behavior, and at least one provider-matrix or fixture test covers non-SQLite provider store-type projections without requiring new infrastructure. (tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:86-245 provides a Binary save-plus-read round-trip test, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:486-542 provides non-SQLite provider matrix coverage without new infrastructure.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: New coverage lands in the existing unit and integration test projects under tests/DCoding.Data.DVault.Tests and reuses existing metadata, schema, PIT, and bridge fixtures instead of creating a parallel test harness. (git diff --name-only develop...f1ee3c7f7114 -- tests/DCoding.Data.DVault.Tests shows test changes only in Integration/HashKeyStorageProfileSqliteTests.cs, Integration/ProviderIntegrationCategoryDiscoveryTests.cs, and Unit/DataVaultProviderCapabilityProfileTests.cs; the branch does not extend the existing schema, PIT, bridge, or shared live-schema fixture surfaces, and tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:401-559 introduces a standalone metadata/context harness instead.).",
    "Definition of Done 1 is not met: the implementation adds a separate HashKeyStorageProfileSqliteTests harness instead of extending the existing schema, PIT, bridge, and shared live-schema fixture surfaces required by the ticket contract (git diff --name-only develop...f1ee3c7f7114 -- tests/DCoding.Data.DVault.Tests; tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:401-559)."
  ],
  "evidence": [
    "git diff --name-only develop...f1ee3c7f7114 -- tests/DCoding.Data.DVault.Tests returned only tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs, tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs.",
    "tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:18-399 contains the new HexString and Binary SQLite persistence, schema, read-shape, and negative coverage claimed in the developer delivery.",
    "tests/DCoding.Data.DVault.Tests/Integration/HashKeyStorageProfileSqliteTests.cs:401-559 defines new HashKeyStorageMetadata, HashKeyStorageProfileContext, and HashKeyStorageProfileModelCacheKeyFactory helper types.",
    "tests/DCoding.Data.DVault.Tests/Integration/ProviderIntegrationCategoryDiscoveryTests.cs:8-35 adds HashKeyStorageProfileSqliteTests to required local SQLite discovery.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderCapabilityProfileTests.cs:486-542 adds the six-profile HexString/Binary store-type matrix.",
    "tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs:72-82 already asserts that DB2 live-schema remains unsupported.",
    "src/DCoding.Data.DVault.Sqlite/SqliteDataVaultSaveStrategy.cs:380-395 and src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs:248-406 now route binary hash-key parameters and result values through src/DCoding.Data.DVault/DataVaultHashKeyProviderValueConverter.cs.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/ef-core, area/hashing, area/provider-support, area/schema, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto\u0027.",
    "Ticket history references implementation commit \u0027f1ee3c7f7114\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Refactor the new hash-key storage coverage into the existing schema, PIT, bridge, and shared live-schema fixture surfaces the contract called out, rather than keeping a standalone HashKeyStorageProfileSqliteTests harness.",
    "After that rework, rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported verification environment."
  ],
  "branchName": "ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto",
  "commitSha": "f1ee3c7f7114"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F9GF60BKEW0CC9FCZRPVX0SR`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F9GF60BKEW0CC9FCZRPVX0SR-task-add-schema-save-and-read-tests-for-hash-sto`