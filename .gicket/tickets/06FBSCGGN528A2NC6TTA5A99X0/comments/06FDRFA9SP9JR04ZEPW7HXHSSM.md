[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSCGGN528A2NC6TTA5A99X0'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCGGN528A2NC6TTA5A99X0`.
- Optimistic claim succeeded (`expectedRevision=06FDRDCA1STSCF4ZZJBPK843CR`, `currentRevision=06FDRDM2XW0TW8TSJCJ60DMFSW`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCGGN528A2NC6TTA5A99X0-task-close-postgresql-pit-and-bridge-read-gaps' from source '70ccff9a4ce68889439c2b6d483e045abb46f0b6'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSCGGN528A2NC6TTA5A99X0-task-close-postgresql-pit-and-bridge-read-gaps` as `8b2d4199197e`.

Open questions / Risiken
- Risky assumption: Assumes the v0.32.0 smoke-read bundle is the accepted canonical evidence source for PostgreSQL PIT/bridge without needing any newer artifact lane.
- Split recommendation: No split recommended; the repository evidence supports one bounded PostgreSQL evidence-reclassification task.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9198`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b2db44e765a34ed8b495ac975fd08663`
- completed-at-utc: `<redacted>-18T19:37:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCGGN528A2NC6TTA5A99X0/runs/20260618T193744644Z-b2db44e765a34ed8b495ac975fd08663.json`