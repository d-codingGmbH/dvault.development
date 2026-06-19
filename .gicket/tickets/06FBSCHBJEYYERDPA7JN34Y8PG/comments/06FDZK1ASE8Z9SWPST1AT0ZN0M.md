[gicket-bot] return-routing-v1

```json
{
  "sourceRole": "test",
  "targetRole": "dev",
  "resumeRole": "test",
  "returnKind": "rework_required",
  "returnCategory": null,
  "summary": "Tester workflow returned ticket \u002706FBSCHBJEYYERDPA7JN34Y8PG\u0027 for rework because persisted acceptance criteria, definition-of-done expectations, or checklist gates were not fully confirmed.",
  "changesApplied": [
    "Selected verification source branch \u0027ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and\u0027 and commit \u0027f7a3b0341bfa\u0027 (verification-source contract).",
    "Prepared interactive tester scratch worktree for target branch \u0027ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and\u0027 from source \u0027f7a3b0341bfa\u0027.",
    "Interactive tester tool loop completed review for branch \u0027ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and\u0027.",
    "Evidence: \u0060git diff --name-only develop..f7a3b0341bfa\u0060 lists \u0060docs/performance-profiles.md\u0060, \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md\u0060, \u0060docs/releases/v0.40.0.md\u0060, and \u0060docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md\u0060.",
    "Evidence: \u0060git ls-tree --name-only -d f7a3b0341bfa artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607\u0060 confirms the required smoke-read artifact directory exists at the claimed commit.",
    "Evidence: \u0060benchmark-summary.csv:18-23\u0060 contains completed SQLite latest-satellite/PIT/bridge rows, while \u0060benchmark-summary.csv:42-56\u0060 keeps PostgreSQL/SQL Server/MySQL/Oracle/DB2 read rows in the root quick baseline as skipped placeholders with planned strategies.",
    "Evidence: \u0060artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.csv:34-51\u0060 contains completed PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge rows.",
    "Evidence: \u0060docs/performance-profiles.md:17\u0060 conflicts with \u0060docs/plans/provider-optimization-gap-matrix.md:12-14\u0060 and \u0060docs/plans/provider-optimization-gap-matrix.md:70\u0060 on PostgreSQL latest-satellite status.",
    "Evidence: Ticket status at verification time is \u0027todo\u0027.",
    "Evidence: Ticket labels at verification time: [area/benchmarking, area/documentation, area/performance, area/provider-support, area/read-models, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Evidence: Ticket history references implementation branch \u0027develop\u0027.",
    "Evidence: Ticket history references implementation commit \u0027f7a3b0341bfa\u0027.",
    "Evidence: Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Evidence: Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Evidence: Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment.",
    "AC check passed: \u0060docs/performance-profiles.md\u0060 clearly separates measured provider-read evidence from implemented-but-unmeasured latest-satellite strategy lanes and from DB2 defer-lane posture. (\u0060docs/performance-profiles.md\u0060 separates SQLite completed latest-satellite/PIT/bridge timing, v0.32 smoke-read PIT/bridge evidence for PostgreSQL/SQL Server/MySQL/Oracle, and DB2 defer-lane wording.).",
    "AC check passed: \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md\u0060 stays aligned with current provider registrations, explicit PIT/bridge maintenance requirements, and finite fallback causes without implying automatic maintenance or new APIs. (\u0060docs/architecture/dvault-v1-pit-bridge-boundary.md\u0060 keeps strategy registration separate from timing claims, keeps PIT/bridge maintenance explicit, and preserves finite provider-neutral fallback causes without adding APIs.).",
    "AC check passed: \u0060docs/releases/v0.40.0.md\u0060 records the accepted read-parity posture without claiming benchmark reruns or completed timing beyond the checked-in evidence. (\u0060docs/releases/v0.40.0.md\u0060 adds a provider-read parity boundary and explicitly says v0.40.0 does not rerun benchmarks or add completed non-SQLite latest-satellite or DB2 PIT/bridge timing claims.).",
    "AC check passed: PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge outcomes are documented as completed timing only through the preserved v0.32.0 smoke-read bundle and not through skipped root quick-baseline rows. (The updated docs route PostgreSQL/SQL Server/MySQL/Oracle PIT and bridge timing claims through the checked-in v0.32 smoke-read bundle and treat skipped root quick-baseline rows as placeholders only.).",
    "AC check passed: PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite outcomes are documented as diagnostics-gated strategy or parity outcomes unless and until a provider-configured completed-timing lane exists. (The updated docs describe non-SQLite latest-satellite lanes as diagnostics-gated strategy or placeholder guidance rather than completed timing evidence.).",
    "AC check passed: DB2 PIT and bridge remain explicitly documented as unmeasured/deferred and no doc claims completed DB2 timing. (DB2 PIT and bridge stay explicitly deferred in the updated docs, and I did not find any completed DB2 timing claim.).",
    "DoD check passed: No updated doc still claims that non-SQLite latest-satellite reads already have completed timing evidence, or that DB2 PIT/bridge has completed timing evidence. (I did not find any updated doc claiming completed non-SQLite latest-satellite timing or completed DB2 PIT/bridge timing.).",
    "DoD check passed: The documentation keeps root skipped-placeholder rows framed as guidance with planned strategy facts, not as measured wins. (The updated docs frame root skipped-placeholder rows as guidance and planned-strategy facts, not measured wins.).",
    "DoD check passed: No benchmark rerun, provider implementation change, or supported-shape expansion is required to satisfy this ticket. (The branch diff is documentation-only for the live surfaces plus the refinement note; it does not require benchmark reruns, provider implementation changes, or supported-shape expansion to satisfy the ticket.).",
    "Update labels for tester handoff to role \u0027dev\u0027.",
    "Ticket already in configured tester target status \u0027todo\u0027."
  ],
  "findings": [
    "DoD check failed: The three live documentation surfaces named in scope are updated together and do not contradict \u0060docs/plans/provider-optimization-evidence-matrix.md\u0060 or \u0060docs/plans/provider-optimization-gap-matrix.md\u0060. (\u0060docs/performance-profiles.md:17\u0060 says PostgreSQL \u0060latest-satellite-read\u0060 remains an evidence-gap recommendation, but \u0060docs/plans/provider-optimization-gap-matrix.md:12-14\u0060 and \u0060docs/plans/provider-optimization-gap-matrix.md:70\u0060 mark that row as a closed capability gap; the updated docs therefore still contradict the authoritative gap matrix.).",
    "\u0060docs/performance-profiles.md:17\u0060 misstates the authoritative gap-matrix status by calling PostgreSQL \u0060latest-satellite-read\u0060 an evidence-gap recommendation even though \u0060docs/plans/provider-optimization-gap-matrix.md:12-14\u0060 and \u0060docs/plans/provider-optimization-gap-matrix.md:70\u0060 mark it as a closed capability gap. This breaks Definition of Done 1."
  ],
  "evidence": [
    "\u0060git diff --name-only develop..f7a3b0341bfa\u0060 lists \u0060docs/performance-profiles.md\u0060, \u0060docs/architecture/dvault-v1-pit-bridge-boundary.md\u0060, \u0060docs/releases/v0.40.0.md\u0060, and \u0060docs/plans/provider-read-parity-outcomes-benchmarks-refinement.md\u0060.",
    "\u0060git ls-tree --name-only -d f7a3b0341bfa artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607\u0060 confirms the required smoke-read artifact directory exists at the claimed commit.",
    "\u0060benchmark-summary.csv:18-23\u0060 contains completed SQLite latest-satellite/PIT/bridge rows, while \u0060benchmark-summary.csv:42-56\u0060 keeps PostgreSQL/SQL Server/MySQL/Oracle/DB2 read rows in the root quick baseline as skipped placeholders with planned strategies.",
    "\u0060artifacts/benchmarks/v0.32.0-06F9XD26D2MHVAKZ2GCZ67BEFC-smoke-read-20260607/benchmark-summary.csv:34-51\u0060 contains completed PostgreSQL, SQL Server, MySQL, and Oracle PIT/bridge rows.",
    "\u0060docs/performance-profiles.md:17\u0060 conflicts with \u0060docs/plans/provider-optimization-gap-matrix.md:12-14\u0060 and \u0060docs/plans/provider-optimization-gap-matrix.md:70\u0060 on PostgreSQL latest-satellite status.",
    "Ticket status at verification time is \u0027todo\u0027.",
    "Ticket labels at verification time: [area/benchmarking, area/documentation, area/performance, area/provider-support, area/read-models, automation/bot-ready, needs-test, type/task, bot/lease:hp-ai-2026-001.1].",
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
    "Ticket history references implementation branch \u0027develop\u0027.",
    "Ticket history references implementation commit \u0027f7a3b0341bfa\u0027.",
    "Observed behavior: ticket history contains traceable implementation branch/commit references.",
    "Observed behavior: the tester handoff already contains branch, commit, or structured delivery-outcome context that a human integrator can use for a decision.",
    "Ticket history contains an explicit tester handoff hint in a runtime-orchestration comment."
  ],
  "nextSteps": [
    "Close the remaining definition-of-done gaps before handing the ticket to the integrator gate.",
    "Re-run tester verification after completing the missing implementation, test, or documentation work.",
    "Update \u0060docs/performance-profiles.md:17\u0060 so its gap-matrix summary preserves PostgreSQL \u0060latest-satellite-read\u0060 as a closed capability gap while keeping the missing completed-timing caveat.",
    "Return the branch to test after that wording fix; if executable validation is needed afterward, run \u0060dotnet test DVault.slnx --nologo\u0060 and \u0060bash tools/check-format.sh\u0060 in the supported verification path."
  ],
  "branchName": "ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and",
  "commitSha": "f7a3b0341bfa"
}
```

[gicket-bot] runtime-orchestration template

- template: `handover-dev`
- transaction-point: `TP10`
- ticket-id: `06FBSCHBJEYYERDPA7JN34Y8PG`
- target-role: `dev`
- decision: `<none>`
- reason: `<none>`
- return-target: `<none>`
- conditions: `<none>`
- return-kind: `rework_required`
- resume-role: `test`
- branch: `ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and`