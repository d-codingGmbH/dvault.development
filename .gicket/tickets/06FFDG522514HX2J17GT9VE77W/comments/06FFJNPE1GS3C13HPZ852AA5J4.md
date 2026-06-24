[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FFDG522514HX2J17GT9VE77W'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FFDG522514HX2J17GT9VE77W`.
- Optimistic claim succeeded (`expectedRevision=06FFJHNPG9JSEPY1HWCKZY0258`, `currentRevision=06FFJM7SHKBPRGADJAZ0K6T2Z8`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FFDG522514HX2J17GT9VE77W-task-implement-mysql-ordinary-hub-parent-pit-ful' from source '6bb8643e1134c1c9c4c145c85ffa262a0981f31d'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FFDG522514HX2J17GT9VE77W-task-implement-mysql-ordinary-hub-parent-pit-ful` as `a8b46912766c`.

Open questions / Risiken
- Risky assumption: The official `MySql.EntityFrameworkCore` provider can prove rollback-clean behavior clearly enough to separate accepted savepoint-backed cases from provider-neutral fallback cases.
- Risky assumption: Existing MySQL capability-profile registration for both provider names will not accidentally widen PIT maintenance selection beyond the official-provider lane.
- Risky assumption: Documentation and tests will keep optimized PIT read evidence separate from PIT maintenance proof so timing claims are not overstated.
- Split recommendation: No additional split is needed for the current official-provider ordinary hub-parent full-rebuild lane.
- Split recommendation: If product later wants Pomelo live PIT maintenance, multi-active hub-parent PIT rebuilds, or link-parent PIT rebuilds, keep those as separate follow-up tickets rather than widening this slice.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `47737`
- cached-tokens: `9600`
- effective-cache-ratio: `0.2011`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `acd624bd37114f8da0b25c19a567cfbc`
- completed-at-utc: `<redacted>-24T11:14:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FFDG522514HX2J17GT9VE77W/runs/20260624T111436167Z-acd624bd37114f8da0b25c19a567cfbc.json`