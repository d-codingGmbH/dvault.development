[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FBSCG18KBRT1FTHDRX073EF4\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap\u0027 and commit \u00276d3cddac93ae\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap\u0027 from source \u00276d3cddac93ae\u0027.",
    "Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap\u0027.",
    "Evidence: git diff --name-only develop...6d3cddac93ae shows Oracle read-strategy code, tests, benchmark summaries, and documentation updates across src/, tests/, benchmarks/, docs/, and the root benchmark summary triplet, but it does not include docs/releases/v0.28.0.md.",
    "Evidence: src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:24-26 adds OracleDataVaultReadStrategy registrations for IDataVaultProviderReadStrategy, IDataVaultProviderPitReadStrategy, and IDataVaultProviderBridgeReadStrategy.",
    "Evidence: src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs:17-260 implements Oracle latest-satellite gating plus current/as-of row selection using Oracle bind placeholders and ROW_NUMBER latest-row SQL.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:10-87 compares Oracle latest-satellite current and as-of rows/projections with provider-neutral fallback.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:252-257 now expects AddDVaultOracle() to register OracleDataVaultReadStrategy for latest-satellite reads.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:463-465 and benchmark-summary.csv:51-53 record Oracle latest-satellite, PIT, and bridge guidance rows with selectedStrategy=OracleDataVaultReadStrategy and skipped not-configured placeholders.",
    "Evidence: docs/plans/provider-optimization-evidence-matrix.md:265-267, docs/plans/provider-optimization-gap-matrix.md:54, docs/performance-profiles.md:125/179/354/364, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md:17/19/128 describe Oracle latest-satellite as a diagnostics-gated planned strategy rather than a no-strategy gap.",
    "Evidence: docs/releases/v0.28.0.md:29-41 says Oracle has a diagnostics-gated latest-satellite strategy candidate, but docs/releases/v0.28.0.md:127 still says Oracle does not add a provider-specific latest-satellite strategy.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, provider/oracle, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap\u0027.",
    "Evidence: Ticket history references implementation commit \u00276d3cddac93ae\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: With AddDVaultOracle() registered and an Oracle provider context in use, IDataVaultReadDiagnosticsService selects OracleDataVaultReadStrategy for supported DataVaultLatestSatelliteReadRequest shapes and preserves provider-neutral fallback for provider mismatch, unsupported satellite parents, and multi-active driving-key satellites. (AddDVaultOracle now registers OracleDataVaultReadStrategy as IDataVaultProviderReadStrategy in src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs, and latest-satellite Oracle gate evaluation plus fallback requirements are covered in DataVaultProviderReadStrategyGateEvaluator and DataVaultProviderReadStrategyTests.).",
    "AC check passed: Oracle latest-satellite current and as-of reads return the same rows and typed projections as the provider-neutral read path for supported shapes, with repository tests covering both semantics. (OracleDataVaultReadStrategy implements latest-satellite current/as-of execution in src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs, and DataVaultRelationalPitBridgeReadStrategyParityTests compares Oracle latest and as-of rows/projections with provider-neutral fallback for supported shapes.).",
    "AC check passed: Oracle latest-satellite gate metadata becomes first-class repository evidence: registration tests, gate-requirement or fallback tests, and diagnostics tests all surface the finite latest-satellite requirements and fallback causes for Oracle. (Registration, gate, diagnostics, and SQL-shape evidence were updated together: ExplicitDataVaultSaveServiceTests expects Oracle latest-satellite registration, DataVaultProviderReadStrategyTests and DataVaultDiagnosticsTests cover Oracle gate metadata and fallback causes, and OracleProviderOptimizationTests asserts the Oracle latest-satellite SQL shape.).",
    "AC check passed: Benchmark expectation surfaces are updated so the Oracle latest-satellite-read row records readShape=LatestSatellite and OracleDataVaultReadStrategy as the planned or selected provider strategy instead of a no-strategy fallback-only posture; skipped rows remain visible with normalized skip reasons when Oracle is not configured. (Benchmark expectation surfaces now record Oracle latest-satellite as readShape=LatestSatellite with OracleDataVaultReadStrategy in BenchmarkScenarioExecutionTests and the root benchmark summary triplet, while the Oracle row remains a skipped placeholder when no Oracle connection string is configured.).",
    "DoD check passed: No existing SQLite or SQL Server latest-satellite behavior regresses, and existing Oracle PIT or bridge candidate behavior remains intact. (The diff preserves existing SQL Server latest-satellite code and Oracle PIT/bridge paths, and added parity coverage compares SQL Server and Oracle latest-satellite behavior against provider-neutral fallback without widening the supported shape.).",
    "DoD check passed: If Oracle is not configured in the validation environment, checked-in evidence still preserves a truthful skipped-placeholder posture and does not fabricate completed Oracle timing results. (The checked-in Oracle benchmark summary rows remain skipped placeholders with normalized not-configured reasons and planned OracleDataVaultReadStrategy metadata rather than fabricated timing results.).",
    "DoD check passed: If implementation cannot satisfy the bounded parity and fallback contract without widening supported shapes, the ticket stops at explicit no-work-required evidence instead of silently broadening the Oracle read contract. (Observed gate logic remains bounded to Oracle provider match, hub-parent satellites, and non-multi-active shapes in DataVaultProviderReadStrategyGateEvaluator and OracleDataVaultReadStrategy; the implementation does not widen the contract.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Repository evidence documents and guidance that currently classify Oracle latest-satellite as a capability gap are updated consistently to the post-implementation evidence posture, with no checked-in document still claiming Oracle latest-satellite has no provider-specific strategy. (docs/releases/v0.28.0.md still says at line 127 that v0.28.0 does not add provider-specific latest-satellite strategies for PostgreSQL, SQL Server, MySQL, or Oracle, which directly contradicts the updated Oracle latest-satellite posture described elsewhere in the repo.).",
    "DoD check failed: Affected Oracle latest-satellite code, tests, diagnostics surfaces, benchmark expectation or verifier surfaces, and evidence or guidance docs are updated together and pass repository validation. (Oracle latest-satellite code, tests, diagnostics, and benchmark surfaces were updated, but evidence docs are not fully consistent because docs/releases/v0.28.0.md still preserves the obsolete Oracle no-strategy statement; repository validation commands were not re-run in this read-only session.).",
    "docs/releases/v0.28.0.md:127 still states that Oracle has no provider-specific latest-satellite strategy, so acceptance criterion 5 and definition-of-done item 1 are not met even though the current code, tests, and most guidance surfaces were updated to the new Oracle posture."
  ],
  "evidence": [
    "git diff --name-only develop...6d3cddac93ae shows Oracle read-strategy code, tests, benchmark summaries, and documentation updates across src/, tests/, benchmarks/, docs/, and the root benchmark summary triplet, but it does not include docs/releases/v0.28.0.md.",
    "src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:24-26 adds OracleDataVaultReadStrategy registrations for IDataVaultProviderReadStrategy, IDataVaultProviderPitReadStrategy, and IDataVaultProviderBridgeReadStrategy.",
    "src/DCoding.Data.DVault.Oracle/OracleDataVaultReadStrategy.cs:17-260 implements Oracle latest-satellite gating plus current/as-of row selection using Oracle bind placeholders and ROW_NUMBER latest-row SQL.",
    "tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:10-87 compares Oracle latest-satellite current and as-of rows/projections with provider-neutral fallback.",
    "tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:252-257 now expects AddDVaultOracle() to register OracleDataVaultReadStrategy for latest-satellite reads.",
    "tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:463-465 and benchmark-summary.csv:51-53 record Oracle latest-satellite, PIT, and bridge guidance rows with selectedStrategy=OracleDataVaultReadStrategy and skipped not-configured placeholders.",
    "docs/plans/provider-optimization-evidence-matrix.md:265-267, docs/plans/provider-optimization-gap-matrix.md:54, docs/performance-profiles.md:125/179/354/364, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md:17/19/128 describe Oracle latest-satellite as a diagnostics-gated planned strategy rather than a no-strategy gap.",
    "docs/releases/v0.28.0.md:29-41 says Oracle has a diagnostics-gated latest-satellite strategy candidate, but docs/releases/v0.28.0.md:127 still says Oracle does not add a provider-specific latest-satellite strategy.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, provider/oracle, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap\u0027.",
    "Ticket history references implementation commit \u00276d3cddac93ae\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update docs/releases/v0.28.0.md so its limitations section no longer says Oracle lacks a provider-specific latest-satellite strategy and keep the wording aligned with the current provider evidence matrix and performance profiles.",
    "After that documentation contradiction is fixed, run repository validation through the supported verification path for dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap",
  "commitSha": "6d3cddac93ae"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FBSCG18KBRT1FTHDRX073EF4`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FBSCG18KBRT1FTHDRX073EF4-task-close-oracle-latest-satellite-read-gap`