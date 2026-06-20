[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FEA6G6R742K2RJ9Q7CY173JR\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FEA6G6R742K2RJ9Q7CY173JR-task-add-db2-live-schema-reader-for-opt-in-prefl\u0027 and commit \u00278730f25e60b8\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FEA6G6R742K2RJ9Q7CY173JR-task-add-db2-live-schema-reader-for-opt-in-prefl\u0027 from source \u00278730f25e60b8\u0027.",
    "Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FEA6G6R742K2RJ9Q7CY173JR-task-add-db2-live-schema-reader-for-opt-in-prefl\u0027.",
    "Evidence: git diff --name-only develop...8730f25e60b8 shows runtime changes in src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs and new src/DCoding.Data.DVault/Db2DataVaultLiveSchemaReader.cs, plus DB2 live-schema test additions and multiple doc updates.",
    "Evidence: src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:18,25,33 now defines IBM.EntityFrameworkCore as the DB2 provider and dispatches it to Db2DataVaultLiveSchemaReader.",
    "Evidence: src/DCoding.Data.DVault/Db2DataVaultLiveSchemaReader.cs:11-159 reads SYSCAT.TABLES, SYSCAT.COLUMNS, SYSCAT.TABCONST/KEYCOLUSE, SYSCAT.INDEXES, and SYSCAT.INDEXCOLUSE and returns a fixed redacted unavailable message.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/Db2LiveSchemaReaderTests.cs:10, tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixture.cs:138, and tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaModelOptions.cs:73 add DB2 opt-in live-schema fixture coverage aligned to DataVaultProviderCapabilityProfiles.Db2.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs:72 updates the unit contract from explicit DB2 unsupported behavior to built-in dispatch.",
    "Evidence: examples/README.md:204 still says DB2 returns UnsupportedProvider until a reader exists, and docs/plans/shared-implementation-standards.md:92 still says the current baseline does not add DB2 live-schema reading; git diff --name-only develop...8730f25e60b8 -- examples/README.md docs/plans/shared-implementation-standards.md returned no changed paths.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/design-time, area/provider-support, area/schema, automation/bot-ready, needs-test, provider/db2, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FEA6G6R742K2RJ9Q7CY173JR-task-add-db2-live-schema-reader-for-opt-in-prefl\u0027.",
    "Evidence: Ticket history references implementation commit \u00278730f25e60b8\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A DB2 \u0060DbContext\u0060 using provider name \u0060IBM.EntityFrameworkCore\u0060 no longer returns the explicit unsupported-provider boundary from \u0060DataVaultLiveSchemaReader.ReadAsync\u0060; when configured, it returns a structured live-schema snapshot. (src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs now maps IBM.EntityFrameworkCore to Db2DataVaultLiveSchemaReader, and tests/DCoding.Data.DVault.Tests/Integration/Db2LiveSchemaReaderTests.cs adds opt-in snapshot coverage.).",
    "AC check passed: The DB2 snapshot is deterministic and limited to DVault table, column, primary-key, and secondary-index facts needed by idempotency preflight, aligned with the repository\u0027s existing DB2 physical-name and index-shape rules. (src/DCoding.Data.DVault/Db2DataVaultLiveSchemaReader.cs reads only bounded DB2 catalog facts (tables, columns, primary keys, and secondary indexes), and DB2 fixture/model-option coverage aligns names with DataVaultProviderCapabilityProfiles.Db2.).",
    "AC check passed: Unavailable DB2 cases such as missing configuration, unreachable catalog access, or insufficient privileges return explicit classified outcomes and do not leak connection strings, credentials, host names, raw SQL, raw data, or provider exception text. (Db2DataVaultLiveSchemaReader.CreateUnavailableResult returns a fixed redacted message, CatalogDataVaultLiveSchemaReader classifies DbException and InvalidOperationException as Unavailable, and existing DB2 configuration tests keep the missing-configuration path explicit.).",
    "AC check passed: Existing SQLite, PostgreSQL, SQL Server, Oracle, and MySQL live-schema behavior and unsupported-provider handling for truly unsupported providers remain unchanged. (Existing SQLite, PostgreSQL, SQL Server, Oracle, and MySQL dispatch entries remain in DataVaultLiveSchemaReader, and the non-DB2 reader diffs only adapt shared expected-table plumbing rather than changing provider-specific catalog behavior.).",
    "DoD check passed: Unit coverage is updated so the old DB2-specific explicit-unsupported boundary assertion is removed or replaced with the new DB2 dispatch contract. (tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs replaces the old DB2 explicit-unsupported assertion with a built-in dispatch assertion.).",
    "DoD check passed: Opt-in DB2 integration coverage exercises the live-schema reader against the canonical shared live-schema fixture under \u0060DVAULT_TEST_DB2_CONNECTION_STRING\u0060 gating. (tests/DCoding.Data.DVault.Tests/Integration/Db2LiveSchemaReaderTests.cs, tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixture.cs, and the existing DVAULT_TEST_DB2_CONNECTION_STRING configuration contract wire DB2 live-schema coverage into the opt-in external-provider lane.).",
    "DoD check passed: No new public API surface or workflow dependency is introduced beyond the bounded live-schema reader implementation and supporting tests/docs. (The branch diff adds an internal reader, tests, and docs only; no public API snapshot files, package manifests, or new workflow dependency files were changed.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Current active guidance no longer states that DB2 live-schema reading is unsupported; it states that DB2 live-schema checks are external, opt-in, and consumer-owned like the other non-SQLite live-schema lanes. (Current guidance is still inconsistent: examples/README.md:204 still says DB2 returns UnsupportedProvider until a reader exists, and docs/plans/shared-implementation-standards.md:92 still says the current v0.42.0 baseline does not add DB2 live-schema reading.).",
    "DoD check failed: Documentation is consistent across the current README, adoption, model-first, and current-baseline surfaces that presently advertise DB2 live-schema as unsupported. (Documentation is not yet consistent across current guidance surfaces because examples/README.md and docs/plans/shared-implementation-standards.md still describe DB2 live-schema as unavailable or unsupported.).",
    "Blocking: examples/README.md:204 still tells adopters that DB2 live-schema returns UnsupportedProvider until a reader exists, which conflicts with the new runtime behavior and the updated README/adoption/model-first guidance.",
    "Blocking: docs/plans/shared-implementation-standards.md:92 still states that the current v0.42.0 baseline does not add DB2 live-schema reading, leaving active planning guidance inconsistent with the implemented feature."
  ],
  "evidence": [
    "git diff --name-only develop...8730f25e60b8 shows runtime changes in src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs and new src/DCoding.Data.DVault/Db2DataVaultLiveSchemaReader.cs, plus DB2 live-schema test additions and multiple doc updates.",
    "src/DCoding.Data.DVault/DataVaultLiveSchemaReader.cs:18,25,33 now defines IBM.EntityFrameworkCore as the DB2 provider and dispatches it to Db2DataVaultLiveSchemaReader.",
    "src/DCoding.Data.DVault/Db2DataVaultLiveSchemaReader.cs:11-159 reads SYSCAT.TABLES, SYSCAT.COLUMNS, SYSCAT.TABCONST/KEYCOLUSE, SYSCAT.INDEXES, and SYSCAT.INDEXCOLUSE and returns a fixed redacted unavailable message.",
    "tests/DCoding.Data.DVault.Tests/Integration/Db2LiveSchemaReaderTests.cs:10, tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixture.cs:138, and tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaModelOptions.cs:73 add DB2 opt-in live-schema fixture coverage aligned to DataVaultProviderCapabilityProfiles.Db2.",
    "tests/DCoding.Data.DVault.Tests/Unit/LiveSchemaReaderContractOutcomeTests.cs:72 updates the unit contract from explicit DB2 unsupported behavior to built-in dispatch.",
    "examples/README.md:204 still says DB2 returns UnsupportedProvider until a reader exists, and docs/plans/shared-implementation-standards.md:92 still says the current baseline does not add DB2 live-schema reading; git diff --name-only develop...8730f25e60b8 -- examples/README.md docs/plans/shared-implementation-standards.md returned no changed paths.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/design-time, area/provider-support, area/schema, automation/bot-ready, needs-test, provider/db2, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FEA6G6R742K2RJ9Q7CY173JR-task-add-db2-live-schema-reader-for-opt-in-prefl\u0027.",
    "Ticket history references implementation commit \u00278730f25e60b8\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update examples/README.md to describe DB2 live-schema the same way as the current README and adoption/model-first docs: built in, external opt-in, and consumer-owned.",
    "Update docs/plans/shared-implementation-standards.md so the current package compatibility contract no longer says DB2 live-schema reading is absent from the v0.42.0 baseline.",
    "After the doc fixes land, request legacy verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported environment."
  ],
  "branchName": "ticket/06FEA6G6R742K2RJ9Q7CY173JR-task-add-db2-live-schema-reader-for-opt-in-prefl",
  "commitSha": "8730f25e60b8"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FEA6G6R742K2RJ9Q7CY173JR`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FEA6G6R742K2RJ9Q7CY173JR-task-add-db2-live-schema-reader-for-opt-in-prefl`