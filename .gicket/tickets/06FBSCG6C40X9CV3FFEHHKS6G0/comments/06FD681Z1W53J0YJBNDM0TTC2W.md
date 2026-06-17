[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FBSCG6C40X9CV3FFEHHKS6G0\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap\u0027 and commit \u002705d5e04355f9\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap\u0027 from source \u002705d5e04355f9\u0027.",
    "Prompt-injection writeback protection rejected tester interactive assessment writeback on attempt 1/2; retrying tester output generation before creating a durable stop.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap\u0027.",
    "Evidence: \u0060git rev-list --left-right --count develop...05d5e04355f9\u0060 returned \u00600 8\u0060, and the branch diff includes product, test, benchmark, and documentation changes for DB2 read-strategy support.",
    "Evidence: \u0060src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs:21-26\u0060 adds \u0060Db2DataVaultReadStrategy\u0060 as \u0060IDataVaultProviderReadStrategy\u0060 while retaining PIT and bridge registrations.",
    "Evidence: \u0060tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs:222-286\u0060 now asserts \u0060Db2DataVaultReadStrategy\u0060 is selected for latest/as-of/PIT/bridge diagnostics and verifies representative latest/as-of/PIT/bridge read behavior.",
    "Evidence: \u0060docs/plans/provider-optimization-evidence-matrix.md:268-271\u0060 and the root benchmark triplet update DB2 latest-satellite to a skipped placeholder with planned \u0060Db2DataVaultReadStrategy\u0060, explicitly avoiding completed timing claims.",
    "Evidence: \u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:879-904\u0060 still emits provider-neutral read guidance that excludes DB2 from the repository-proven optimized-provider set; \u0060Db2DataVaultReadStrategy\u0060 is absent from both \u0060IsRepositoryProvenOptimizedReadStrategy(...)\u0060 and \u0060FormatOptimizedReadProviderName(...)\u0060.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, provider/db2, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap\u0027.",
    "Evidence: Ticket history references implementation commit \u002705d5e04355f9\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: The authoritative outcome is explicit: either AddDVaultDb2() gains a DB2 provider-specific latest/current/as-of satellite read strategy, or the ticket lands an authoritative no-work-required rejection that keeps DB2 latest-satellite reads on the provider-neutral path. (\u0060AddDVaultDb2()\u0060 now registers \u0060Db2DataVaultReadStrategy\u0060 as \u0060IDataVaultProviderReadStrategy\u0060 in addition to the existing PIT and bridge registrations (\u0060src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs:21-26\u0060).).",
    "AC check passed: If a DB2 latest-satellite strategy is added, it is diagnostics-gated and bounded to hub-parent, non-multi-active satellite reads, with provider-neutral fallback preserved for provider mismatch, unsupported parent shapes, multi-active satellites, incomplete evidence, and the existing finite fallback posture. (\u0060DataVaultProviderReadStrategyGateEvaluator\u0060 now adds DB2 latest-satellite gating for provider match, hub-parent satellites, and non-multi-active shapes, and the DB2 smoke test asserts DB2 selection for latest/as-of/PIT/bridge reads (\u0060src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs\u0060, \u0060tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs:222-286\u0060).).",
    "AC check passed: Benchmark and provider-evidence surfaces stay truthful to the chosen outcome: completed DB2 timing claims require a configured DB2 artifact triplet, while a rejection or no-work-required outcome preserves the skipped-placeholder latest-satellite evidence boundary and does not imply measured optimization. (The root benchmark triplet and evidence matrix keep DB2 latest-satellite as a skipped placeholder with planned \u0060Db2DataVaultReadStrategy\u0060, not completed timing (\u0060benchmark-summary.md\u0060, \u0060benchmark-summary.json\u0060, \u0060benchmark-summary.csv\u0060, \u0060docs/plans/provider-optimization-evidence-matrix.md:268-271\u0060).).",
    "DoD check passed: If the ticket resolves as no-work-required, the authoritative ticket or planning handoff surface states that DB2 latest-satellite remains provider-neutral by design in the current baseline. (The branch chose the implementation path, so the no-work-required handoff surface is not required for this outcome.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "AC check failed: Tests cover the chosen outcome, including strategy registration and gate behavior, DB2 latest/current/as-of read diagnostics or rejection posture, and representative DB2 read behavior without regressing PIT/bridge expectations. (The branch adds registration, gate, smoke, and benchmark coverage, but it leaves a user-visible diagnostics surface stale: \u0060DefaultDataVaultDiagnosticsService\u0060 still describes DB2-selected reads as provider-neutral guidance, and no test in this branch covers that recommendation path.).",
    "AC check failed: Any touched diagnostics or documentation surfaces remain aligned on the same DB2 read boundary and do not infer DB2 latest-satellite optimization from PIT/bridge candidate evidence. (Diagnostics and documentation are not fully aligned. Docs and smoke coverage now present DB2 latest/PIT/bridge as diagnostics-gated candidates, but \u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:879-904\u0060 still names only SQLite as the optimized latest-satellite provider and only SQLite/PostgreSQL/SQL Server as optimized PIT/bridge providers.).",
    "DoD check failed: The chosen outcome is implemented or documented on the ticket branch with the relevant repository tests passing for the touched surfaces. (The implementation is present, but the remaining diagnostics defect means the touched surface is not complete, so the branch is not ready for tester acceptance.).",
    "DoD check failed: DB2 latest-satellite diagnostics, fallback behavior, smoke expectations, and benchmark or evidence references are internally consistent after the change. (DB2 latest-satellite diagnostics, fallback messaging, smoke expectations, and benchmark references are not internally consistent after the change because provider-tuning/read-shape recommendation text still falls back to provider-neutral messaging for DB2-selected reads (\u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:879-904\u0060).).",
    "DoD check failed: No remaining changed surface claims DB2 has provider-specific latest-satellite optimization unless the repository also contains the supporting registration, diagnostics selection, and benchmark evidence. (Changed docs and benchmark surfaces claim DB2 provider-specific latest-satellite support exists, but one diagnostics surface still omits DB2 from the supported optimized-provider set, so the supporting diagnostics-selection story is incomplete.).",
    "Blocking: \u0060DefaultDataVaultDiagnosticsService\u0060 was not updated for DB2 read strategies. Even when \u0060Db2DataVaultReadStrategy\u0060 is selected, the provider-tuning/read-shape recommendation path still reports provider-neutral guidance and omits DB2 from the repository-proven optimized-provider set (\u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:879-904\u0060).",
    "The current tests cover DB2 registration, gating, smoke reads, and benchmark metadata, but they do not exercise the stale diagnostics recommendation branch, so this inconsistency can ship unnoticed."
  ],
  "evidence": [
    "\u0060git rev-list --left-right --count develop...05d5e04355f9\u0060 returned \u00600 8\u0060, and the branch diff includes product, test, benchmark, and documentation changes for DB2 read-strategy support.",
    "\u0060src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs:21-26\u0060 adds \u0060Db2DataVaultReadStrategy\u0060 as \u0060IDataVaultProviderReadStrategy\u0060 while retaining PIT and bridge registrations.",
    "\u0060tests/DCoding.Data.DVault.Tests/Integration/Db2DataVaultSmokeTests.cs:222-286\u0060 now asserts \u0060Db2DataVaultReadStrategy\u0060 is selected for latest/as-of/PIT/bridge diagnostics and verifies representative latest/as-of/PIT/bridge read behavior.",
    "\u0060docs/plans/provider-optimization-evidence-matrix.md:268-271\u0060 and the root benchmark triplet update DB2 latest-satellite to a skipped placeholder with planned \u0060Db2DataVaultReadStrategy\u0060, explicitly avoiding completed timing claims.",
    "\u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs:879-904\u0060 still emits provider-neutral read guidance that excludes DB2 from the repository-proven optimized-provider set; \u0060Db2DataVaultReadStrategy\u0060 is absent from both \u0060IsRepositoryProvenOptimizedReadStrategy(...)\u0060 and \u0060FormatOptimizedReadProviderName(...)\u0060.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/performance, area/provider-support, area/read-models, area/testing, automation/bot-ready, needs-test, provider/db2, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap\u0027.",
    "Ticket history references implementation commit \u002705d5e04355f9\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Extend/adjust automated tests and evidence so every acceptance-criteria item is explicitly observed.",
    "Re-run tester verification after updating tests or implementation.",
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update \u0060src/DCoding.Data.DVault/DefaultDataVaultDiagnosticsService.cs\u0060 so DB2 latest/PIT/bridge selections are treated as repository-backed optimized read paths and the provider name is formatted consistently in recommendations.",
    "Add diagnostics tests that assert the provider-tuning/read-shape recommendation content when \u0060Db2DataVaultReadStrategy\u0060 is selected.",
    "After the fix, run policy-defined verification outside this read-only review path: \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060."
  ],
  "branchName": "ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap",
  "commitSha": "05d5e04355f9"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FBSCG6C40X9CV3FFEHHKS6G0`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FBSCG6C40X9CV3FFEHHKS6G0-task-close-db2-latest-satellite-read-gap`