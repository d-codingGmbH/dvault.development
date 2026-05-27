[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F5Q91DR1555RSBQT7KDST684\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma\u0027 and commit \u00276a1d413db40c\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma\u0027 from source \u00276a1d413db40c\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma\u0027.",
    "Evidence: git diff --name-only develop...6a1d413db40c -- artifacts/benchmarks benchmark-summary.md benchmark-summary.csv benchmark-summary.json returned no paths.",
    "Evidence: git ls-files --others --exclude-standard -- artifacts/benchmarks benchmark-summary.md benchmark-summary.csv benchmark-summary.json returned no untracked benchmark evidence files.",
    "Evidence: git diff --name-only develop...6a1d413db40c shows the claimed product changes in benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs, benchmarks/DCoding.Data.DVault.Benchmarks/README.md, benchmarks/DCoding.Data.DVault.Benchmarks/ReadModelBenchmarks.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs.",
    "Evidence: benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs at 6a1d413db40c adds readStrategyStatus= and readShapeProviderStatus= to read executionDetail, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md line 18 says read rows preserve that diagnostics-backed detail.",
    "Evidence: benchmark-summary.md at 6a1d413db40c lines 48-53 still show latest-satellite-read, pit-as-of-read, and bridge-traversal-read rows whose executionDetail stops at selectedStrategy= and omits readStrategyStatus= and readShapeProviderStatus=.",
    "Evidence: The new PIT/bridge verification lives in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs, covering link-parent PIT fallback, multi-active tuple PIT behavior, registry-by-name PIT rebuild, and bridge shrink via RebuildBridgeAsync(...).",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/benchmarks, area/diagnostics, area/ef-core, area/performance, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma\u0027.",
    "Evidence: Ticket history references implementation commit \u00276a1d413db40c\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: Diagnostics tests and integration coverage show explicit PIT read diagnostics and metadata-resolved PIT maintenance-name resolution paths stay equivalent for the implemented PIT surfaces, and fallback or gate details remain visible when provider-specific strategies decline a request shape. (tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs and tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs now assert explicit DataVaultPitAsOfReadRequest diagnostics, registry-by-name PIT rebuild coverage, and visible fallback causes when unsupported PIT shapes decline provider-specific selection.).",
    "AC check passed: Link-parent PIT evidence shows ParentHashKey-based row identity, declared snapshot-column order, provider-neutral fallback status, and no claim of provider-specific PIT optimization or a new registry PIT read API. (The added link-parent PIT unit/integration coverage asserts link-parent row identity, ordered snapshot-column projection, and provider-neutral UnsupportedPitShape fallback, and the diff adds no src/API surface for a registry PIT read request or provider-specific link-parent PIT optimization.).",
    "AC check passed: Multi-active PIT evidence shows tuple-aware row identity, PIT driving-key projection, deterministic row-selection and snapshot-lookup behavior, and expected index baselines for the shared-driving-key shape. (The multi-active PIT changes assert tuple-aware row identity, pitDrivingKeyProjection, tuple-based row-selection/snapshot-lookup behavior, and the expected composite index baseline in the PIT diagnostics and SQLite integration tests.).",
    "AC check passed: Bridge evidence proves the current explicit maintenance contract only: append-only MaintainBridgeAsync(...) keeps insert, update, and unchanged outcomes visible, post-maintenance many-to-many and hierarchy bridge reads remain correct, and any shrink or removal scenario cited by this ticket uses RebuildBridgeAsync(...) rather than implying incremental delete-aware maintenance. (tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs keeps append-only MaintainBridgeAsync(...) outcome visibility, reads back many-to-many and hierarchy bridge rows after maintenance, and uses RebuildBridgeAsync(...) for shrink/increased-depth correction instead of implying delete-aware incremental maintenance.).",
    "AC check passed: Benchmark-facing or diagnostics-facing wording added by this ticket does not promise delete-aware bridge maintenance, raw SQL, provider physical plans, automatic index creation, automatic PIT or bridge maintenance, or non-existent public request types. (benchmarks/DCoding.Data.DVault.Benchmarks/README.md keeps unsupported link-parent and multi-active PIT shapes as provider-neutral fallback evidence and does not add delete-aware bridge, raw SQL, provider physical-plan, automatic index, automatic maintenance, or non-existent request-type promises.).",
    "DoD check passed: Required unit, integration, and approval-snapshot coverage lands without widening the public API beyond the existing read-diagnostics, PIT read, bridge read, and maintenance-result surfaces. (The diff adds unit/integration coverage in the PIT and bridge test files and does not touch src/ or the public API snapshot files, so the repository shape stays within the existing read-diagnostics, PIT read, bridge read, and maintenance-result surfaces.).",
    "DoD check passed: The final contract leaves no PO ambiguity about registry-backed PIT meaning, link-parent and multi-active PIT optimization boundaries, or the bridge baseline of append-only MaintainBridgeAsync(...) plus RebuildBridgeAsync(...) for shrink. (The delivered tests and README stay within the contract\u0027s registry-backed PIT meaning and append-only bridge baseline and do not reintroduce PO ambiguity about a new registry PIT read API or delete-aware bridge maintenance.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Any new measured claim introduced by this ticket is backed by new root benchmark rows or a checked-in artifact bundle that conforms to docs/plans/performance-evidence-benchmark-artifact-contract.md, and unsupported PIT shapes remain visible as provider-neutral fallback evidence rather than implied optimization. (The branch diff for develop...6a1d413db40c contains no changed paths under artifacts/benchmarks and no refreshed benchmark-summary.md/.csv/.json, so the new benchmark-facing read-strategy evidence is not backed by checked-in benchmark output.).",
    "DoD check failed: Benchmark harness changes and any checked-in artifacts are reproducible under the existing root triplet or artifacts/benchmarks/\u003Clabel\u003E/before and after contract and preserve strategy and fallback execution detail. (BenchmarkRunner.cs now emits read-strategy and fallback execution detail, but no reproducible checked-in artifact bundle under artifacts/benchmarks/\u003Clabel\u003E/before|after and no refreshed root benchmark triplet were delivered to preserve that detail.).",
    "DoD check failed: Any narrowly necessary benchmark-surface or diagnostics-surface documentation is updated so shipped evidence matches the actual non-delete-aware bridge baseline, while the broader completeness rollup remains delegated to 06F5Q91M0PM17RP43ZQRPBDXP0. (benchmarks/DCoding.Data.DVault.Benchmarks/README.md was updated, but the checked-in benchmark evidence was not refreshed to match it; the shipped evidence still reflects the older read execution-detail surface.).",
    "Required benchmark evidence output is missing from the claimed change: no tracked or untracked additions were found under artifacts/benchmarks, and the root benchmark-summary triplet was not refreshed.",
    "The checked-in benchmark evidence is stale relative to the new harness/docs: BenchmarkRunner.cs and README.md describe diagnostics-backed read execution detail, but benchmark-summary.md still shows the older read rows without those fields."
  ],
  "evidence": [
    "git diff --name-only develop...6a1d413db40c -- artifacts/benchmarks benchmark-summary.md benchmark-summary.csv benchmark-summary.json returned no paths.",
    "git ls-files --others --exclude-standard -- artifacts/benchmarks benchmark-summary.md benchmark-summary.csv benchmark-summary.json returned no untracked benchmark evidence files.",
    "git diff --name-only develop...6a1d413db40c shows the claimed product changes in benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs, benchmarks/DCoding.Data.DVault.Benchmarks/README.md, benchmarks/DCoding.Data.DVault.Benchmarks/ReadModelBenchmarks.cs, tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs.",
    "benchmarks/DCoding.Data.DVault.Benchmarks/BenchmarkRunner.cs at 6a1d413db40c adds readStrategyStatus= and readShapeProviderStatus= to read executionDetail, and benchmarks/DCoding.Data.DVault.Benchmarks/README.md line 18 says read rows preserve that diagnostics-backed detail.",
    "benchmark-summary.md at 6a1d413db40c lines 48-53 still show latest-satellite-read, pit-as-of-read, and bridge-traversal-read rows whose executionDetail stops at selectedStrategy= and omits readStrategyStatus= and readShapeProviderStatus=.",
    "The new PIT/bridge verification lives in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs, and tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs, covering link-parent PIT fallback, multi-active tuple PIT behavior, registry-by-name PIT rebuild, and bridge shrink via RebuildBridgeAsync(...).",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/benchmarks, area/diagnostics, area/ef-core, area/performance, area/read-models, automation/bot-ready, needs-test, type/story, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma\u0027.",
    "Ticket history references implementation commit \u00276a1d413db40c\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Check in the required benchmark artifact bundle under artifacts/benchmarks/\u003Clabel\u003E/before and artifacts/benchmarks/\u003Clabel\u003E/after from a run that includes the updated read executionDetail fields.",
    "Refresh any repository-facing benchmark summary files used as ticket evidence so the latest/PIT/bridge read rows show readStrategyStatus, readShapeProviderStatus, and fallback detail consistent with the updated harness.",
    "After the evidence files are committed, rerun dotnet test DVault.slnx --nologo and bash tools/check-format.sh in the supported verification environment."
  ],
  "branchName": "ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma",
  "commitSha": "6a1d413db40c"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F5Q91DR1555RSBQT7KDST684`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F5Q91DR1555RSBQT7KDST684-story-add-pit-and-bridge-diagnostics-and-benchma`