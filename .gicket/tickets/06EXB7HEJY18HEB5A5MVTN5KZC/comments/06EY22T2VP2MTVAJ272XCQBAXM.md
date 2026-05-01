[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06EXB7HEJY18HEB5A5MVTN5KZC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06EXB7HEJY18HEB5A5MVTN5KZC`.
- Optimistic claim succeeded (`expectedRevision=06EY21PN1CTSKB4R7FD50S6A5R`, `currentRevision=06EY21XFEQ19Z31RBJ9EC1D644`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently' from source '4e0d303c0af43b3546fdf13bb4e7a79643c72085'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06EXB7HEJY18HEB5A5MVTN5KZC-task-persist-hubs-and-links-idempotently` as `19b11ae86d5c`.

Open questions / Risiken
- Risky assumption: Reuse lookup still depends on the implementation matching the existing stable-hash normalization and field ordering exactly for both hub business keys and link participant keys; the ticket already captures this risk in description.md:67-71.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8502`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `36e54d0e9d044d6a8b68e3942e900c19`
- completed-at-utc: `<redacted>-01T00:58:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06EXB7HEJY18HEB5A5MVTN5KZC/runs/20260501T005824201Z-36e54d0e9d044d6a8b68e3942e900c19.json`