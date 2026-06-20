[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FE4QPR8TF8R6PXNM3RMXN8JG\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w\u0027 and commit \u0027f002b3468257\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w\u0027 from source \u0027f002b3468257\u0027.",
    "Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w\u0027.",
    "Evidence: git diff --name-only 8335e48c34bb..f002b3468257 includes benchmark-summary.csv/json/md, benchmark helper/test files, and both docs/plans/provider-optimization-evidence-matrix.md and docs/plans/provider-optimization-gap-matrix.md.",
    "Evidence: benchmark-summary.csv:42 records the PostgreSQL latest-satellite root row as executionStatus=skipped with selectedStrategy=PostgresDataVaultReadStrategy, plannedReadStrategy=PostgresDataVaultReadStrategy, readShape=LatestSatellite, latestSatelliteSqlShape=windowed-row-number, and persistedOutcome=not executed.",
    "Evidence: artifacts/benchmarks/v0.31.0-all-providers-smoke-20260606/benchmark-summary.csv:33 preserves the historical PostgreSQL latest-satellite comparator as a completed provider-neutral fallback row with mean 25.723 ms and selectedStrategy=\u003Cnone\u003E.",
    "Evidence: benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:122-135 now expects PostgresDataVaultReadStrategy for latest-satellite-read, and benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs:170-180 adds the retained-shape execution-detail token.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs:102-130 asserts the retained ROW_NUMBER latest-satellite SQL, tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:454-460 and 1442-1460 assert the artifact/detail tokens, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:178-196 assert PostgreSQL fallback gates.",
    "Evidence: docs/plans/provider-optimization-evidence-matrix.md:280 and docs/plans/provider-optimization-gap-matrix.md:84 contain new broader matrix-promotion text on this branch.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, automation/bot-ready, needs-test, provider/postgres, type/task, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w\u0027.",
    "Evidence: Ticket history references implementation commit \u0027f002b3468257\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: For PostgreSQL latest-satellite reads, the implemented path is either a measured improvement or an evidence-backed retain-current decision, and the preserved artifact clearly shows the comparator used. (The retained decision is repository-backed: benchmark-summary.csv:42 keeps the PostgreSQL latest-satellite row as a checked-in skipped placeholder with planned strategy and retained-shape tokens, and artifacts/benchmarks/v0.31.0-all-providers-smoke-20260606/benchmark-summary.csv:33 preserves the historical provider-neutral comparator.).",
    "AC check passed: If the PostgreSQL SQL shape changes, unit and integration coverage still proves provider-neutral parity for supported shapes and still rejects provider mismatch, link-parent satellites, and multi-active satellites with provider-neutral fallback. (The SQL shape was retained rather than changed, and repository tests still cover the retained ROW_NUMBER query plus fallback gates in tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs:102-130 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:178-196.).",
    "AC check passed: Benchmark or diagnostics output for the PostgreSQL latest-satellite lane makes the chosen path auditable with bounded tokens such as selectedStrategy, plannedReadStrategy, readShape=LatestSatellite, and fallback causes when applicable. (Auditable benchmark tokens are wired through benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs:170-180, benchmark-summary.csv:42, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:454-460 and 1442-1460.).",
    "AC check passed: No documentation or code in this ticket promotes the root skipped PostgreSQL latest-satellite row into completed timing evidence without a provider-configured completed run. (The root PostgreSQL latest-satellite row remains skipped/not-executed in benchmark-summary.csv:42, and the updated matrix text still says completed PostgreSQL timing requires a configured provider run rather than promoting the skipped row into completed evidence.).",
    "DoD check passed: A developer can point to one authoritative PostgreSQL latest-satellite decision: tuned SQL shape or explicit retention of the current windowed query, with preserved evidence for why. (A single retained-windowed-query decision is explicit in tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs:102-130, and the preserved historical comparator remains checked in at artifacts/benchmarks/v0.31.0-all-providers-smoke-20260606/benchmark-summary.csv:33.).",
    "DoD check passed: Repository tests cover the PostgreSQL latest-satellite command shape or selection behavior being kept, changed, or intentionally retained, plus fallback and parity behavior. (Repository tests cover the retained SQL shape, planned benchmark-detail tokens, and fallback behavior in tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs:102-130, tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:454-460 and 1442-1460, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:178-196.).",
    "DoD check passed: The ticket leaves the provider boundary unchanged: PostgresDataVaultReadStrategy is diagnostics-gated and provider-neutral fallback remains the public safety net. (The provider boundary remains diagnostics-gated: src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs:728-731 keeps the PostgreSQL latest-satellite fallback requirements unchanged, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:1102-1116 still assert the known PostgreSQL gate requirements.).",
    "DoD check passed: Any evidence cited for the decision is stored as a preserved benchmark artifact or checked-in contract surface, not a transient local observation. (The cited decision evidence is checked in as contract surfaces and preserved artifacts, including benchmark-summary.csv:42 and artifacts/benchmarks/v0.31.0-all-providers-smoke-20260606/benchmark-summary.csv:33, rather than depending on transient local output.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Any targeted diagnostics or narrow developer-facing notes added here explain the chosen PostgreSQL path while leaving broader evidence-matrix and release-document promotion to the downstream docs ticket. (This branch edits broader evidence-matrix documents in docs/plans/provider-optimization-evidence-matrix.md:280 and docs/plans/provider-optimization-gap-matrix.md:84 even though the contract leaves broader evidence-matrix and release-document promotion to downstream docs ticket 06FE4QRMXVGJVA65ZR5MZ817K8.).",
    "DoD check failed: Downstream docs work has enough bounded input to update matrices and release notes without reopening the strategy-selection decision. (Downstream docs input is no longer cleanly isolated because this ticket already updates the broader provider evidence/gap matrices in docs/plans/provider-optimization-evidence-matrix.md and docs/plans/provider-optimization-gap-matrix.md instead of leaving that promotion work to downstream ticket 06FE4QRMXVGJVA65ZR5MZ817K8.).",
    "Blocking: the branch changes broader evidence-matrix documents (docs/plans/provider-optimization-evidence-matrix.md:280 and docs/plans/provider-optimization-gap-matrix.md:84), but the contract scopes that promotion work to downstream docs ticket 06FE4QRMXVGJVA65ZR5MZ817K8 rather than this implementation ticket."
  ],
  "evidence": [
    "git diff --name-only 8335e48c34bb..f002b3468257 includes benchmark-summary.csv/json/md, benchmark helper/test files, and both docs/plans/provider-optimization-evidence-matrix.md and docs/plans/provider-optimization-gap-matrix.md.",
    "benchmark-summary.csv:42 records the PostgreSQL latest-satellite root row as executionStatus=skipped with selectedStrategy=PostgresDataVaultReadStrategy, plannedReadStrategy=PostgresDataVaultReadStrategy, readShape=LatestSatellite, latestSatelliteSqlShape=windowed-row-number, and persistedOutcome=not executed.",
    "artifacts/benchmarks/v0.31.0-all-providers-smoke-20260606/benchmark-summary.csv:33 preserves the historical PostgreSQL latest-satellite comparator as a completed provider-neutral fallback row with mean 25.723 ms and selectedStrategy=\u003Cnone\u003E.",
    "benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:122-135 now expects PostgresDataVaultReadStrategy for latest-satellite-read, and benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs:170-180 adds the retained-shape execution-detail token.",
    "tests/DCoding.Data.DVault.Tests/Unit/PostgresProviderCapabilityTests.cs:102-130 asserts the retained ROW_NUMBER latest-satellite SQL, tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:454-460 and 1442-1460 assert the artifact/detail tokens, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:178-196 assert PostgreSQL fallback gates.",
    "docs/plans/provider-optimization-evidence-matrix.md:280 and docs/plans/provider-optimization-gap-matrix.md:84 contain new broader matrix-promotion text on this branch.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, automation/bot-ready, needs-test, provider/postgres, type/task, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 5 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w\u0027.",
    "Ticket history references implementation commit \u0027f002b3468257\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Remove or move the broader provider evidence/gap matrix edits out of this ticket so the branch only carries the bounded PostgreSQL strategy, artifact, diagnostics, and test surfaces required here.",
    "After the scope issue is corrected, run legacy verification for bash tools/check-format.sh, dotnet build DVault.slnx --nologo, and dotnet test DVault.slnx --nologo in the supported environment before re-handoff to test."
  ],
  "branchName": "ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w",
  "commitSha": "f002b3468257"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FE4QPR8TF8R6PXNM3RMXN8JG`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w`