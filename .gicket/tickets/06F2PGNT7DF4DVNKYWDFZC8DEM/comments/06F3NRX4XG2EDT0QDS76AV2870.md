[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F2PGNT7DF4DVNKYWDFZC8DEM\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F2PGNT7DF4DVNKYWDFZC8DEM-task-add-provider-bulk-integration-coverage\u0027 and commit \u00273ad7f1cfcfca\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F2PGNT7DF4DVNKYWDFZC8DEM-task-add-provider-bulk-integration-coverage\u0027 from source \u00273ad7f1cfcfca\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F2PGNT7DF4DVNKYWDFZC8DEM-task-add-provider-bulk-integration-coverage\u0027.",
    "Evidence: \u0060git rev-parse HEAD\u0060 returned \u00608bdef825afa7186a67afc94204e1740a90b1add6\u0060, while the claimed implementation commit resolved to \u00603ad7f1cfcfca4766fdb1a08457a62782094f7228\u0060; \u0060git diff 3ad7f1cfcfca..HEAD -- README.md src tests docs\u0060 returned no code/doc paths, so the code review matched the claimed implementation and later branch movement was \u0060.gicket\u0060 metadata only.",
    "Evidence: \u0060git diff --name-only develop...3ad7f1cfcfca\u0060 showed code changes in \u0060README.md\u0060, \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060, \u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs\u0060, and the four provider integration test files; \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0060 was not changed.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs\u0060 creates a five-request \u0060DataVaultBulkSaveRequest\u0060 with 20 customer hubs, 20 order hubs, 20 links, and 4 satellite operations, then asserts \u0060ProviderStrategySelected\u0060, \u0060RowsWritten == 63\u0060, \u0060SavedRecords.Count == 64\u0060, saved-record ordering, persisted rows, and suppression of the unchanged replay satellite row.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs\u0060, \u0060SqlServerDataVaultSmokeTests.cs\u0060, \u0060OracleDataVaultSmokeTests.cs\u0060, and \u0060MySqlExplicitDataVaultSaveServiceTests.cs\u0060 each add a \u0060[Fact]\u0060 calling \u0060ExternalProviderBulkSaveAssertions.AssertProviderBulkSaveAsync(...)\u0060 with the existing fixture factory and \u0060AddDVault*()\u0060 registration for that provider.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixtures.cs\u0060 already owns \u0060CreatePostgresAsync\u0060, \u0060CreateSqlServerAsync\u0060, \u0060CreateOracleAsync\u0060, and \u0060CreateMySqlAsync\u0060, including skip-on-missing-env behavior and provider-specific schema/table cleanup; the new coverage reuses that harness rather than introducing a new one.",
    "Evidence: \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060 and \u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060 now resolve physical table names from EF model metadata before executing inserts and latest-HashDiff lookups, matching the fixture\u0027s table-name overrides for MySQL prefixes and Oracle shortened table names.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0060 still evaluates Oracle bulk eligibility by provider name, clean context, multi-active rejection, and a minimum 50 operations; it does not reject ordinary satellite batches, which matches the new Oracle bulk test shape.",
    "Evidence: \u0060README.md\u0060 provider-package overview still says Oracle declines request batches that contain satellite operations, while the optional Oracle integration section now says the live lane includes an ordered bulk hub, link, and satellite batch through the provider strategy. \u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060 still describes Oracle as hub/link-only and SQL Server external opt-in coverage as a smoke lane.",
    "Evidence: \u0060git diff --check develop...3ad7f1cfcfca -- README.md src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs\u0060 produced no whitespace or patch-format errors.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027develop\u0027.",
    "Evidence: Ticket history references implementation commit \u00273ad7f1cfcfca\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: External opt-in integration tests exercise the ordered bulk-save path itself for each scoped provider lane instead of proving bulk behavior only through repeated single-request saves. (\u0060ExternalProviderBulkSaveAssertions\u0060 builds one ordered \u0060DataVaultBulkSaveRequest\u0060 and the four provider-specific tests call it directly, so each scoped lane exercises the bulk entry path instead of looping single-save requests.).",
    "AC check passed: PostgreSQL coverage proves AddDVaultPostgres on a clean Npgsql-backed context persists an ordered bulk hub, link, and satellite batch through the provider strategy and leaves no fallback-tracked DVault rows. (The Postgres bulk test uses \u0060ExternalProviderLiveSchemaFixture.CreatePostgresAsync\u0060 plus \u0060services.AddDVaultPostgres()\u0060, and the shared assertion checks \u0060ProviderStrategySelected\u0060, persisted hub/link/satellite rows, saved-record ordering, and an empty change tracker after save.).",
    "AC check passed: SQL Server coverage proves AddDVaultSqlServer on a clean SQL Server context persists an eligible ordered bulk batch that meets the current native gate of at least 50 total operations and no more than 500 satellite operations through the provider strategy rather than the fallback writer. (The SQL Server bulk test reuses the shared bulk assertion with \u0060AddDVaultSqlServer()\u0060. The shared scenario contains 64 total operations and 4 satellite operations, satisfying the current SQL Server gate while asserting provider-strategy selection and persisted batch results.).",
    "AC check passed: MySQL coverage proves AddDVaultMySql on a clean supported MySQL provider context persists an eligible ordered bulk batch of at least 50 total operations through the provider strategy rather than the fallback writer. (The MySQL bulk test reuses the shared bulk assertion with \u0060AddDVaultMySql()\u0060. The shared scenario contains 64 total operations, asserts provider-strategy selection, and the MySQL strategy was patched to resolve fixture-specific physical table names during inserts and latest-HashDiff lookups.).",
    "AC check passed: Oracle coverage proves AddDVaultOracle on a clean Oracle context persists an eligible ordered bulk batch of at least 50 total operations through the currently implemented provider strategy rather than only through the existing single-hub smoke lane. (The Oracle bulk test reuses the shared bulk assertion with \u0060AddDVaultOracle()\u0060. The shared scenario exceeds the 50-operation Oracle gate and asserts provider-strategy selection plus persisted hub/link/satellite rows, so it is no longer limited to the pre-existing single-hub smoke lane.).",
    "AC check passed: Each live-provider bulk test validates observable batch correctness for its provider lane: expected row counts and saved-record results, provider-visible hub, link, and satellite data, and latest-state satellite HashDiff suppression when the current strategy path supports satellite writes. (The shared bulk assertion checks written-row count, saved-record count and ordering, provider-visible hub/link/satellite rows, and suppression of the unchanged satellite replay row, covering the observable batch-correctness requirements.).",
    "AC check passed: All new or updated live-provider tests continue to use the existing opt-in contract: deterministic skip when the matching DVAULT_TEST_*_CONNECTION_STRING value is absent, Category=ProviderIntegration.ExternalOptIn, the correct Provider trait, and no checked-in machine-specific configuration. (The provider test classes still carry \u0060Category=ProviderIntegration.ExternalOptIn\u0060 and provider traits, and the reused fixture methods keep the existing deterministic skip-on-missing-\u0060DVAULT_TEST_*_CONNECTION_STRING\u0060 contract without adding checked-in machine-specific configuration.).",
    "DoD check passed: The live tests reuse the existing provider configuration helpers, schema or table cleanup approach, and provider category discovery conventions instead of adding a second external-provider harness. (The new tests reuse \u0060ExternalProviderLiveSchemaFixture.CreatePostgresAsync/CreateSqlServerAsync/CreateOracleAsync/CreateMySqlAsync\u0060, the existing cleanup logic, and the existing provider-category conventions instead of adding a second external-provider harness.).",
    "DoD check passed: The bulk tests explicitly prove native-strategy execution under eligible conditions and would fail if the provider path silently fell back to the core writer. (The shared assertion requires \u0060ProviderStrategySelected\u0060, an expected provider strategy name, no fallback causes, persisted results, and no tracked rows after save, so the tests are designed to fail if the eligible batch silently falls back to the core writer.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: If provider test commands or coverage wording change, the affected README and v0.14 documentation text are updated to match the actual opt-in execution path. (\u0060README.md\u0060 was updated in the optional provider-test sections, but the same file still says Oracle declines satellite request batches in the provider-package overview, so the delivered guidance does not fully match the new Oracle bulk lane. The architecture note also remains on the old smoke/hub-link-only wording.).",
    "DoD check failed: The integration project contains external opt-in bulk-provider tests for the scoped providers and those tests pass when their documented local connection-string prerequisites are supplied. (The four external opt-in bulk tests are present, but this read-only review did not produce direct pass evidence for \u0060dotnet test DVault.slnx --nologo\u0060 with the required external provider connection strings, so the pass condition remains unverified.).",
    "DoD check failed: Ticket scope, repository guidance, and relation context remain aligned with the current split: fallback baseline in 06F2PGN4GPQCGC5WHZQBGP4SD0, native strategy implementation in 06F2PGNGVQ3TZZWSABAK5SNFK4, benchmarks in 06F2PGNZBRNCQ1SV2KKP6F3BA8, and broader docs closure in 06F2PGP2B2RZGGK3CVKK5WRRP8. (Repository guidance is not aligned with the delivered coverage: \u0060README.md\u0060 and \u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060 still describe Oracle as declining satellite batches, and the architecture note still describes SQL Server external opt-in coverage as a smoke lane rather than the new bulk lane.).",
    "\u0060README.md\u0060 is internally inconsistent for Oracle: the optional Oracle integration section documents provider-strategy bulk hub/link/satellite coverage, but the earlier provider-package overview still says Oracle declines request batches that contain satellite operations.",
    "\u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060 still documents Oracle as a hub/link-only optimized path and still describes SQL Server external opt-in validation as a one-hub/one-link/one-satellite smoke lane, so repository guidance is not aligned with the delivered bulk-provider coverage.",
    "The read-only tester session did not execute the documented verification commands against live external databases, so Definition of Done 1 remains unproven until deterministic legacy verification runs in a writable environment with the required provider connection strings and restore inputs."
  ],
  "evidence": [
    "\u0060git rev-parse HEAD\u0060 returned \u00608bdef825afa7186a67afc94204e1740a90b1add6\u0060, while the claimed implementation commit resolved to \u00603ad7f1cfcfca4766fdb1a08457a62782094f7228\u0060; \u0060git diff 3ad7f1cfcfca..HEAD -- README.md src tests docs\u0060 returned no code/doc paths, so the code review matched the claimed implementation and later branch movement was \u0060.gicket\u0060 metadata only.",
    "\u0060git diff --name-only develop...3ad7f1cfcfca\u0060 showed code changes in \u0060README.md\u0060, \u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060, \u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs\u0060, and the four provider integration test files; \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0060 was not changed.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs\u0060 creates a five-request \u0060DataVaultBulkSaveRequest\u0060 with 20 customer hubs, 20 order hubs, 20 links, and 4 satellite operations, then asserts \u0060ProviderStrategySelected\u0060, \u0060RowsWritten == 63\u0060, \u0060SavedRecords.Count == 64\u0060, saved-record ordering, persisted rows, and suppression of the unchanged replay satellite row.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs\u0060, \u0060SqlServerDataVaultSmokeTests.cs\u0060, \u0060OracleDataVaultSmokeTests.cs\u0060, and \u0060MySqlExplicitDataVaultSaveServiceTests.cs\u0060 each add a \u0060[Fact]\u0060 calling \u0060ExternalProviderBulkSaveAssertions.AssertProviderBulkSaveAsync(...)\u0060 with the existing fixture factory and \u0060AddDVault*()\u0060 registration for that provider.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderLiveSchemaFixtures.cs\u0060 already owns \u0060CreatePostgresAsync\u0060, \u0060CreateSqlServerAsync\u0060, \u0060CreateOracleAsync\u0060, and \u0060CreateMySqlAsync\u0060, including skip-on-missing-env behavior and provider-specific schema/table cleanup; the new coverage reuses that harness rather than introducing a new one.",
    "\u0060src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs\u0060 and \u0060src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs\u0060 now resolve physical table names from EF model metadata before executing inserts and latest-HashDiff lookups, matching the fixture\u0027s table-name overrides for MySQL prefixes and Oracle shortened table names.",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0060 still evaluates Oracle bulk eligibility by provider name, clean context, multi-active rejection, and a minimum 50 operations; it does not reject ordinary satellite batches, which matches the new Oracle bulk test shape.",
    "\u0060README.md\u0060 provider-package overview still says Oracle declines request batches that contain satellite operations, while the optional Oracle integration section now says the live lane includes an ordered bulk hub, link, and satellite batch through the provider strategy. \u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060 still describes Oracle as hub/link-only and SQL Server external opt-in coverage as a smoke lane.",
    "\u0060git diff --check develop...3ad7f1cfcfca -- README.md src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs tests/DCoding.Data.DVault.Tests/Integration/ExternalProviderBulkSaveAssertions.cs tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs tests/DCoding.Data.DVault.Tests/Integration/PostgresOptimizedDataVaultSaveServiceTests.cs tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs\u0060 produced no whitespace or patch-format errors.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/testing, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027develop\u0027.",
    "Ticket history references implementation commit \u00273ad7f1cfcfca\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update \u0060README.md\u0060 and \u0060docs/architecture/dvault-v1-explicit-save-service.md\u0060 so Oracle and SQL Server guidance matches the delivered bulk-provider coverage and current gate behavior.",
    "After the documentation alignment lands, rerun tester review on the updated claim.",
    "Run deterministic legacy verification for \u0060bash tools/check-format.sh\u0060 and \u0060dotnet test DVault.slnx --nologo\u0060 in a writable environment with the required \u0060DVAULT_TEST_POSTGRES_CONNECTION_STRING\u0060, \u0060DVAULT_TEST_SQLSERVER_CONNECTION_STRING\u0060, \u0060DVAULT_TEST_ORACLE_CONNECTION_STRING\u0060, and \u0060DVAULT_TEST_MYSQL_CONNECTION_STRING\u0060 inputs or the intended provider-filtered equivalent."
  ],
  "branchName": "ticket/06F2PGNT7DF4DVNKYWDFZC8DEM-task-add-provider-bulk-integration-coverage",
  "commitSha": "3ad7f1cfcfca"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F2PGNT7DF4DVNKYWDFZC8DEM`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F2PGNT7DF4DVNKYWDFZC8DEM-task-add-provider-bulk-integration-coverage`