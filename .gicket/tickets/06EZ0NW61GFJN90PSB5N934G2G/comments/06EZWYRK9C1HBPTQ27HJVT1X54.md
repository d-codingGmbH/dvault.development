[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706EZ0NW61GFJN90PSB5N934G2G\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ\u0027 and commit \u00271431d8f0238b\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ\u0027 from source \u00271431d8f0238b\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ\u0027.",
    "Evidence: git -C /mnt/c/Projects/DVault show --stat --oneline 1431d8f0238b reports the handoff commit touching src/DCoding.Data.DVault/DataVaultSaveService.cs, tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.",
    "Evidence: git -C /mnt/c/Projects/DVault diff --stat develop...1431d8f0238b -- src/DCoding.Data.DVault src/DCoding.Data.DVault.Sqlite src/DCoding.Data.DVault.Postgres src/DCoding.Data.DVault.SqlServer src/DCoding.Data.DVault.MySql src/DCoding.Data.DVault.Oracle tests/DCoding.Data.DVault.Tests shows 19 relevant src/test files changed for this ticket.",
    "Evidence: src/DCoding.Data.DVault/Modeling/DataVaultModel.cs adds DataVaultSatelliteBuilder.DrivingKey(string), driving-key validation, and multi-active PK/index column ordering.",
    "Evidence: src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs adds the multi-active DataVaultSatelliteMetadata constructor plus DrivingKeyNames and keeps ordinary satellites on empty driving-key collections.",
    "Evidence: src/DCoding.Data.DVault/DataVaultSaveService.cs adds DataVaultSatelliteSaveOperation drivingKeyValues, partitions latest satellite state by SatelliteSeriesKey(parentHashKey, drivingKey tuple), and returns DataVaultSavedRecord with driving-key identity.",
    "Evidence: src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs and the Postgres, SqlServer, MySql, and Oracle save strategies all add !ContainsMultiActiveSatelliteOperations(requests) to CanSave.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs asserts SatCustomerContactChannel columns CustomerHashKey, ContactType, RegionCode, HashDiff, LoadTimestamp, RecordSource, EmailAddress and PK/index order CustomerHashKey, ContactType, RegionCode, LoadTimestamp.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs adds DefaultSaveServicePersistsMultiActiveSatelliteRowsByCanonicalDrivingKeysThroughSqlite and asserts RowsWritten values 2, 0, and 1 plus ordered saved-record driving keys and persisted row contents.",
    "Evidence: The required verification commands dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run from this read-only review surface.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/modeling, area/multi-active-satellite, area/persistence, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 8 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EZ0NWCA6NEZH8VBJNGW4FVHG-task-add-multi-active-satellite-docs-and-tests\u0027.",
    "Evidence: Ticket history references implementation commit \u00271431d8f0238b\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: Satellites become multi-active only when one or more driving keys are declared through the sibling-approved opt-in contract, while ordinary satellites keep the current builder, metadata, and save behavior unchanged and expose empty driving-key collections. (src/DCoding.Data.DVault/Modeling/DataVaultModel.cs adds DataVaultSatelliteBuilder.DrivingKey(string), src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs adds the driving-key constructor/property, and the ordinary constructor still produces empty DrivingKeyNames; tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs asserts ordinary satellites expose an empty driving-key collection.).",
    "AC check passed: Validation rejects empty or duplicate driving-key names, overlaps with payload names, missing or extra driving-key values, duplicate supplied names, and null driving-key values, while matching supplied names to canonical declaration order regardless of caller enumeration order. (src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs rejects empty, duplicate, and payload-overlapping driving-key names; src/DCoding.Data.DVault/DataVaultSaveService.cs rejects missing, extra, duplicate, and null driving-key values; tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs covers caller-order-independent name matching.).",
    "AC check passed: For opt-in multi-active satellites, translated schema stores driving-key columns immediately after the parent hash-key column and expands the satellite primary key and relevant index layout to (parentHashKey, drivingKeyValue1, ..., drivingKeyValueN, loadTimestamp) in canonical declaration order. (src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs and src/DCoding.Data.DVault/Modeling/DataVaultModel.cs place driving-key columns immediately after the parent hash key and build PK/index order as parent hash key plus driving keys plus load timestamp; tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs and Unit/DataVaultEfMetadataTranslationTests.cs assert that order.).",
    "AC check passed: A replay with the same parent hash key, the same canonical driving-key tuple, and the same latest hash diff writes no new row. (src/DCoding.Data.DVault/DataVaultSaveService.cs now partitions latest-state tracking by SatelliteSeriesKey(parentHashKey, drivingKey tuple), and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs expects replayResult.RowsWritten == 0 for the same canonical tuple and hash diff.).",
    "AC check passed: For the same parent hash key and canonical driving-key tuple, a later changed hash diff inserts a new history row and preserves the earlier row unchanged. (The same SQLite integration test expects changedResult.RowsWritten == 1 for a later billing/DE hash diff and verifies that both history rows remain persisted for that canonical tuple.).",
    "AC check passed: Rows with the same parent hash key and same load timestamp but different canonical driving-key tuples can both persist without colliding, and SQLite tests plus relevant public API or snapshot coverage prove deterministic RowsWritten, saved-record ordering, and persisted row contents. (tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs asserts same-parent same-load-timestamp coexistence for billing and shipping tuples, deterministic saved-record order and driving-key identity, and persisted row contents; tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt captures the new public surfaces.).",
    "DoD check passed: The provider-neutral save service and translated satellite schema honor the sibling-approved multi-active uniqueness and ordering rules without regressing hub, link, or ordinary satellite persistence. (The inspected diff shows the provider-neutral save service and translated schema both using canonical driving-key ordering and series partitioning, while hub, link, and ordinary-satellite paths remain structurally intact.).",
    "DoD check passed: The contract-defined public opt-in and save surfaces are implemented exactly as specified by the shared artifact and are reflected in approved snapshot tests together with the required validation behavior. (The contract surfaces are implemented in src/DCoding.Data.DVault/Modeling/DataVaultModel.cs, src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs, and src/DCoding.Data.DVault/DataVaultSaveService.cs, with matching public API snapshot and validation test updates in the same slice.).",
    "DoD check passed: Any provider strategy that cannot yet honor the multi-active rules declines those batches so dispatch falls back to the provider-neutral writer. (src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs plus the Postgres, SqlServer, MySql, and Oracle save strategies all add a ContainsMultiActiveSatelliteOperations guard in CanSave so multi-active batches decline optimized handling and fall back to the provider-neutral writer.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Required local SQLite baseline tests pass for validation failures, canonical ordering, unchanged replay suppression, changed-row insertion, same-parent same-load-timestamp coexistence across different driving-key tuples, and deterministic RowsWritten and saved-record ordering. (The required verification commands dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not executed from this read-only review surface, so passing status is not directly confirmed.).",
    "Definition of Done index 4 remains unconfirmed because the required local verification commands could not be executed from this read-only review surface."
  ],
  "evidence": [
    "git -C /mnt/c/Projects/DVault show --stat --oneline 1431d8f0238b reports the handoff commit touching src/DCoding.Data.DVault/DataVaultSaveService.cs, tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt.",
    "git -C /mnt/c/Projects/DVault diff --stat develop...1431d8f0238b -- src/DCoding.Data.DVault src/DCoding.Data.DVault.Sqlite src/DCoding.Data.DVault.Postgres src/DCoding.Data.DVault.SqlServer src/DCoding.Data.DVault.MySql src/DCoding.Data.DVault.Oracle tests/DCoding.Data.DVault.Tests shows 19 relevant src/test files changed for this ticket.",
    "src/DCoding.Data.DVault/Modeling/DataVaultModel.cs adds DataVaultSatelliteBuilder.DrivingKey(string), driving-key validation, and multi-active PK/index column ordering.",
    "src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs adds the multi-active DataVaultSatelliteMetadata constructor plus DrivingKeyNames and keeps ordinary satellites on empty driving-key collections.",
    "src/DCoding.Data.DVault/DataVaultSaveService.cs adds DataVaultSatelliteSaveOperation drivingKeyValues, partitions latest satellite state by SatelliteSeriesKey(parentHashKey, drivingKey tuple), and returns DataVaultSavedRecord with driving-key identity.",
    "src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs and the Postgres, SqlServer, MySql, and Oracle save strategies all add !ContainsMultiActiveSatelliteOperations(requests) to CanSave.",
    "tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs asserts SatCustomerContactChannel columns CustomerHashKey, ContactType, RegionCode, HashDiff, LoadTimestamp, RecordSource, EmailAddress and PK/index order CustomerHashKey, ContactType, RegionCode, LoadTimestamp.",
    "tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs adds DefaultSaveServicePersistsMultiActiveSatelliteRowsByCanonicalDrivingKeysThroughSqlite and asserts RowsWritten values 2, 0, and 1 plus ordered saved-record driving keys and persisted row contents.",
    "The required verification commands dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run from this read-only review surface.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/modeling, area/multi-active-satellite, area/persistence, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 8 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EZ0NWCA6NEZH8VBJNGW4FVHG-task-add-multi-active-satellite-docs-and-tests\u0027.",
    "Ticket history references implementation commit \u00271431d8f0238b\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Request deterministic legacy verification for dotnet test DVault.slnx --nologo.",
    "Request deterministic legacy verification for bash tools/check-format.sh.",
    "If both commands pass, rerun the tester gate; this inspection did not find an additional code-level blocker."
  ],
  "branchName": "ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ",
  "commitSha": "1431d8f0238b"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06EZ0NW61GFJN90PSB5N934G2G`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ`