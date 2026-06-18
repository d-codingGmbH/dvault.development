[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FBSCFVT3SBHKMDGNEXWVWFXG\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FBSCFVT3SBHKMDGNEXWVWFXG-task-close-mysql-latest-satellite-read-gap\u0027 and commit \u0027c4954337807b\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FBSCFVT3SBHKMDGNEXWVWFXG-task-close-mysql-latest-satellite-read-gap\u0027 from source \u0027c4954337807b\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FBSCFVT3SBHKMDGNEXWVWFXG-task-close-mysql-latest-satellite-read-gap\u0027.",
    "Evidence: git diff --name-only develop...c4954337807b shows code changes in src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs, src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs, src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs, and src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs, plus benchmark/docs/test updates for the MySQL latest-satellite outcome.",
    "Evidence: src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs now registers ServiceDescriptor.Singleton\u003CIDataVaultProviderReadStrategy, MySqlDataVaultReadStrategy\u003E(), and src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs adds CanReadLatestSatelliteRows(...) using DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(...).",
    "Evidence: src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs now contains ReadLatestSatelliteRowsAsync(...), ReadLatestSatelliteProjectionRowsAsync(...), ExecuteLatestRowsBatchAsync(...), and CreateLatestRowsCommandText(...), introducing the new relational latest-satellite SQL path.",
    "Evidence: The branch diff adds latest-satellite assertions only in tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs, tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs; tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs is unchanged in the diff.",
    "Evidence: A repository-wide test search for MySQL/latest-satellite execution coverage found gate, diagnostics, SQL-text, and benchmark expectation assertions, but no MySQL latest-satellite parity/integration test that executes MySqlDataVaultReadStrategy.ReadLatestSatelliteRowsAsync against seeded rows or compares its results/projections with the provider-neutral fallback path.",
    "Evidence: benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs now record the MySQL latest-satellite optional-provider row as selectedStrategy=MySqlDataVaultReadStrategy / plannedReadStrategy=MySqlDataVaultReadStrategy while keeping executionStatus/persisted outcome at skipped/not executed when DVAULT_TEST_MYSQL_CONNECTION_STRING is unset.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, provider/mysql, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FBSCFVT3SBHKMDGNEXWVWFXG-task-close-mysql-latest-satellite-read-gap\u0027.",
    "Evidence: Ticket history references implementation commit \u0027c4954337807b\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Repository evidence remains internally consistent for MySQL latest-satellite reads: either a MySQL-specific latest-satellite strategy is added and visibly registered or selected where appropriate, or the ticket lands explicit no-work-required or rejection documentation that preserves the current provider-neutral fallback baseline. (The branch adds a MySQL-specific latest-satellite strategy and wires it into registration/selection surfaces: src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs registers IDataVaultProviderReadStrategy, src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs adds CanReadLatestSatelliteRows(...), and benchmark/docs surfaces now reflect MySqlDataVaultReadStrategy for the MySQL latest-satellite row.).",
    "AC check passed: If a MySQL latest-satellite strategy is added, provider-neutral fallback remains intact for the bounded unsupported cases already implied by the repository baseline: provider mismatch, non-hub-parent satellites, and multi-active driving keys. (The fallback boundary remains explicit for the scoped unsupported cases. src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs adds MySQL latest-satellite gate evaluation and requirements for ProviderNameMismatch, UnsupportedSatelliteParent, and MultiActiveSatelliteUnsupported, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs asserts those decline paths.).",
    "AC check passed: The benchmark evidence surface for MySQL latest-satellite reads is updated to match the chosen outcome: implementation updates the checked-in guidance or evidence expectations for the MySQL row, while rejection keeps the row as a no-strategy fallback case and documents why. (The benchmark evidence surface was updated to the implementation outcome. benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, docs/plans/provider-optimization-evidence-matrix.md, docs/plans/provider-optimization-gap-matrix.md, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs now describe the MySQL latest-satellite row as a planned MySqlDataVaultReadStrategy path while keeping the optional-provider row skipped when no MySQL connection string is configured.).",
    "AC check passed: No ticket outcome may regress the established PIT or bridge MySQL posture or restate skipped-placeholder guidance as measured external-provider timing. (Nothing in the inspected diff shows a PIT/bridge posture regression, and the updated benchmark/docs surfaces do not restate skipped MySQL rows as measured timing. The MySQL latest-satellite row remains skipped in benchmark-summary.md/csv/json, while PIT and bridge rows still point to MySqlDataVaultReadStrategy as before.).",
    "DoD check passed: The code, tests, and docs baseline clearly states whether MySQL latest-satellite optimization exists or is explicitly rejected in the current release posture. (The code/docs/test baseline now states that MySQL latest-satellite optimization exists: registration and gate logic were added in src/, and the benchmark/docs/test expectation surfaces were updated accordingly.).",
    "DoD check passed: Checked-in benchmark guidance or evidence surfaces and related tests align with the selected MySQL latest-satellite outcome. (Checked-in benchmark guidance/evidence surfaces and the related benchmark expectation test align on the selected outcome: the MySQL latest-satellite optional-provider row now expects MySqlDataVaultReadStrategy while remaining a skipped placeholder without measured timing.).",
    "DoD check passed: Any no-work-required closure cites the existing evidence matrix, gap matrix, and root benchmark posture instead of leaving the ticket as an undocumented open gap. (Not applicable for this implementation path because the branch implements MySQL latest-satellite optimization rather than closing the ticket as no-work-required.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Diagnostics and automated tests cover the MySQL latest-satellite decision boundary so the repository no longer relies on implicit behavior for this shape. (The new tests cover registration, gate causes, diagnostics metadata, SQL text, and benchmark guidance, but the repository still lacks execution-level automated coverage for the selected MySQL latest-satellite path itself. The branch adds assertions in tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs, tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs, yet no branch change adds a parity/integration test that executes MySqlDataVaultReadStrategy.ReadLatestSatelliteRowsAsync against seeded rows or compares it with the provider-neutral fallback.).",
    "DoD check failed: Automated coverage proves the selected MySQL latest-satellite behavior and its fallback boundary. (Automated coverage does not yet prove the selected MySQL latest-satellite behavior. The repository adds no execution-parity or integration test for MySqlDataVaultReadStrategy latest-satellite reads, so the new SQL read path in src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs is not proven against seeded data and fallback behavior.).",
    "The branch introduces a new MySQL latest-satellite execution path, but it does not add automated execution-level proof for that path. Repository evidence currently proves registration, gate fallback causes, diagnostics metadata, SQL text shape, and benchmark guidance, yet it does not prove that MySqlDataVaultReadStrategy latest-satellite reads return the same seeded-row results/projections as the provider-neutral fallback or that the selected path behaves correctly end to end."
  ],
  "evidence": [
    "git diff --name-only develop...c4954337807b shows code changes in src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs, src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs, src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs, and src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs, plus benchmark/docs/test updates for the MySQL latest-satellite outcome.",
    "src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs now registers ServiceDescriptor.Singleton\u003CIDataVaultProviderReadStrategy, MySqlDataVaultReadStrategy\u003E(), and src/DCoding.Data.DVault.MySql/MySqlDataVaultReadStrategy.cs adds CanReadLatestSatelliteRows(...) using DataVaultProviderReadStrategyGateEvaluator.EvaluateMySql(...).",
    "src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs now contains ReadLatestSatelliteRowsAsync(...), ReadLatestSatelliteProjectionRowsAsync(...), ExecuteLatestRowsBatchAsync(...), and CreateLatestRowsCommandText(...), introducing the new relational latest-satellite SQL path.",
    "The branch diff adds latest-satellite assertions only in tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs, tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs; tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs is unchanged in the diff.",
    "A repository-wide test search for MySQL/latest-satellite execution coverage found gate, diagnostics, SQL-text, and benchmark expectation assertions, but no MySQL latest-satellite parity/integration test that executes MySqlDataVaultReadStrategy.ReadLatestSatelliteRowsAsync against seeded rows or compares its results/projections with the provider-neutral fallback path.",
    "benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs now record the MySQL latest-satellite optional-provider row as selectedStrategy=MySqlDataVaultReadStrategy / plannedReadStrategy=MySqlDataVaultReadStrategy while keeping executionStatus/persisted outcome at skipped/not executed when DVAULT_TEST_MYSQL_CONNECTION_STRING is unset.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, provider/mysql, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FBSCFVT3SBHKMDGNEXWVWFXG-task-close-mysql-latest-satellite-read-gap\u0027.",
    "Ticket history references implementation commit \u0027c4954337807b\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add execution-level automated coverage for MySQL latest-satellite reads, ideally by extending the existing parity-style read tests to seed latest-satellite rows, execute MySqlDataVaultReadStrategy through ReadLatestSatelliteRowsAsync and projection reads, and assert parity with the provider-neutral AddDVault() fallback plus as-of behavior.",
    "After the coverage gap is closed, rerun the repository verification commands in the supported verification path, including dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06FBSCFVT3SBHKMDGNEXWVWFXG-task-close-mysql-latest-satellite-read-gap",
  "commitSha": "c4954337807b"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FBSCFVT3SBHKMDGNEXWVWFXG`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FBSCFVT3SBHKMDGNEXWVWFXG-task-close-mysql-latest-satellite-read-gap`