[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FE4QQ9VF7B74E60CXEHSS5XW\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0027 and commit \u0027c25f54ead589\u0027 (ticket-comment branch\u002Bcommit reference; advanced to branch tip after newer repository changes).",
    "Advanced tester verification from stale pinned commit \u002766df5965b5a7\u0027 to branch tip \u0027c25f54ead589\u0027 because branch \u0027ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0027 contains newer committed repository changes after the pinned commit.",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0027 from source \u0027c25f54ead589\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0027.",
    "Evidence: \u0060git diff --name-status develop...ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0060 shows one non-ticket code file change (\u0060benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs\u0060) plus a new artifact triplet under \u0060artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/\u0060; no canonical docs files changed.",
    "Evidence: \u0060benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:127-145\u0060 now returns \u0060MySqlDataVaultReadStrategy\u0060 for \u0060latest-satellite-read\u0060, which is the path used by \u0060benchmarks/DCoding.Data.DVault.Benchmarks/ReadBenchmarkServices.cs:22-30\u0060 to assert provider read-strategy selection.",
    "Evidence: \u0060artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.md:34\u0060 and \u0060benchmark-summary.json:95-110\u0060 record \u0060latest-satellite-read\u0060 for \u0060MySQL external provider\u0060 as \u0060completed\u0060 with mean \u006019.113\u0060 ms, \u0060selectedStrategy=MySqlDataVaultReadStrategy\u0060, and \u0060plannedReadStrategy=MySqlDataVaultReadStrategy\u0060.",
    "Evidence: \u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-30\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:272-320\u0060, \u0060src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs:321-374\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:224-262\u0060 still prove the current MySQL strategy registration, fallback gates, window-function SQL shape, and optional as-of cutoff behavior.",
    "Evidence: \u0060docs/performance-profiles.md:17,30,42\u0060, \u0060docs/plans/provider-optimization-gap-matrix.md:86\u0060, \u0060docs/plans/provider-optimization-evidence-matrix.md:286\u0060, and \u0060docs/releases/v0.42.0.md:45-49\u0060 still state that MySQL latest-satellite has no completed timing and remains a skipped/evidence-gap lane.",
    "Evidence: \u0060rg -n \u002206FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620\u0022 /mnt/c/Projects/DVault\u0060 returned no matches outside the new artifact directory, so the added artifact is not wired into the repository\u0027s canonical evidence surfaces.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, automation/bot-ready, provider/mysql, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0027.",
    "Evidence: Ticket history references implementation commit \u002766df5965b5a7\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "AC check passed: The ticket records the current MySQL latest-satellite runtime boundary: \u0060AddDVaultMySql()\u0060 registers \u0060MySqlDataVaultReadStrategy\u0060, and optimized latest-satellite reads are only eligible for MySQL-provider hub-parent, non-multi-active current/as-of satellite requests; provider mismatch, link-parent satellites, or multi-active satellites fall back to provider-neutral reads. (\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-30\u0060 still registers \u0060MySqlDataVaultReadStrategy\u0060 for read/PIT/bridge services, and \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:272-320\u0060 plus \u0060tests/DCoding.Data.DVault.Tests/Integration/MySqlExplicitDataVaultSaveServiceTests.cs:235-265\u0060 still prove supported MySQL latest/as-of reads select that strategy while provider mismatch, link-parent satellites, and multi-active satellites fall back.).",
    "AC check passed: The ticket records the current MySQL SQL-shape baseline at planning level: latest-satellite reads use \u0060ROW_NUMBER() OVER (PARTITION BY ParentHashKey ORDER BY LoadTimestamp DESC)\u0060 with parent-hash-key \u0060IN\u0060 batching and an optional \u0060LoadTimestamp \u003C= asOf\u0060 predicate before selecting one row per parent. (\u0060src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs:321-374\u0060 and \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:224-262\u0060 still lock the MySQL latest-satellite SQL shape to \u0060ROW_NUMBER() OVER (PARTITION BY ParentHashKey ORDER BY LoadTimestamp DESC)\u0060, parent-hash-key \u0060IN\u0060 batching, and the optional \u0060LoadTimestamp \u003C= asOf\u0060 predicate.).",
    "AC check passed: Any tuning change keeps deterministic row-selection semantics and preserves diagnostics visibility so supported runs can still explain why \u0060MySqlDataVaultReadStrategy\u0060 was selected or why provider-neutral fallback was used. (The product read-path code is unchanged, and the new benchmark wiring in \u0060benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:122-145\u0060 now makes the harness assert \u0060MySqlDataVaultReadStrategy\u0060 for \u0060latest-satellite-read\u0060; the new artifact row also preserves \u0060selectedStrategy\u0060, \u0060plannedReadStrategy\u0060, and \u0060fallbackCauses\u0060 diagnostics tokens.).",
    "AC check passed: Any claim of MySQL latest-satellite improvement is backed by provider-configured evidence with preserved run context; the skipped 2026-06-20 root placeholder row and the historical 2026-06-07 configured fallback row cannot be promoted into completed timing evidence by themselves. (The new completed MySQL latest-satellite row is preserved in a provider-configured artifact triplet under \u0060artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/\u0060, so the branch does not rely on the skipped root placeholder or the historical 2026-06-07 fallback row as standalone timing evidence.).",
    "AC check passed: The ticket keeps MySQL PIT/bridge completed timing separate from latest-satellite tuning and does not widen supported shapes, provider set, or public fallback behavior. (No supported read shapes, provider set, or fallback behavior changed. The branch adds benchmark evidence and harness assertion wiring only, and the new artifact still records latest-satellite, PIT, and bridge as separate rows with their own execution details.).",
    "AC check passed: If evidence does not show a bounded MySQL latest-satellite win, keeping the existing window-function path and documenting the deferral is an acceptable outcome. (The branch keeps the existing MySQL window-function implementation in place and does not introduce a new SQL shape; the only code change is benchmark assertion wiring plus the checked-in evidence artifact.).",
    "DoD check passed: A reviewer can tell whether the outcome is \u0027keep current MySQL latest-satellite shape\u0027 or \u0027apply tuned MySQL latest-satellite shape\u0027 without reopening architecture or scope questions. (The branch diff shows no MySQL SQL-shape/runtime change beyond benchmark assertion wiring, so the outcome is reviewable as \u0027keep current MySQL latest-satellite shape and add evidence\u0027 rather than \u0027apply a tuned shape\u0027.).",
    "DoD check passed: Supported current/as-of MySQL latest-satellite reads preserve provider-neutral result semantics and explicit fallback behavior after any tuning change. (Existing registration, gate, SQL-shape, and integration coverage for supported current/as-of MySQL latest-satellite reads remain intact, and no product-side read semantics changed in this branch.).",
    "DoD check passed: No surface produced by this ticket implies completed MySQL latest-satellite timing without configured evidence. (No surface in this branch claims completed MySQL latest-satellite timing without configured evidence; the only completed timing claim is the new provider-configured artifact triplet itself.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: Code, tests, benchmark evidence, and docs all agree that MySQL latest-satellite remains a bounded evidence-gap/tuning lane separate from MySQL PIT/bridge timing. (The new artifact records a completed MySQL latest-satellite timing row, but \u0060docs/performance-profiles.md:17,30,42\u0060, \u0060docs/plans/provider-optimization-gap-matrix.md:86\u0060, \u0060docs/plans/provider-optimization-evidence-matrix.md:286\u0060, and \u0060docs/releases/v0.42.0.md:45-49\u0060 still describe MySQL latest-satellite as a skipped/evidence-gap lane with no completed timing. Code/tests/benchmark evidence and docs do not agree.).",
    "DoD check failed: Downstream docs ticket \u006006FE4QRMXVGJVA65ZR5MZ817K8\u0060 has a clear handoff on whether to document a new measured MySQL latest-satellite claim or keep the current deferral language. (The downstream docs handoff is not materially wired in repository surfaces: \u0060rg -n \u002206FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620\u0022 /mnt/c/Projects/DVault\u0060 returns no references outside the new artifact directory, so the canonical docs/evidence system still does not tell readers whether to document a new measured MySQL latest-satellite claim or keep deferral language.).",
    "Canonical evidence documentation is stale: the new artifact triplet records completed MySQL latest-satellite timing, but \u0060docs/performance-profiles.md\u0060, \u0060docs/plans/provider-optimization-gap-matrix.md\u0060, \u0060docs/plans/provider-optimization-evidence-matrix.md\u0060, and \u0060docs/releases/v0.42.0.md\u0060 still classify MySQL latest-satellite as a skipped-placeholder/no-completed-timing lane.",
    "The new artifact triplet is orphaned from the documented evidence system: no canonical matrix, performance-profile, or release-note surface references \u0060artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620\u0060, so downstream consumers do not get a wired handoff on whether the MySQL latest-satellite evidence gap is now closed or still deferred."
  ],
  "evidence": [
    "\u0060git diff --name-status develop...ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0060 shows one non-ticket code file change (\u0060benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs\u0060) plus a new artifact triplet under \u0060artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/\u0060; no canonical docs files changed.",
    "\u0060benchmarks/DCoding.Data.DVault.Benchmarks/DataVaultBenchmarkHelpers.cs:127-145\u0060 now returns \u0060MySqlDataVaultReadStrategy\u0060 for \u0060latest-satellite-read\u0060, which is the path used by \u0060benchmarks/DCoding.Data.DVault.Benchmarks/ReadBenchmarkServices.cs:22-30\u0060 to assert provider read-strategy selection.",
    "\u0060artifacts/benchmarks/06FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620/benchmark-summary.md:34\u0060 and \u0060benchmark-summary.json:95-110\u0060 record \u0060latest-satellite-read\u0060 for \u0060MySQL external provider\u0060 as \u0060completed\u0060 with mean \u006019.113\u0060 ms, \u0060selectedStrategy=MySqlDataVaultReadStrategy\u0060, and \u0060plannedReadStrategy=MySqlDataVaultReadStrategy\u0060.",
    "\u0060src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-30\u0060, \u0060tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:272-320\u0060, \u0060src/DCoding.Data.DVault/DataVaultRelationalPitBridgeReadStrategy.cs:321-374\u0060, and \u0060tests/DCoding.Data.DVault.Tests/Unit/MySqlProviderCapabilityTests.cs:224-262\u0060 still prove the current MySQL strategy registration, fallback gates, window-function SQL shape, and optional as-of cutoff behavior.",
    "\u0060docs/performance-profiles.md:17,30,42\u0060, \u0060docs/plans/provider-optimization-gap-matrix.md:86\u0060, \u0060docs/plans/provider-optimization-evidence-matrix.md:286\u0060, and \u0060docs/releases/v0.42.0.md:45-49\u0060 still state that MySQL latest-satellite has no completed timing and remains a skipped/evidence-gap lane.",
    "\u0060rg -n \u002206FE4QQ9VF7B74E60CXEHSS5XW-mysql-latest-satellite-20260620\u0022 /mnt/c/Projects/DVault\u0060 returned no matches outside the new artifact directory, so the added artifact is not wired into the repository\u0027s canonical evidence surfaces.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, automation/bot-ready, provider/mysql, type/task, needs-test, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e\u0027.",
    "Ticket history references implementation commit \u002766df5965b5a7\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update the canonical evidence surfaces (\u0060docs/plans/provider-optimization-evidence-matrix.md\u0060, \u0060docs/plans/provider-optimization-gap-matrix.md\u0060, \u0060docs/performance-profiles.md\u0060, and \u0060docs/releases/v0.42.0.md\u0060) to either cite the new MySQL latest-satellite artifact triplet as completed timing or explicitly preserve deferral language with an unambiguous downstream handoff.",
    "Reference the new artifact path from the chosen canonical surface so the benchmark triplet is not left as an unwired delivery artifact.",
    "After the docs/evidence wiring is corrected, run \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 through legacy verification because this read-only tester session cannot execute those policy-defined verification commands."
  ],
  "branchName": "ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e",
  "commitSha": "c25f54ead589"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FE4QQ9VF7B74E60CXEHSS5XW`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FE4QQ9VF7B74E60CXEHSS5XW-task-tune-mysql-latest-satellite-strategy-with-e`