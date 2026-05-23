[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F492CFSJHN0RGXXRG3KT63FM\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F492CFSJHN0RGXXRG3KT63FM-story-measure-and-tune-explicit-save-change-trac\u0027 and commit \u00273c1ca7e9589a\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F492CFSJHN0RGXXRG3KT63FM-story-measure-and-tune-explicit-save-change-trac\u0027 from source \u00273c1ca7e9589a\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F492CFSJHN0RGXXRG3KT63FM-story-measure-and-tune-explicit-save-change-trac\u0027.",
    "Evidence: git diff --name-only develop...3c1ca7e9589a shows only benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, src/DCoding.Data.DVault/DataVaultSaveService.cs, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs outside .gicket metadata.",
    "Evidence: src/DCoding.Data.DVault/DataVaultSaveService.cs now routes provider-neutral hub/link writes through CreateUniqueRowSavePlans(...), AddUniqueRowsAsync(...), GetTrackedHashKeys(...), and LoadPersistedUniqueHashKeysAsync(...), batching persisted hash-key lookups per produced table and restoring original order via Ordinal.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs adds DefaultSaveServiceBatchesUniqueRowExistenceChecksPerTable, asserting first-save RowsWritten=5 with one HubCustomer SELECT and replay RowsWritten=0 with zero HubCustomer SELECTs.",
    "Evidence: benchmark-summary.json contains 26 result rows with providerFilter=\u0027all\u0027, iterations=1, warmupIterations=0, optionalProviders length 4, 18 failed SQLite rows, and 8 skipped optional-provider rows.",
    "Evidence: benchmark-summary.md states scenario execution did not start because Microsoft.EntityFrameworkCore.Analyzers 10.0.8 was missing, so timing and allocation values remain blank.",
    "Evidence: docs/plans/performance-evidence-benchmark-artifact-contract.md requires before/after files under artifacts/benchmarks/\u003Clabel\u003E/before and after, but no artifacts/benchmarks/ paths appear in the branch diff and find artifacts/benchmarks ... | rg \u002706F492CFSJHN0RGXXRG3KT63FM|explicit-save-change-tracker\u0027 returned no matches.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/ef-core, area/performance, area/persistence, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F492CFSJHN0RGXXRG3KT63FM-story-measure-and-tune-explicit-save-change-trac\u0027.",
    "Evidence: Ticket history references implementation commit \u00273c1ca7e9589a\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: The story defines its v1 measurement baseline as the existing explicit IDataVaultSaveService save scenarios for customer profile history, bulk insert-only, bulk history, and order-product fulfillment history on required SQLite local temporary files, reusing the shared benchmark artifact contract. (The committed benchmark summary rows define the required SQLite explicit-save baseline scenarios: customer-profile-history, customer-profile-bulk-insert-only, customer-profile-bulk-history, and order-product-fulfillment-history.).",
    "AC check passed: The measured tuning target is bounded to explicit-save hot spots visible in the current codebase, such as repeated ChangeTracker scans, per-row existence checks or state churn, request metadata resolution/defaulting, or batching behavior in the shared save pipeline or SQLite save path. (src/DCoding.Data.DVault/DataVaultSaveService.cs now batches tracked and persisted unique-hash detection per produced hub/link table via CreateUniqueRowSavePlans(...), AddUniqueRowsAsync(...), GetTrackedHashKeys(...), and LoadPersistedUniqueHashKeysAsync(...).).",
    "AC check passed: Any optimization preserves current save semantics already covered by repository tests: hub/link replay still reports RowsWritten=0 when rows are reused, unchanged satellite replays still avoid new rows, saved-record ordering remains deterministic, and provider strategy selection/fallback remains explainable by the existing diagnostics gates. (Repository tests still cover replay RowsWritten=0, satellite hash-diff filtering, saved-record ordering, request hooks, and diagnostics fallback behavior, and the new Ordinal-based unique-row plan preserves hub/link record order.).",
    "AC check passed: If the tuning affects shared save-path behavior that can influence provider dispatch or optional provider evidence rows, the artifact set keeps those optional provider rows visible as completed or skipped instead of silently dropping them. (The artifact set keeps optional-provider rows visible: benchmark-summary.json has four optionalProviders in context and eight provider-native bulk rows persisted as skipped rather than omitted.).",
    "DoD check passed: Regression coverage proves the optimization did not break explicit save semantics for reuse detection, satellite append-only/hash-diff behavior, deterministic saved-record ordering, or request hook resolution. (Regression coverage is present in ExplicitDataVaultSaveServiceSqliteTests.cs and DataVaultDiagnosticsIntegrationTests.cs for reuse detection, satellite append-only/hash-diff behavior, deterministic saved-record ordering, and request hook resolution.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Before/after evidence is persisted under one explicit label with benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json, and the paired runs keep comparable iterations, warmup, load-timestamp storage, provider filter, and provider execution/skip metadata. (The branch adds only root-level benchmark-summary.md/csv/json files; it does not add a labeled before/after evidence set under artifacts/benchmarks/\u003Clabel\u003E/before and after with paired run context.).",
    "AC check failed: Benchmark results show the targeted save metric improves or holds with allocation evidence preserved, and required SQLite non-target regressions above 5% fail unless explicitly justified under the shared performance-evidence contract. (benchmark-summary.json records all 18 required SQLite rows as executionStatus=\u0027failed\u0027, iterations=0, null timing/allocation fields, and persistedOutcome=\u0027not executed\u0027, so there is no measured improvement/hold evidence or non-target regression proof.).",
    "DoD check failed: Repository-facing code, tests, and benchmark artifacts identify the measured explicit-save hotspot and the bounded tuning or no-op conclusion clearly enough that downstream work does not need to reopen baseline questions. (The code and tests identify the hotspot, but the benchmark artifacts do not close the baseline question because there is no labeled before/after evidence set and no completed benchmark run for the ticket.).",
    "DoD check failed: The benchmark harness and related assertions continue to prove the required save artifact fields, allocation fields, and comparable before/after run context for this ticket\u0027s evidence set. (The benchmark harness contract remains in repository tests, but this ticket\u0027s committed evidence is a single failed run at the repository root, not a comparable before/after evidence set with completed allocation fields.).",
    "DoD check failed: The story lands either a measured improvement or a documented evidence-backed conclusion that no worthwhile tuning was justified, without speculative semantic changes. (The ticket lands tuning code, but the committed artifacts document only failed/not-executed SQLite benchmark rows, not a measured improvement or an evidence-backed no-op conclusion.).",
    "The ticket does not persist its benchmark evidence under a single labeled before/after artifact set; only root-level benchmark-summary.md/csv/json files were added.",
    "The committed benchmark artifacts contain no completed SQLite benchmark rows, so the ticket does not prove an improvement/hold outcome or the required allocation/regression budgets."
  ],
  "evidence": [
    "git diff --name-only develop...3c1ca7e9589a shows only benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, src/DCoding.Data.DVault/DataVaultSaveService.cs, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs outside .gicket metadata.",
    "src/DCoding.Data.DVault/DataVaultSaveService.cs now routes provider-neutral hub/link writes through CreateUniqueRowSavePlans(...), AddUniqueRowsAsync(...), GetTrackedHashKeys(...), and LoadPersistedUniqueHashKeysAsync(...), batching persisted hash-key lookups per produced table and restoring original order via Ordinal.",
    "tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs adds DefaultSaveServiceBatchesUniqueRowExistenceChecksPerTable, asserting first-save RowsWritten=5 with one HubCustomer SELECT and replay RowsWritten=0 with zero HubCustomer SELECTs.",
    "benchmark-summary.json contains 26 result rows with providerFilter=\u0027all\u0027, iterations=1, warmupIterations=0, optionalProviders length 4, 18 failed SQLite rows, and 8 skipped optional-provider rows.",
    "benchmark-summary.md states scenario execution did not start because Microsoft.EntityFrameworkCore.Analyzers 10.0.8 was missing, so timing and allocation values remain blank.",
    "docs/plans/performance-evidence-benchmark-artifact-contract.md requires before/after files under artifacts/benchmarks/\u003Clabel\u003E/before and after, but no artifacts/benchmarks/ paths appear in the branch diff and find artifacts/benchmarks ... | rg \u002706F492CFSJHN0RGXXRG3KT63FM|explicit-save-change-tracker\u0027 returned no matches.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/ef-core, area/performance, area/persistence, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F492CFSJHN0RGXXRG3KT63FM-story-measure-and-tune-explicit-save-change-trac\u0027.",
    "Ticket history references implementation commit \u00273c1ca7e9589a\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Persist the ticket evidence under artifacts/benchmarks/\u003Clabel\u003E/before and artifacts/benchmarks/\u003Clabel\u003E/after, with benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json in each directory.",
    "Rerun the required SQLite benchmark scenarios in a supported environment so the explicit-save SQLite rows carry completed timing and allocation metrics, or narrow the claim with explicit justification if no completed performance claim is being made.",
    "After the artifact rework, run deterministic verification for dotnet test DVault.slnx --nologo and bash tools/check-format.sh."
  ],
  "branchName": "ticket/06F492CFSJHN0RGXXRG3KT63FM-story-measure-and-tune-explicit-save-change-trac",
  "commitSha": "3c1ca7e9589a"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F492CFSJHN0RGXXRG3KT63FM`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F492CFSJHN0RGXXRG3KT63FM-story-measure-and-tune-explicit-save-change-trac`