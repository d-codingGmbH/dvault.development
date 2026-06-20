[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FE4QPR8TF8R6PXNM3RMXN8JG'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FE4QPR8TF8R6PXNM3RMXN8JG`.
- Optimistic claim succeeded (`expectedRevision=06FE8367HNJJQJGK4808YJ302C`, `currentRevision=06FE85AR9J3RD4RKSCBY6XKE84`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w' from source '52bfcb073c41a224395f8c93d40cc5410c13bed8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FE4QPR8TF8R6PXNM3RMXN8JG-task-tune-postgresql-latest-satellite-strategy-w` as `8335e48c34bb`.

Open questions / Risiken
- Risky assumption: The contract allows an equivalent preserved comparator instead of the standard provider-configured artifact triplet; developers still need to keep that comparator explicit enough to satisfy description.md:19, :30, and :40.
- Risky assumption: The historical relation .gicket/relations/SR/JG/06FE4QP6FB892E7TJMB47A3MSR--06FE4QPR8TF8R6PXNM3RMXN8JG--blocks.json remains present even though ticket 06FE4QP6FB892E7TJMB47A3MSR is done; the contract assumes that relation remains housekeeping-only.
- Split recommendation: No additional split is needed; provider-specific latest-satellite tuning stays isolated in this ticket and broader documentation promotion remains in 06FE4QRMXVGJVA65ZR5MZ817K8.
- Split recommendation: If the historical done-ticket blocks relation causes routing noise later, clean it up as separate relation housekeeping instead of widening this ticket.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9269`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `b010e99f3e524d4690206107a2b79aa1`
- completed-at-utc: `<redacted>-20T08:18:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FE4QPR8TF8R6PXNM3RMXN8JG/runs/20260620T081839631Z-b010e99f3e524d4690206107a2b79aa1.json`