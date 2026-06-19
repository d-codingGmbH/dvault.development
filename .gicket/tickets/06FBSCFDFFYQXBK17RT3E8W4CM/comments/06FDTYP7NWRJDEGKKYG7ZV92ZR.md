[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FBSCFDFFYQXBK17RT3E8W4CM\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap\u0027 and commit \u0027952f9ec9fa18\u0027 (ticket-comment branch\u002Bcommit reference; advanced to branch tip after newer repository changes).",
    "Advanced tester verification from stale pinned commit \u00273198a33cf6bb\u0027 to branch tip \u0027952f9ec9fa18\u0027 because branch \u0027ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap\u0027 contains newer committed repository changes after the pinned commit.",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap\u0027 from source \u0027952f9ec9fa18\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap\u0027.",
    "Evidence: git rev-parse resolved ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap to 952f9ec9fa18187951fce7cb8629c1d97c2ee4c1, and git diff --name-status develop...that branch shows product changes in src/, tests/, benchmarks/, docs/, and benchmark-summary.*.",
    "Evidence: src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-26 now registers PostgresDataVaultReadStrategy as IDataVaultProviderReadStrategy in addition to the existing PIT and bridge strategy interfaces.",
    "Evidence: src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-16 and src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs:44-50 add PostgreSQL latest-satellite gate evaluation on the read-service boundary.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:243-259 expects PostgreSQL latest-satellite read registration, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:125-175 adds latest-satellite parity coverage against the provider-neutral path.",
    "Evidence: benchmark-summary.md:75, benchmark-summary.csv, benchmark-summary.json, and benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs:176-181 now advertise selectedStrategy=PostgresDataVaultReadStrategy and plannedReadStrategy=PostgresDataVaultReadStrategy for the PostgreSQL latest-satellite guidance row.",
    "Evidence: docs/plans/provider-optimization-evidence-matrix.md:255 keeps the PostgreSQL latest-satellite row as skipped-placeholder guidance, and docs/releases/v0.40.0.md:67 states that v0.40.0 adds PostgreSQL latest-satellite strategy selection guidance without adding new completed timing claims.",
    "Evidence: benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:127-130 still returns a latest-satellite read strategy name only for SQLite; benchmarks/DCoding.Data.DVault.Benchmarks/ReadBenchmarkServices.cs:24-30 and benchmarks/DCoding.Data.DVault.Benchmarks/LatestSatelliteReadBenchmark.cs:80-85 therefore skip AssertProviderReadStrategySelected for PostgreSQL latest-satellite benchmark runs.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, provider/postgres, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 2 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap\u0027.",
    "Evidence: Ticket history references implementation commit \u0027ea0c3363dc33971735d282a244823d7de4937d14\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "AC check passed: Any implemented outcome adds a PostgreSQL latest-satellite provider strategy on the existing read-service boundary without widening PIT/bridge scope, and request-bound diagnostics must show provider strategy selection for supported shapes and bounded fallback for unsupported or declined shapes. (The implementation adds a PostgreSQL latest-satellite provider strategy on the existing read-service boundary: AddDVaultPostgres() now registers IDataVaultProviderReadStrategy and PostgresDataVaultReadStrategy now gates latest-satellite dispatch through EvaluatePostgres without widening PIT/bridge scope.).",
    "AC check passed: Tests cover the chosen outcome: service registration or absence thereof, latest-satellite dispatch behavior, finite fallback behavior, and the expected diagnostics surface for PostgreSQL latest-satellite reads. (Repository tests cover the implemented outcome through service-registration coverage, latest-satellite parity/dispatch coverage, bounded fallback gate coverage, and diagnostics-surface coverage in ExplicitDataVaultSaveServiceTests, DataVaultRelationalPitBridgeReadStrategyParityTests, DataVaultProviderReadStrategyTests, and DataVaultDiagnosticsTests.).",
    "AC check passed: Any implemented PostgreSQL performance claim is backed by completed benchmark evidence with preserved triplet/run context and compared against the provider-neutral latest-satellite baseline; skipped-placeholder guidance rows do not satisfy this gate. (The branch does not promote PostgreSQL latest-satellite to a completed timing claim: benchmark-summary.* keeps the PostgreSQL latest-satellite row as skipped/not executed guidance, and docs/releases/v0.40.0.md explicitly says no new completed timing claims were added.).",
    "AC check passed: Any no-work-required outcome explicitly cites the current repository posture: AddDVaultPostgres() does not register a latest-satellite provider strategy, benchmark guidance rows keep selectedStrategy=\u003Cnone\u003E, and fallback remains NoProviderSpecificStrategyRegistered/provider-neutral. (Not applicable to the implemented branch outcome; this criterion only applies to a no-work-required closure.).",
    "DoD check passed: The ticket no longer reopens baseline questions about provider list, evidence vocabulary, or whether PIT/bridge work is included; PostgreSQL latest-satellite is the only delivery lane. (The branch stays on the PostgreSQL latest-satellite lane and updates the gap/evidence docs so SQL Server, MySQL, Oracle, and DB2 remain separate latest-satellite capability gaps.).",
    "DoD check passed: Closure evidence cites the authoritative repository surfaces for this lane: the gap matrix P0.01 row, the evidence matrix PostgreSQL latest-satellite row, benchmark guidance/tests, and the PostgreSQL registration surface. (The branch updates the cited repository surfaces for this lane, including the PostgreSQL registration surface, benchmark guidance rows, provider-optimization evidence matrix, and provider-optimization gap matrix.).",
    "DoD check passed: If closed as no-work-required, closure evidence states why the current capability-gap posture remains the correct bounded outcome and leaves the outbound docs ticket ready to record that decision. (Not applicable to the implemented branch outcome; this definition-of-done item only applies to a no-work-required closure.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: The ticket closes with one of two explicit outcomes only: implemented PostgreSQL latest-satellite optimization with proof, or no-work-required with repository-backed rationale for retaining provider-neutral fallback. (The branch clearly takes the implemented outcome, but the closure proof is incomplete because the PostgreSQL latest-satellite benchmark path still skips strategy-selection validation for the optimized lane.).",
    "DoD check failed: If implemented, closure evidence includes updated diagnostics/tests/benchmark artifacts sufficient to prove the selected strategy and bounded fallback behavior. (The benchmark artifacts are not yet sufficient to prove the selected PostgreSQL latest-satellite strategy because the live benchmark assertion path still skips provider read-strategy validation for that scenario.).",
    "benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:127-130 leaves the PostgreSQL optimized latest-satellite lane unmapped, so LatestSatelliteReadBenchmark never asserts that diagnostics actually selected PostgresDataVaultReadStrategy. That disconnect leaves the implemented benchmark proof unwired: a configured PostgreSQL latest-satellite benchmark could complete under the optimized baseline even if it fell back to the provider-neutral read path."
  ],
  "evidence": [
    "git rev-parse resolved ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap to 952f9ec9fa18187951fce7cb8629c1d97c2ee4c1, and git diff --name-status develop...that branch shows product changes in src/, tests/, benchmarks/, docs/, and benchmark-summary.*.",
    "src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:21-26 now registers PostgresDataVaultReadStrategy as IDataVaultProviderReadStrategy in addition to the existing PIT and bridge strategy interfaces.",
    "src/DCoding.Data.DVault.Postgres/PostgresDataVaultReadStrategy.cs:10-16 and src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs:44-50 add PostgreSQL latest-satellite gate evaluation on the read-service boundary.",
    "tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:243-259 expects PostgreSQL latest-satellite read registration, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:125-175 adds latest-satellite parity coverage against the provider-neutral path.",
    "benchmark-summary.md:75, benchmark-summary.csv, benchmark-summary.json, and benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs:176-181 now advertise selectedStrategy=PostgresDataVaultReadStrategy and plannedReadStrategy=PostgresDataVaultReadStrategy for the PostgreSQL latest-satellite guidance row.",
    "docs/plans/provider-optimization-evidence-matrix.md:255 keeps the PostgreSQL latest-satellite row as skipped-placeholder guidance, and docs/releases/v0.40.0.md:67 states that v0.40.0 adds PostgreSQL latest-satellite strategy selection guidance without adding new completed timing claims.",
    "benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:127-130 still returns a latest-satellite read strategy name only for SQLite; benchmarks/DCoding.Data.DVault.Benchmarks/ReadBenchmarkServices.cs:24-30 and benchmarks/DCoding.Data.DVault.Benchmarks/LatestSatelliteReadBenchmark.cs:80-85 therefore skip AssertProviderReadStrategySelected for PostgreSQL latest-satellite benchmark runs.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, provider/postgres, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 2 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic.",
    "Ticket history references implementation branch \u0027ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap\u0027.",
    "Ticket history references implementation commit \u0027ea0c3363dc33971735d282a244823d7de4937d14\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs so latest-satellite-read returns PostgresDataVaultReadStrategy for DataVaultBenchmarkStrategy.PostgresOptimized, and add targeted test coverage for the benchmark strategy-selection assertion path.",
    "After that fix, rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh via legacy verification before resubmitting to test."
  ],
  "branchName": "ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap",
  "commitSha": "952f9ec9fa18"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FBSCFDFFYQXBK17RT3E8W4CM`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FBSCFDFFYQXBK17RT3E8W4CM-task-close-postgresql-latest-satellite-read-gap`