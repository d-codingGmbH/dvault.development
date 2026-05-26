[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F5Q8ZM9N9Z8J5SCGRY989904\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F5Q8ZM9N9Z8J5SCGRY989904-story-evaluate-and-implement-oracle-staged-bulk\u0027 and commit \u0027b96100350b00\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F5Q8ZM9N9Z8J5SCGRY989904-story-evaluate-and-implement-oracle-staged-bulk\u0027 from source \u0027b96100350b00\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F5Q8ZM9N9Z8J5SCGRY989904-story-evaluate-and-implement-oracle-staged-bulk\u0027.",
    "Evidence: git diff --name-only develop...b96100350b00 changes README.md, benchmark-summary.{md,csv,json}, benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs, docs/architecture/dvault-v1-explicit-save-service.md, docs/plans/performance-evidence-benchmark-artifact-contract.md, docs/releases/v0.19.0.md, src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs, and tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs.",
    "Evidence: src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs:26-89 makes CanSave() depend on SelectOracleBulkSavePath(...); that selector returns only ProviderNeutralFallback or DirectOracleBatching and never selects StagedOracleBulk.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs:13-61 adds direct-path and fallback selector tests for provider mismatch, dirty context, under-threshold, oversized-satellite, and multi-active shapes.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs:21-67 still contains only the configured bulk-save happy path and a single-hub smoke path for Oracle opt-in integration.",
    "Evidence: A repo-wide search under tests/DCoding.Data.DVault.Tests for Oracle staged/cleanup/failure coverage returned no matches.",
    "Evidence: benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs:570-572 and benchmark-summary.{md,csv,json} record Oracle as the direct optimized path with stagedOracleBulk=not-selected-no-measured-win and skipped status when DVAULT_TEST_ORACLE_CONNECTION_STRING is not configured.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/ef-core, area/performance, area/persistence, area/provider-support, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F5Q8ZM9N9Z8J5SCGRY989904-story-evaluate-and-implement-oracle-staged-bulk\u0027.",
    "Evidence: Ticket history references implementation commit \u0027b96100350b00\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The repository records the Oracle decision boundary between the existing direct Oracle path, a staged Oracle path, and provider-neutral fallback for eligible ordered bulk batches, including the conditions under which each path is selected or declined. (README, the architecture/release docs, benchmark metadata, and OracleDataVaultSaveStrategy now record direct Oracle batching, a reserved staged Oracle branch, and provider-neutral fallback with explicit Oracle gate conditions.).",
    "AC check passed: Any Oracle staged path stays behind \u0060AddDVaultOracle()\u0060 and the existing \u0060IDataVaultSaveService\u0060 contract, uses the internal staging SPI from \u006006F5Q8YKR31DXGRXVPJ9031BQW\u0060, and does not introduce new public save APIs or public staging types. (The diff keeps Oracle behind AddDVaultOracle()/IDataVaultSaveService and does not add public save APIs or public staging types.).",
    "AC check passed: Staged Oracle execution is enabled only for shapes where evidence shows a net benefit over the current Oracle direct path and where stage creation, population, execution, cleanup, cancellation, and failure handling are deterministic under the caller-owned transaction boundary and Oracle limits. (The branch does not enable staged Oracle execution; the docs and benchmark metadata explicitly keep staged Oracle unselected until a measured win and deterministic cleanup are proven.).",
    "AC check passed: Shapes that do not beat or cannot safely satisfy the staged path keep the current Oracle direct path or provider-neutral fallback, and unsupported shapes such as dirty contexts, multi-active satellites, oversized batches, or missing Oracle prerequisites are declined deterministically. (Unsupported provider, dirty-context, minimum-operation, oversized-satellite, and multi-active shapes still decline deterministically, while eligible batches retain the direct Oracle path.).",
    "AC check passed: When Oracle is configured, benchmark evidence is captured through the existing optional-provider artifact contract with visible Oracle rows comparing the retained direct path and any staged path; when Oracle is not configured, the harness still preserves deterministic skipped Oracle rows instead of silently dropping the Oracle boundary. (BenchmarkRunner and the checked-in benchmark-summary triplet keep visible Oracle rows with skipped status when Oracle is not configured and record the retained direct-path versus not-selected staged-path boundary.).",
    "DoD check passed: Code and any supporting internal documentation make the retained-versus-staged Oracle boundary explicit enough that downstream benchmark work can extend evidence without reopening Oracle path-selection rules. (README, docs/architecture/dvault-v1-explicit-save-service.md, docs/releases/v0.19.0.md, and docs/plans/performance-evidence-benchmark-artifact-contract.md make the retained direct-versus-staged Oracle boundary explicit.).",
    "DoD check passed: The Oracle implementation reuses the completed shared staging SPI contract and does not change the settled public \u0060IDataVaultSaveService\u0060 or caller-owned transaction and cancellation semantics. (No public IDataVaultSaveService surface or caller-visible transaction/cancellation contract changed in the diff.).",
    "DoD check passed: Any Oracle benchmark evidence or planned skipped Oracle rows remains compatible with the existing benchmark summary triplet and the shared performance-evidence artifact contract. (The checked-in benchmark-summary.md/csv/json rows remain aligned with BenchmarkRunner and the benchmark artifact contract for planned skipped Oracle evidence.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Oracle-focused tests cover staged-path selection, retained direct-path selection, fallback behavior, persisted-row correctness for hub, link, and ordinary satellite batches, and cleanup or failure handling in the Oracle opt-in lane. (Only Oracle selector unit tests were added. The existing Oracle opt-in integration file remains happy-path only, and there is no Oracle staged-path selection or Oracle cleanup/failure coverage.).",
    "DoD check failed: Oracle unit tests and opt-in Oracle integration tests cover selected, declined, and fallback shapes for hub, link, and ordinary satellite batches, including cleanup and failure behavior for the staged path when that path is implemented. (Oracle unit tests cover selector decisions, but the opt-in Oracle integration lane still lacks declined/fallback and cleanup/failure coverage, and there is no staged-path test coverage.).",
    "The branch does not satisfy the ticket\u0027s Oracle test-coverage requirement: only selector unit coverage was added, while Oracle opt-in integration still lacks staged-path selection and cleanup/failure coverage."
  ],
  "evidence": [
    "git diff --name-only develop...b96100350b00 changes README.md, benchmark-summary.{md,csv,json}, benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs, docs/architecture/dvault-v1-explicit-save-service.md, docs/plans/performance-evidence-benchmark-artifact-contract.md, docs/releases/v0.19.0.md, src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs, and tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs.",
    "src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs:26-89 makes CanSave() depend on SelectOracleBulkSavePath(...); that selector returns only ProviderNeutralFallback or DirectOracleBatching and never selects StagedOracleBulk.",
    "tests/DCoding.Data.DVault.Tests/Unit/OracleProviderOptimizationTests.cs:13-61 adds direct-path and fallback selector tests for provider mismatch, dirty context, under-threshold, oversized-satellite, and multi-active shapes.",
    "tests/DCoding.Data.DVault.Tests/Integration/OracleDataVaultSmokeTests.cs:21-67 still contains only the configured bulk-save happy path and a single-hub smoke path for Oracle opt-in integration.",
    "A repo-wide search under tests/DCoding.Data.DVault.Tests for Oracle staged/cleanup/failure coverage returned no matches.",
    "benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs:570-572 and benchmark-summary.{md,csv,json} record Oracle as the direct optimized path with stagedOracleBulk=not-selected-no-measured-win and skipped status when DVAULT_TEST_ORACLE_CONNECTION_STRING is not configured.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/ef-core, area/performance, area/persistence, area/provider-support, area/testing, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q8ZM9N9Z8J5SCGRY989904-story-evaluate-and-implement-oracle-staged-bulk\u0027.",
    "Ticket history references implementation commit \u0027b96100350b00\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Add Oracle-focused coverage for the missing contract cases: staged-path selection if a staged path remains in scope, plus Oracle opt-in declined/fallback and cleanup/failure coverage.",
    "After the missing coverage is added, run policy verification through legacy execution for dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F5Q8ZM9N9Z8J5SCGRY989904-story-evaluate-and-implement-oracle-staged-bulk",
  "commitSha": "b96100350b00"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F5Q8ZM9N9Z8J5SCGRY989904`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F5Q8ZM9N9Z8J5SCGRY989904-story-evaluate-and-implement-oracle-staged-bulk`