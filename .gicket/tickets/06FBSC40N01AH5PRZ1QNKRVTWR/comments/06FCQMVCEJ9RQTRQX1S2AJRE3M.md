[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FBSC40N01AH5PRZ1QNKRVTWR\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FBSC40N01AH5PRZ1QNKRVTWR-task-add-binary-vs-hex-provider-benchmark-dimens\u0027 and commit \u00278c63b102a05a\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FBSC40N01AH5PRZ1QNKRVTWR-task-add-binary-vs-hex-provider-benchmark-dimens\u0027 from source \u00278c63b102a05a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FBSC40N01AH5PRZ1QNKRVTWR-task-add-binary-vs-hex-provider-benchmark-dimens\u0027.",
    "Evidence: Diff develop...8c63b102a05a changes only benchmark-summary.md, benchmark-summary.json, benchmarks/DCoding.Data.DVault.Benchmarks/README.md, docs/local-validation.md, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs in repository content relevant to this ticket.",
    "Evidence: benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs appends variantSource.HashKeyVariant.CreateExecutionDetail() for any IBenchmarkHashKeyVariantSource row, and BenchmarkHashKeyVariant.CreateExecutionDetail() starts with hashKeyVariant=\u003Clabel\u003E.",
    "Evidence: The required output benchmark-summary.json at commit 8c63b102a05a now includes context.hashKeyVariants=[sha256-v1-hex], but DVault result rows such as dvault-adddvault-fallback, dvault-adddvaultsqlite-optimized, and latest-satellite-read still omit hashKeyVariant= in executionDetail; benchmark-summary.md shows the same omission in its DVault rows.",
    "Evidence: The checked-in SQLite matrix bundle artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.md and benchmark-summary.json include providerFilter=sqlite, all four variant labels, and hashKeyVariant= execution detail on DVault rows.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs adds a PostgreSQL-filtered matrix test that expects 24 skipped placeholder rows across the four variants and asserts context hashKeyVariants, but the root artifact assertions still do not require hashKeyVariant= on the checked-in root benchmark summary rows.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/benchmarking, area/hashing, area/performance, area/provider-support, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FBSC40N01AH5PRZ1QNKRVTWR-task-add-binary-vs-hex-provider-benchmark-dimens\u0027.",
    "Evidence: Ticket history references implementation commit \u00278c63b102a05a\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: A bounded hash-key matrix run can emit comparable benchmark-summary artifacts for SQLite plus any configured PostgreSQL, SQL Server, MySQL, or Oracle lane, using the shared markdown, CSV, and JSON contract and preserving skipped rows for unconfigured providers. (BenchmarkOptions.Parse exposes the bounded four-variant matrix, BenchmarkRunner iterates EffectiveHashKeyVariants for SQLite and provider lanes, BenchmarkArtifacts.WriteAsync still emits benchmark-summary.md/csv/json, and the new PostgreSQL-filtered integration test expects 24 skipped placeholder rows across the four variants.).",
    "AC check passed: Optional-provider save and read rows remain present under each variant with the same planned or selected strategy facts and normalized skip-reason behavior already used by the provider optimization matrix. (BenchmarkRunner.CreateProviderBenchmarks creates provider-native bulk, latest-satellite, PIT, and bridge rows for every variant, BenchmarkExecutionDetails preserves planned strategy facts for skipped rows, and the new provider-filtered test verifies 24 skipped PostgreSQL placeholder rows across the four variants.).",
    "AC check passed: The run context preserves hashKeyVariants, providerFilter, required and optional provider execution status, iterations, warmup iterations, load-timestamp storage, and runtime environment so binary-vs-hex comparisons stay machine- and provider-context aware. (BenchmarkRunContext carries hashKeyVariants, providerFilter, iterations, warmupIterations, loadTimestampStorage, runtime details, and optional provider status, and benchmark-summary.md/json plus the updated tests now assert those context fields.).",
    "AC check passed: When a matrix run includes more than one variant, SQLite hash-key-footprint sidecars are still emitted and docs explicitly scope them as supplemental SQLite-local storage evidence rather than cross-provider timing proof. (BenchmarkHashKeyFootprintArtifacts.WriteAsync emits sidecars whenever more than one hash-key variant is present, and the README, hash-key-footprint.md, and provider-optimization-evidence-matrix scope that evidence to supplemental SQLite-local storage facts rather than cross-provider timing claims.).",
    "AC check passed: Benchmark guidance clearly states that the configured external-provider set for this ticket is PostgreSQL, SQL Server, MySQL, and Oracle, while DB2 remains outside the benchmark lane baseline. (benchmarks/DCoding.Data.DVault.Benchmarks/README.md and docs/local-validation.md now explicitly name PostgreSQL, SQL Server, MySQL, and Oracle as the external benchmark-lane set and explicitly keep DB2 outside this benchmark baseline.).",
    "DoD check passed: A standard matrix run that includes SQLite can generate contract-compliant artifacts without custom post-processing or consumer-only setup beyond the already documented optional provider environment variables. (The checked-in SQLite matrix bundle under artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612 demonstrates a SQLite-inclusive four-variant run with the shared markdown/CSV/JSON contract and same-directory footprint sidecars, and BenchmarkArtifacts.WriteAsync generates those filenames directly.).",
    "DoD check passed: Public docs and release or planning references describe benchmark execution as optional evidence tooling, not as a runtime prerequisite for consumers who adopt binary hash-key storage. (Benchmark README and local-validation guidance keep benchmark execution framed as optional evidence tooling behind documented environment-variable gates rather than a runtime prerequisite for consumers.).",
    "DoD check passed: No repository guidance overstates skipped, diagnostics-only, smoke-only, or SQLite-local storage-footprint evidence as guaranteed measured cross-provider performance. (The README, hash-key-footprint.md, and provider-optimization-evidence-matrix explicitly bound skipped provider rows, diagnostics-only boundaries, and SQLite-local storage-footprint claims instead of overstating them as guaranteed measured cross-provider performance.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: The bounded variant set is exactly sha256-v1-hex, sha256-v1-binary, sha256-128-v1-hex, and sha256-128-v1-binary, and each emitted row preserves deterministic hashKeyVariant execution detail without inventing new row fields. (The four labels exist, but the required checked-in outputs benchmark-summary.md and benchmark-summary.json still contain DVault result executionDetail values without hashKeyVariant=. That conflicts with the deterministic hashKeyVariant execution-detail requirement and the current harness behavior in BenchmarkExecutionDetails.CreateDetail(...).).",
    "DoD check failed: Repository tests, benchmark harness behavior, and benchmark guidance tell one consistent story about the four-variant matrix and the existing optional provider set. (The repository surfaces do not tell one consistent story yet: the docs and tests describe preserved hashKeyVariant metadata, but the required checked-in root benchmark summary outputs still omit hashKeyVariant from DVault row executionDetail.).",
    "DoD check failed: Downstream evidence collection can treat this ticket as the harness or dimension prerequisite without reopening provider set, variant set, or artifact-contract decisions. (Downstream evidence collection would still inherit or need to reconcile stale required root benchmark-summary outputs before this ticket can be treated as the settled harness/artifact prerequisite.).",
    "The only required repository outputs for this ticket, benchmark-summary.md and benchmark-summary.json, are stale relative to the ticket contract: their DVault result rows still omit hashKeyVariant= from executionDetail, so acceptance criterion 2 is not met.",
    "The updated test coverage does not enforce row-level hashKeyVariant= on the checked-in root benchmark summary outputs, which allowed the stale required artifacts to remain undetected."
  ],
  "evidence": [
    "Diff develop...8c63b102a05a changes only benchmark-summary.md, benchmark-summary.json, benchmarks/DCoding.Data.DVault.Benchmarks/README.md, docs/local-validation.md, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs in repository content relevant to this ticket.",
    "benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkExecutionDetails.cs appends variantSource.HashKeyVariant.CreateExecutionDetail() for any IBenchmarkHashKeyVariantSource row, and BenchmarkHashKeyVariant.CreateExecutionDetail() starts with hashKeyVariant=\u003Clabel\u003E.",
    "The required output benchmark-summary.json at commit 8c63b102a05a now includes context.hashKeyVariants=[sha256-v1-hex], but DVault result rows such as dvault-adddvault-fallback, dvault-adddvaultsqlite-optimized, and latest-satellite-read still omit hashKeyVariant= in executionDetail; benchmark-summary.md shows the same omission in its DVault rows.",
    "The checked-in SQLite matrix bundle artifacts/benchmarks/06F9GF66B10J4K7RBDTJ9NQRQC-hash-key-storage-matrix-sqlite-20260612/benchmark-summary.md and benchmark-summary.json include providerFilter=sqlite, all four variant labels, and hashKeyVariant= execution detail on DVault rows.",
    "tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs adds a PostgreSQL-filtered matrix test that expects 24 skipped placeholder rows across the four variants and asserts context hashKeyVariants, but the root artifact assertions still do not require hashKeyVariant= on the checked-in root benchmark summary rows.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/benchmarking, area/hashing, area/performance, area/provider-support, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FBSC40N01AH5PRZ1QNKRVTWR-task-add-binary-vs-hex-provider-benchmark-dimens\u0027.",
    "Ticket history references implementation commit \u00278c63b102a05a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Refresh the required root benchmark-summary.md and benchmark-summary.json outputs so every DVault save/read row carries deterministic hashKeyVariant= execution detail consistent with the current harness contract.",
    "Tighten BenchmarkScenarioExecutionTests.cs root artifact assertions to require hashKeyVariant= on the checked-in benchmark summary DVault rows.",
    "After those fixes land, rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh through the normal verification path before returning to test."
  ],
  "branchName": "ticket/06FBSC40N01AH5PRZ1QNKRVTWR-task-add-binary-vs-hex-provider-benchmark-dimens",
  "commitSha": "8c63b102a05a"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FBSC40N01AH5PRZ1QNKRVTWR`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FBSC40N01AH5PRZ1QNKRVTWR-task-add-binary-vs-hex-provider-benchmark-dimens`