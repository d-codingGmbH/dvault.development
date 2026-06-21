[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FE4R261S2FSQ786S4F4JE90R\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation\u0027 and commit \u00276b0ee5afb9ce\u0027 (ticket-comment branch\u002Bcommit reference; advanced to branch tip after newer repository changes).",
    "Advanced tester verification from stale pinned commit \u00273401dd0e29fa\u0027 to branch tip \u00276b0ee5afb9ce\u0027 because branch \u0027ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation\u0027 contains newer committed repository changes after the pinned commit.",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation\u0027 from source \u00276b0ee5afb9ce\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation\u0027.",
    "Evidence: git rev-parse resolved ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation to 6b0ee5afb9ce0cf9197f5515509715b936152135.",
    "Evidence: git diff --name-status a7427ddac..c44acde0e shows branch-only implementation changes in src/DCoding.Data.DVault/BuiltInStableHashService.cs, src/DCoding.Data.DVault/DefaultStableHashNormalizer.cs, src/DCoding.Data.DVault/DefaultDataVaultSaveService.cs, .gitignore, and artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621/* beyond ticket metadata.",
    "Evidence: docs/plans/performance-evidence-benchmark-artifact-contract.md states that before/after evidence must store two comparable artifact sets under one explicit scenario, ticket, or release label.",
    "Evidence: find artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621 -maxdepth 2 -type f returns only benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, allocation-hotspots.md, allocation-hotspots.csv, and allocation-hotspots.json; no before/ directory exists under the ticket label.",
    "Evidence: The repository already contains contract-shaped benchmark folders such as artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/before and /after.",
    "Evidence: artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/benchmark-summary.json versus artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621/benchmark-summary.json shows mean allocated bytes improved on all six targeted rows: canonicalization 1773205.33 to 1674637.33, digest 775904 to 416008, customer hub save-prep 1872101.33 to 1851160, order/product save-prep 5843261.33 to 5761440, unchanged replay 4165488 to 3992056, changed replay 5657104 to 5478696.",
    "Evidence: tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs and tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs keep published digest-vector and canonicalization assertions, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs contains unchanged-replay and changed-replay save assertions.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/hash-storage, area/performance, automation/bot-ready, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027-\u0027.",
    "Evidence: Ticket history references implementation commit \u00273401dd0e29fa\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "AC check passed: Implementation uses the hotspot ordering from artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/allocation-hotspots.md as the bounded optimization target set and prioritizes the dominant satellite replay/save-preparation allocations before lower-ranked micro-optimizations. (Branch-only implementation changes are limited to src/DCoding.Data.DVault/DefaultDataVaultSaveService.cs, DefaultStableHashNormalizer.cs, and BuiltInStableHashService.cs, and the refreshed allocation-hotspots.md still ranks AddSatellitesAsync, FilterSatellitePlansAsync, LoadLatestSatelliteHashDiffsAsync, NormalizeFields, and ComputeHash as the bounded target set.).",
    "AC check passed: The directly targeted rows improve or hold on mean allocated bytes, and the visible regression budget rules remain satisfied: targeted metrics improve or hold, required SQLite non-target regressions above 5% fail by default, and any configured optional-provider regression above 10% is explicitly called out and justified. (The authoritative hotspot baseline at artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/benchmark-summary.json versus the refreshed after artifact at artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621/benchmark-summary.json shows mean allocated bytes improved on all six targeted SQLite sha256-v1 HexString rows, and the after artifact records providerFilter=sqlite with optional providers skipped.).",
    "AC check passed: Stable hash behavior stays bit-for-bit compatible: current published digest vectors, algorithm ids, canonical lowercase-hex output, and normalization rules remain unchanged. (BuiltInStableHashService.cs keeps the existing stable-hash algorithm ids and digest lengths, DefaultStableHashNormalizer.cs preserves duplicate rejection and ordinal field sorting, and existing StableHashServiceTests.cs and StableHashNormalizerTests.cs still assert published digest vectors, lowercase-hex output, and canonical normalization rules.).",
    "AC check passed: Save-path behavior stays stable: unchanged replay continues to suppress duplicate satellite writes, changed replay continues to persist the expected new state, and provider-neutral versus provider-specific strategy-selection boundaries are not widened or redefined by this ticket. (The save-path changes stay inside provider-neutral DefaultDataVaultSaveService.cs, and ExplicitDataVaultSaveServiceSqliteTests.cs still covers unchanged replay with RowsWritten=0 and changed replay with RowsWritten=1 for satellite hash-diff filtering behavior.).",
    "DoD check passed: Repository code contains only low-risk allocation reductions inside the bounded DVault-owned hot paths and does not change caller-facing hash or storage contracts. (Observed branch-only code changes stay inside the bounded DVault-owned hot paths plus benchmark artifacts and .gitignore; no caller-facing hash or storage contract files were changed on the ticket branch.).",
    "DoD check passed: Benchmark evidence shows the targeted allocation rows improved or held under the same SQLite sha256-v1 HexString baseline and workload shapes used by the hotspot ticket. (The refreshed after artifact uses the same SQLite sha256-v1 HexString workload families as the 2026-06-21 hotspot baseline and shows lower mean allocated bytes for each targeted row.).",
    "DoD check passed: Existing unit and integration coverage, or equivalent updated tests, protect stable hash vectors, canonicalization rules, and unchanged-versus-changed satellite replay behavior after the allocation changes. (Repository coverage remains in place for stable-hash vectors in StableHashServiceTests.cs, canonicalization in StableHashNormalizerTests.cs, and unchanged-versus-changed replay behavior in ExplicitDataVaultSaveServiceSqliteTests.cs; StreamingExplicitSaveContractSnapshotTests.cs also asserts those compatibility tests remain present.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Comparable before/after evidence is produced with the existing contract from docs/plans/performance-evidence-benchmark-artifact-contract.md; if hotspot profiling is rerun, the standard benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json triplet remains authoritative and allocation-hotspots.* stays additive. (docs/plans/performance-evidence-benchmark-artifact-contract.md requires comparable before/after benchmark-summary triplets under one explicit label, but the branch adds only artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621 with one triplet plus additive allocation-hotspots sidecars and no sibling before/ artifact set.).",
    "DoD check failed: The refreshed evidence is sufficient for downstream docs ticket 06FE4R2EGQ444EGPKZBRZCDEV8 to cite measured results without reopening product-boundary questions. (The refreshed evidence is not sufficient for downstream docs work because it is not stored as one self-contained before/after artifact set under a single ticket label; reviewers still have to reconstruct the comparison from a separate baseline ticket directory.).",
    "DoD check failed: No residual PO blocker remains once the bounded optimization targets, regression budget, and evidence contract are documented. (A residual blocker remains because the checked-in benchmark artifact layout does not fully satisfy the documented performance-evidence contract.).",
    "Refreshed benchmark evidence is checked in only as artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621/*. The contract requires one label containing comparable before/benchmark-summary.* and after/benchmark-summary.* triplets, so the current evidence bundle is not contract-compliant.",
    "Because the baseline remains externalized in artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/, downstream docs cannot consume this ticket\u0027s refreshed evidence as one self-contained comparison bundle."
  ],
  "evidence": [
    "git rev-parse resolved ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation to 6b0ee5afb9ce0cf9197f5515509715b936152135.",
    "git diff --name-status a7427ddac..c44acde0e shows branch-only implementation changes in src/DCoding.Data.DVault/BuiltInStableHashService.cs, src/DCoding.Data.DVault/DefaultStableHashNormalizer.cs, src/DCoding.Data.DVault/DefaultDataVaultSaveService.cs, .gitignore, and artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621/* beyond ticket metadata.",
    "docs/plans/performance-evidence-benchmark-artifact-contract.md states that before/after evidence must store two comparable artifact sets under one explicit scenario, ticket, or release label.",
    "find artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621 -maxdepth 2 -type f returns only benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, allocation-hotspots.md, allocation-hotspots.csv, and allocation-hotspots.json; no before/ directory exists under the ticket label.",
    "The repository already contains contract-shaped benchmark folders such as artifacts/benchmarks/06F492CAB2293R7BGJWMWMRKT4-provider-neutral-read-allocations/before and /after.",
    "artifacts/benchmarks/06FE4R1XJVQZTQ8S9WN2YE3ZKW-allocation-hotspots-20260621/benchmark-summary.json versus artifacts/benchmarks/06FE4R261S2FSQ786S4F4JE90R-allocation-hotspots-after-20260621/benchmark-summary.json shows mean allocated bytes improved on all six targeted rows: canonicalization 1773205.33 to 1674637.33, digest 775904 to 416008, customer hub save-prep 1872101.33 to 1851160, order/product save-prep 5843261.33 to 5761440, unchanged replay 4165488 to 3992056, changed replay 5657104 to 5478696.",
    "tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs and tests/DCoding.Data.DVault.Tests/Unit/StableHashNormalizerTests.cs keep published digest-vector and canonicalization assertions, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs contains unchanged-replay and changed-replay save assertions.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/hash-storage, area/performance, automation/bot-ready, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027-\u0027.",
    "Ticket history references implementation commit \u00273401dd0e29fa\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Repackage the refreshed benchmark evidence under one contract-shaped ticket label with before/benchmark-summary.*, before/benchmark-summary.csv, before/benchmark-summary.json and matching after/* triplets, keeping allocation-hotspots.* additive beside the authoritative triplets.",
    "Populate the matching before set from the comparable 2026-06-21 SQLite sha256-v1 HexString hotspot baseline, or rerun a comparable before lane if a new baseline is required.",
    "Return the branch to test after the evidence bundle is corrected."
  ],
  "branchName": "ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation",
  "commitSha": "6b0ee5afb9ce"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FE4R261S2FSQ786S4F4JE90R`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FE4R261S2FSQ786S4F4JE90R-task-implement-targeted-hash-pipeline-allocation`