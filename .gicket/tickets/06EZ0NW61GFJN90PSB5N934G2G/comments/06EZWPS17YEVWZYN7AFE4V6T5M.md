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
    "Selected verification source branch \u0027ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ\u0027 and commit \u00276f7cbfe203bf\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ\u0027 from source \u00276f7cbfe203bf\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ\u0027.",
    "Evidence: Reviewed the claimed implementation commit directly with git show --stat --oneline 6f7cbfe203bf; the commit changes 19 implementation and test files across src/DCoding.Data.DVault* and tests/DCoding.Data.DVault.Tests/*.",
    "Evidence: src/DCoding.Data.DVault/Modeling/DataVaultModel.cs adds DataVaultSatelliteBuilder.DrivingKey(...) and builds satellite columns and keys as parent hash key, driving keys, then load timestamp for multi-active declarations.",
    "Evidence: src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs adds the multi-active DataVaultSatelliteMetadata constructor and DrivingKeyNames property, with duplicate and payload-overlap validation.",
    "Evidence: src/DCoding.Data.DVault/DataVaultSaveService.cs adds DrivingKeyValues validation and partitions satellite latest-state tracking by SatelliteSeriesKey(parentHashKey, drivingKeyValues) instead of parent hash key alone.",
    "Evidence: src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs, src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs, and src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs all reject multi-active batches in optimized CanSave gates.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs asserts the SatCustomerContactChannel schema with columns CustomerHashKey, ContactType, RegionCode, HashDiff, LoadTimestamp, RecordSource, EmailAddress and key/index order CustomerHashKey, ContactType, RegionCode, LoadTimestamp.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs covers canonical driving-key reordering, unchanged replay suppression, changed-row insertion, same-parent same-load-timestamp coexistence, and SQLite optimized-strategy fallback for multi-active requests.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt includes the new DataVaultSatelliteBuilder.DrivingKey(...), DataVaultSatelliteMetadata(..., drivingKeyNames), and DataVaultSatelliteSaveOperation(..., drivingKeyValues, ..., hashDiff) surfaces.",
    "Evidence: No build or test commands were executed in this session because the execution boundary is read-only and the declared verification commands are dotnet test DVault.slnx --nologo and bash tools/check-format.sh.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/modeling, area/multi-active-satellite, area/persistence, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 6 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06EZ0NWCA6NEZH8VBJNGW4FVHG-task-add-multi-active-satellite-docs-and-tests\u0027.",
    "Evidence: Ticket history references implementation commit \u00276f7cbfe203bf\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Satellites become multi-active only when one or more driving keys are declared through the sibling-approved opt-in contract, while ordinary satellites keep the current builder, metadata, and save behavior unchanged and expose empty driving-key collections. (Opt-in surfaces are present and ordinary surfaces keep empty driving-key collections: DataVaultSatelliteBuilder.DrivingKey(...) was added in src/DCoding.Data.DVault/Modeling/DataVaultModel.cs, DrivingKeyNames was added in src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs, and the legacy satellite constructor still initializes DrivingKeyNames to an empty collection.).",
    "AC check passed: Validation rejects empty or duplicate driving-key names, overlaps with payload names, missing or extra driving-key values, duplicate supplied names, and null driving-key values, while matching supplied names to canonical declaration order regardless of caller enumeration order. (Driving-key declaration and value validation is implemented in src/DCoding.Data.DVault/Modeling/DataVaultModel.cs, src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs, and src/DCoding.Data.DVault/DataVaultSaveService.cs; unit coverage for invalid declarations and invalid value sets exists in tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs and tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs.).",
    "AC check passed: For opt-in multi-active satellites, translated schema stores driving-key columns immediately after the parent hash-key column and expands the satellite primary key and relevant index layout to (parentHashKey, drivingKeyValue1, ..., drivingKeyValueN, loadTimestamp) in canonical declaration order. (Schema translation and modeling insert driving-key columns immediately after the parent hash key and widen the satellite primary key and parent index to parent plus canonical driving-key tuple plus load timestamp; tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs asserts the resulting SatCustomerContactChannel layout.).",
    "AC check passed: A replay with the same parent hash key, the same canonical driving-key tuple, and the same latest hash diff writes no new row. (Provider-neutral replay suppression now keys latest state by SatelliteSeriesKey(parentHashKey, drivingKeyValues) in src/DCoding.Data.DVault/DataVaultSaveService.cs, and the SQLite multi-active integration test covers unchanged replay for the same canonical tuple.).",
    "AC check passed: For the same parent hash key and canonical driving-key tuple, a later changed hash diff inserts a new history row and preserves the earlier row unchanged. (The same parent plus canonical driving-key series with a later changed hash diff produces a new history row while preserving the earlier row; the multi-active SQLite test verifies both persisted billing rows remain present.).",
    "DoD check passed: The provider-neutral save service and translated satellite schema honor the sibling-approved multi-active uniqueness and ordering rules without regressing hub, link, or ordinary satellite persistence. (The provider-neutral save service and translated schema implement canonical multi-active uniqueness and ordering in code, and the ordinary hub, link, and satellite save paths remain wired through the existing integration suite. Runtime regression confirmation still depends on Definition of Done 4 verification.).",
    "DoD check passed: The contract-defined public opt-in and save surfaces are implemented exactly as specified by the shared artifact and are reflected in approved snapshot tests together with the required validation behavior. (The contract-defined public opt-in and save surfaces are implemented in source and reflected in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt, with unit coverage for the required validation behavior.).",
    "DoD check passed: Any provider strategy that cannot yet honor the multi-active rules declines those batches so dispatch falls back to the provider-neutral writer. (SQLite, Postgres, SQL Server, MySQL, and Oracle optimized strategies all now decline multi-active batches via !ContainsMultiActiveSatelliteOperations(requests), allowing dispatch to fall back to the provider-neutral writer.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Rows with the same parent hash key and same load timestamp but different canonical driving-key tuples can both persist without colliding, and SQLite tests plus relevant public API or snapshot coverage prove deterministic RowsWritten, saved-record ordering, and persisted row contents. (The coexistence and persisted-row aspects are covered, but the repository does not actually prove deterministic saved-record ordering for the same-parent same-load-timestamp multi-active case. tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs only asserts MetadataName for firstResult.SavedRecords, while satellite DataVaultSavedRecord entries do not carry driving-key identity and use only the parent hash key.).",
    "DoD check failed: Required local SQLite baseline tests pass for validation failures, canonical ordering, unchanged replay suppression, changed-row insertion, same-parent same-load-timestamp coexistence across different driving-key tuples, and deterministic RowsWritten and saved-record ordering. (The required local SQLite baseline commands were not executed in this read-only session, and the new multi-active SQLite test still does not directly prove deterministic saved-record ordering.).",
    "Blocking: tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs does not actually prove deterministic saved-record ordering for the multi-active same-parent same-load-timestamp scenario. It only checks MetadataName, while src/DCoding.Data.DVault/DataVaultSaveService.cs creates satellite DataVaultSavedRecord values from kind, metadata name, table name, and parent hash key only, making the two first-request saved records indistinguishable.",
    "Verification gap: the required commands dotnet test DVault.slnx --nologo and bash tools/check-format.sh were not run in this read-only tester session, so the local SQLite baseline and formatting gate remain open."
  ],
  "evidence": [
    "Reviewed the claimed implementation commit directly with git show --stat --oneline 6f7cbfe203bf; the commit changes 19 implementation and test files across src/DCoding.Data.DVault* and tests/DCoding.Data.DVault.Tests/*.",
    "src/DCoding.Data.DVault/Modeling/DataVaultModel.cs adds DataVaultSatelliteBuilder.DrivingKey(...) and builds satellite columns and keys as parent hash key, driving keys, then load timestamp for multi-active declarations.",
    "src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs adds the multi-active DataVaultSatelliteMetadata constructor and DrivingKeyNames property, with duplicate and payload-overlap validation.",
    "src/DCoding.Data.DVault/DataVaultSaveService.cs adds DrivingKeyValues validation and partitions satellite latest-state tracking by SatelliteSeriesKey(parentHashKey, drivingKeyValues) instead of parent hash key alone.",
    "src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs, src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs, src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs, and src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs all reject multi-active batches in optimized CanSave gates.",
    "tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs asserts the SatCustomerContactChannel schema with columns CustomerHashKey, ContactType, RegionCode, HashDiff, LoadTimestamp, RecordSource, EmailAddress and key/index order CustomerHashKey, ContactType, RegionCode, LoadTimestamp.",
    "tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs covers canonical driving-key reordering, unchanged replay suppression, changed-row insertion, same-parent same-load-timestamp coexistence, and SQLite optimized-strategy fallback for multi-active requests.",
    "tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt includes the new DataVaultSatelliteBuilder.DrivingKey(...), DataVaultSatelliteMetadata(..., drivingKeyNames), and DataVaultSatelliteSaveOperation(..., drivingKeyValues, ..., hashDiff) surfaces.",
    "No build or test commands were executed in this session because the execution boundary is read-only and the declared verification commands are dotnet test DVault.slnx --nologo and bash tools/check-format.sh.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/modeling, area/multi-active-satellite, area/persistence, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 6 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06EZ0NWCA6NEZH8VBJNGW4FVHG-task-add-multi-active-satellite-docs-and-tests\u0027.",
    "Ticket history references implementation commit \u00276f7cbfe203bf\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Amend the multi-active SQLite coverage so deterministic SavedRecords ordering is directly asserted with distinguishable evidence; the current firstResult.SavedRecords assertion is insufficient.",
    "After fixing that proof gap, request deterministic legacy verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh in a writable supported environment.",
    "Return the ticket to test only after the updated proof and legacy verification both succeed."
  ],
  "branchName": "ticket/06EZ0NW61GFJN90PSB5N934G2G-task-persist-multi-active-satellites-with-determ",
  "commitSha": "6f7cbfe203bf"
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