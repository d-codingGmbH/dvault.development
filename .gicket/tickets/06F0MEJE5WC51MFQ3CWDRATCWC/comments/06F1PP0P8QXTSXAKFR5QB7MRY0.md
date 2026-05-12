[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706F0MEJE5WC51MFQ3CWDRATCWC\u0027 for rework because persisted acceptance criteria or definition-of-done expectations were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti\u0027 and commit \u002791be286ac212\u0027 (ticket-comment branch\u002Bcommit reference).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti\u0027 from source \u002791be286ac212\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti\u0027.",
    "Evidence: \u0060git show --stat --oneline 91be286ac212\u0060 reported 13 changed files, including new \u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategyContext.cs\u0060, \u0060src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs\u0060, and matching test/snapshot updates.",
    "Evidence: \u0060git diff --name-only 91be286ac212..HEAD -- src tests benchmarks\u0060 returned no output; later branch commits after the claimed implementation only updated \u0060.gicket\u0060 comments, not product/test/benchmark code.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs:10-43\u0060 defines the public read-strategy hook, and \u0060src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:20-40,56-76\u0060 dispatches both row reads and typed projection reads through ordered provider strategies before provider-neutral fallback.",
    "Evidence: \u0060src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-31\u0060 registers \u0060SqliteDataVaultReadStrategy\u0060; \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:196-207\u0060 verifies \u0060AddDVault()\u0060 has no read strategy and \u0060AddDVaultSqlite()\u0060 has one.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:9-113\u0060 covers priority ordering, registration-order tie-breaks, typed projection routing, and no-strategy fallback.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:925-951\u0060 verifies supported latest/as-of SQLite read values, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs:118-161,165-254\u0060 verifies typed latest/as-of reads plus fallback behavior for unsupported link-parent and multi-active shapes.",
    "Evidence: Tracked comment \u0060.gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1PFA84GGY7QG3AENNN99RC0.md:8-18\u0060 records passing build, test, format, and Release SQLite benchmark smoke for implementation commit \u006091be286ac212\u0060.",
    "Evidence: Local artifact \u0060artifacts/benchmarks/ticket-06F0MEJE-repair-smoke/benchmark-summary.md:13-21,40-45\u0060 records ProviderDefault/sqlite run context and completed benchmark rows, including \u0060latest-satellite-read\u0060 fallback 10.862 ms vs optimized 10.226 ms.",
    "Evidence: \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:425-437\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:673-686\u0060 show that the existing public diagnostics interface now has new abstract members.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/tests, automation/bot-ready, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Evidence: Configured tester success handoff role is \u0027integrator\u0027.",
    "Evidence: Ticket description contains a persisted delivery contract block.",
    "Evidence: Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Evidence: Ticket description contains persisted acceptance criteria.",
    "Evidence: Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Evidence: Ticket description contains persisted definition-of-done expectations.",
    "Evidence: Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Evidence: Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Evidence: Observed behavior: role handoff templates are persisted in ticket history.",
    "Evidence: Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Evidence: Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Evidence: Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Evidence: Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Evidence: Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Evidence: Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Evidence: Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Evidence: Ticket history references implementation branch \u0027ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti\u0027.",
    "Evidence: Ticket history references implementation commit \u002791be286ac212\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Evidence: Ticket history contains 1 structured return-routing contract comment(s).",
    "Evidence: Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths.",
    "AC check passed: Core exposes a provider read-strategy hook with IDataVaultProviderReadStrategy and DataVaultProviderReadStrategyContext, following the existing save-strategy priority and registration-order dispatch semantics. (Commit 91be286 adds \u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs\u0060 and \u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategyContext.cs\u0060; \u0060src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:12-17\u0060 sorts by \u0060Priority\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:9-74\u0060 covers priority and registration-order dispatch.).",
    "AC check passed: DefaultDataVaultReadService.ReadLatestSatelliteRowsAsync selects a compatible registered provider read strategy for supported requests and otherwise preserves the DataVaultSatelliteReadPipeline fallback behavior. (\u0060src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:20-40\u0060 dispatches \u0060ReadLatestSatelliteRowsAsync\u0060 through the first compatible strategy and otherwise falls back to \u0060DataVaultSatelliteReadPipeline.ReadLatestReadRecordsAsync(...)\u0060; \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:106-113\u0060 covers the no-strategy fallback path.).",
    "AC check passed: The typed latest satellite projection path uses the same selection/fallback decision for supported optimized reads and returns projections matching fallback semantics, including nullable payload handling and load timestamp conversion. (\u0060src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:56-76\u0060 routes \u0060IDataVaultSatelliteProjectionReadService\u0060 through the same strategy selection; \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:76-103\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs:118-161\u0060 cover typed projections, as-of behavior, and preserved payload/timestamp mapping.).",
    "AC check passed: Read-strategy diagnostics are available for DataVaultLatestSatelliteReadRequest analysis and report provider selected, provider-neutral fallback, candidates, selected strategy name/priority, and fallback causes without changing existing save-strategy diagnostics semantics. (\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs\u0060 adds \u0060DataVaultReadStrategyDiagnostics*\u0060 types and request-bound read analysis; \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:63-120\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs:93-132\u0060 verify selected strategy, candidates, fallback status, and fallback causes.).",
    "AC check passed: AddDVaultSqlite registers SqliteDataVaultReadStrategy; AddDVault alone does not register a provider read strategy and continues through provider-neutral fallback. (\u0060src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-31\u0060 registers \u0060SqliteDataVaultReadStrategy\u0060; \u0060src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-29\u0060 stays provider-neutral; \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:196-207\u0060 proves \u0060AddDVault()\u0060 exposes no read strategy while \u0060AddDVaultSqlite()\u0060 exposes one.).",
    "AC check passed: For the supported SQLite latest/as-of satellite shape, optimized output matches fallback output for row count, metadata/table names, parent hash keys, hash diff, load timestamp, record source, payload values, and deterministic ordering. (\u0060tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:925-951\u0060 asserts supported latest/as-of SQLite read output fields (metadata name, table, parent hash key, hash diff, load timestamp, record source, payload values), and \u0060artifacts/benchmarks/ticket-06F0MEJE-repair-smoke/benchmark-summary.md:40-41\u0060 shows fallback and optimized latest-satellite rows both returned 100 seeded results.).",
    "AC check passed: Unsupported shapes and providers decline cleanly and continue through the fallback path with existing behavior. (\u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs:89-120\u0060 verifies clean decline with fallback causes for unsupported multi-active reads, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs:130-161\u0060 plus \u0060:165-254\u0060 show link-parent and multi-active reads continue through existing fallback behavior under \u0060AddDVaultSqlite()\u0060.).",
    "AC check passed: Benchmark evidence compares the pre-optimization fallback baseline and optimized SQLite latest-satellite read on the same machine/options, and the optimized row shows a measured mean-time improvement for the selected shape. (Tracked comment \u0060.gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1PFA84GGY7QG3AENNN99RC0.md:8-18\u0060 records the rerun command, and local artifact \u0060artifacts/benchmarks/ticket-06F0MEJE-repair-smoke/benchmark-summary.md:11-21,40-41\u0060 captures provider filter, iterations/warmup, ProviderDefault storage, machine/runtime context, and fallback 10.862 ms vs optimized 10.226 ms for \u0060latest-satellite-read\u0060.).",
    "AC check passed: Existing write-path tests and benchmark smoke coverage do not show write behavior regressions. (Tracked comment \u0060.gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1PFA84GGY7QG3AENNN99RC0.md:9-12\u0060 records passing build, test, format, and Release benchmark smoke; \u0060artifacts/benchmarks/ticket-06F0MEJE-repair-smoke/benchmark-summary.md:28-39\u0060 shows completed SQLite write-history rows, so smoke coverage did not surface write regressions.).",
    "DoD check passed: Core read-strategy contracts, dispatcher integration, fallback behavior, and read diagnostics are implemented and covered by unit or integration tests. (Core hook types, dispatcher, fallback, and read diagnostics are implemented in \u0060DataVaultProviderReadStrategy*.cs\u0060, \u0060DefaultDataVaultReadService.cs\u0060, and \u0060DataVaultDiagnostics.cs\u0060, with unit/integration coverage in \u0060DataVaultProviderReadStrategyTests.cs\u0060 and \u0060DataVaultDiagnosticsIntegrationTests.cs\u0060.).",
    "DoD check passed: SQLite read strategy implementation and AddDVaultSqlite registration are complete in the established provider package layout. (\u0060src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs\u0060 implements the SQLite read strategy and \u0060src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-31\u0060 wires it through the provider package registration path.).",
    "DoD check passed: Automated tests cover optimized selection, no-strategy fallback, decline-to-fallback behavior, latest read correctness, as-of read correctness, typed projection parity, diagnostics status/candidates/fallback causes, and provider registration. (Automated tests now cover selection ordering, no-strategy fallback, decline-to-fallback diagnostics, latest/as-of SQLite reads, typed projection behavior, and provider registration across \u0060DataVaultProviderReadStrategyTests.cs\u0060, \u0060DataVaultDiagnosticsIntegrationTests.cs\u0060, \u0060DataVaultTypedSatelliteReadServiceSqliteTests.cs\u0060, and \u0060ExplicitDataVaultSaveServiceTests.cs\u0060.).",
    "DoD check passed: Public API approval snapshots are updated if the hook or diagnostics surface is public. (\u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt\u0060 was updated to include the new hook and diagnostics surface.).",
    "DoD check passed: Before/after benchmark artifacts or ticket comments include command line, provider filter, iterations/warmup, load timestamp storage, run context, and measured rows used for the optimization choice. (The tracked manual evidence comment supplies the Release command line, and \u0060artifacts/benchmarks/ticket-06F0MEJE-repair-smoke/benchmark-summary.md:11-21,40-45\u0060 supplies provider filter, iterations/warmup, ProviderDefault storage, host/runtime context, and measured read rows.).",
    "DoD check passed: dotnet build DVault.slnx and dotnet test DVault.slnx pass in the expected local configuration. (Tracked comment \u0060.gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1PFA84GGY7QG3AENNN99RC0.md:9-10\u0060 reports \u0060dotnet build DVault.slnx --nologo\u0060 and \u0060dotnet test DVault.slnx --nologo\u0060 passed for implementation commit \u006091be286ac212\u0060.).",
    "DoD check passed: A SQLite benchmark smoke run using the existing benchmark host completes and includes the optimized latest-satellite read row. (Tracked comment \u0060.gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1PFA84GGY7QG3AENNN99RC0.md:12-18\u0060 plus local artifact \u0060artifacts/benchmarks/ticket-06F0MEJE-repair-smoke/benchmark-summary.md:40-45\u0060 show the SQLite benchmark smoke run completed and included the optimized latest-satellite row.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: No public API compatibility break is introduced beyond additive hook and diagnostics surface required by this ticket. (\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:425-437\u0060 adds new abstract overloads to the existing public \u0060IDataVaultDiagnosticsService\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:673-686\u0060 records them. That is a compatibility break for external implementers of the public interface, not just an additive new surface.).",
    "Public API compatibility break: \u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:425-437\u0060 adds new abstract \u0060Analyze(...)\u0060 overloads to the existing public \u0060IDataVaultDiagnosticsService\u0060 contract, and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:673-686\u0060 records them. Existing third-party implementations of that interface would now need new method bodies, so AC10 is not met."
  ],
  "evidence": [
    "\u0060git show --stat --oneline 91be286ac212\u0060 reported 13 changed files, including new \u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs\u0060, \u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategyContext.cs\u0060, \u0060src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs\u0060, and matching test/snapshot updates.",
    "\u0060git diff --name-only 91be286ac212..HEAD -- src tests benchmarks\u0060 returned no output; later branch commits after the claimed implementation only updated \u0060.gicket\u0060 comments, not product/test/benchmark code.",
    "\u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategy.cs:10-43\u0060 defines the public read-strategy hook, and \u0060src/DCoding.Data.DVault/DefaultDataVaultReadService.cs:20-40,56-76\u0060 dispatches both row reads and typed projection reads through ordered provider strategies before provider-neutral fallback.",
    "\u0060src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs:22-31\u0060 registers \u0060SqliteDataVaultReadStrategy\u0060; \u0060tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:196-207\u0060 verifies \u0060AddDVault()\u0060 has no read strategy and \u0060AddDVaultSqlite()\u0060 has one.",
    "\u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:9-113\u0060 covers priority ordering, registration-order tie-breaks, typed projection routing, and no-strategy fallback.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:925-951\u0060 verifies supported latest/as-of SQLite read values, and \u0060tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs:118-161,165-254\u0060 verifies typed latest/as-of reads plus fallback behavior for unsupported link-parent and multi-active shapes.",
    "Tracked comment \u0060.gicket/tickets/06F0MEJE5WC51MFQ3CWDRATCWC/comments/06F1PFA84GGY7QG3AENNN99RC0.md:8-18\u0060 records passing build, test, format, and Release SQLite benchmark smoke for implementation commit \u006091be286ac212\u0060.",
    "Local artifact \u0060artifacts/benchmarks/ticket-06F0MEJE-repair-smoke/benchmark-summary.md:13-21,40-45\u0060 records ProviderDefault/sqlite run context and completed benchmark rows, including \u0060latest-satellite-read\u0060 fallback 10.862 ms vs optimized 10.226 ms.",
    "\u0060src/DCoding.Data.DVault/DataVaultDiagnostics.cs:425-437\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:673-686\u0060 show that the existing public diagnostics interface now has new abstract members.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/tests, automation/bot-ready, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
    "Configured tester success handoff role is \u0027integrator\u0027.",
    "Ticket description contains a persisted delivery contract block.",
    "Observed behavior: a visible delivery contract is persisted in the ticket description.",
    "Ticket description contains persisted acceptance criteria.",
    "Observed behavior: acceptance criteria are explicitly persisted in the ticket description.",
    "Ticket description contains persisted definition-of-done expectations.",
    "Observed behavior: definition of done is explicitly persisted in the ticket description.",
    "Ticket history contains 7 persisted runtime-orchestration template comment(s).",
    "Observed behavior: role handoff templates are persisted in ticket history.",
    "Tester success path hands the ticket to integrator; final accept/rework decision happens after tester gate.",
    "Observed behavior: tester success continues at the integrator gate, so the final human integrator decision itself is not required yet.",
    "Observed behavior: tester success routes to \u0027integrator\u0027 while rework routes to \u0027dev\u0027, so handoff and rework paths are structurally distinguishable.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027dev\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027dev\u0027.",
    "Ticket history contains 1 runtime-orchestration template comment(s) targeting role \u0027po\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027po-critic\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027po-critic\u0027.",
    "Ticket history contains 2 runtime-orchestration template comment(s) targeting role \u0027test\u0027.",
    "Observed behavior: ticket history contains persisted handoff evidence for role \u0027test\u0027.",
    "Observed behavior: the ticket history shows a multi-role delivery loop across dev, po, po-critic, test.",
    "Ticket history references implementation branch \u0027ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti\u0027.",
    "Ticket history references implementation commit \u002791be286ac212\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "Ticket history contains 1 structured return-routing contract comment(s).",
    "Observed behavior: structured return-routing contracts preserve explicit rework or clarification paths."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Move latest-satellite diagnostic analysis off the existing \u0060IDataVaultDiagnosticsService\u0060 public interface (for example via extension methods, a new companion interface, or another additive API) so existing implementers remain compatible.",
    "After the compatibility-safe API adjustment, refresh the public API snapshot and rerun the deterministic build/test/benchmark verification already used for this ticket."
  ],
  "branchName": "ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti",
  "commitSha": "91be286ac212"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06F0MEJE5WC51MFQ3CWDRATCWC`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06F0MEJE5WC51MFQ3CWDRATCWC-task-implement-highest-impact-provider-read-opti`