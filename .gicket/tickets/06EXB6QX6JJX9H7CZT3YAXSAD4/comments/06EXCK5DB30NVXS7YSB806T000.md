[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB6QX6JJX9H7CZT3YAXSAD4'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB6QX6JJX9H7CZT3YAXSAD4`.
- Optimistic claim succeeded (`expectedRevision=06EXCJ98V08KVW5WHFWFVST3EW`, `currentRevision=06EXCJC99142Y3DE9TZRY4C9WG`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB6QX6JJX9H7CZT3YAXSAD4-task-define-optional-advanced-configuration-hook' from source '08d53124a38d644336a30a43813434f16f844edd'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB6QX6JJX9H7CZT3YAXSAD4-task-define-optional-advanced-configuration-hook` as `e7bb87b2dbdf`.

Open questions / Risiken
- Risky assumption: The contract relies on a developer creating an architecture-level plan/document rather than runtime code; this is acceptable because implementation is explicitly out of scope, but the dev handoff should preserve that boundary.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9027`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `f6017385b61f4c97aca2715c2dee2e44`
- completed-at-utc: `<redacted>-28T22:54:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB6QX6JJX9H7CZT3YAXSAD4/runs/20260428T225401973Z-f6017385b61f4c97aca2715c2dee2e44.json`