[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSCHBJEYYERDPA7JN34Y8PG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSCHBJEYYERDPA7JN34Y8PG`.
- Optimistic claim succeeded (`expectedRevision=06FDYNMH8KJAM0BFSXYCS2NX00`, `currentRevision=06FDYNTXXYGXN3A9XE1A13KXZM`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and' from source '9a69159dbb6671a949b5eda1d4277e1ad5e8d4ed'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSCHBJEYYERDPA7JN34Y8PG-task-document-provider-read-parity-outcomes-and` as `5b6b9cd4519d`.

Open questions / Risiken
- Risky assumption: Developers will treat the `## Follow-Up Questions` as later backlog routing and will not expand this ticket into provider-configured latest-satellite timing collection or DB2 PIT/bridge benchmark activation.
- Risky assumption: The checked-in v0.32.0 smoke-read bundle will remain the authoritative PIT/bridge timing source for PostgreSQL, SQL Server, MySQL, and Oracle until a later ticket explicitly supersedes it.
- Split recommendation: No split is needed for developer handoff. If later work is opened, keep it split between provider-configured latest-satellite timing collection and DB2 PIT/bridge evidence activation, as the current contract already recommends.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9034`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `ca84a22727ad41b78109938d85716c83`
- completed-at-utc: `<redacted>-19T10:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSCHBJEYYERDPA7JN34Y8PG/runs/20260619T101131837Z-ca84a22727ad41b78109938d85716c83.json`