[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F492CN76GS3CKM8EFD0C20XM'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F492CN76GS3CKM8EFD0C20XM`.
- Optimistic claim succeeded (`expectedRevision=06F52CQ53KKZHP9Z5QZQEVG0ZW`, `currentRevision=06F52F6V0S06FYKX4BGP533FA0`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco' from source '7a0a24c8fe821bb008d60d1970d76f377fb4f958'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F492CN76GS3CKM8EFD0C20XM-story-add-compiled-model-compiled-query-and-dbco` as `4dfd7bfd3362`.

Open questions / Risiken
- Risky assumption: The pooled benchmark assumes one fixed metadata source/model shape per context model; caller-owned discriminators must stay out of scope or the numbers will not match the documented cache-key rules.
- Risky assumption: The compiled-query benchmark assumes a stable shared-type table/projection shape and must not drift into dynamic IDataVaultReadService request composition.
- Risky assumption: SQL capture is only optional when the final claim is not about emitted SQL shape, index usage, or batching behavior; if docs cite those effects, SQL capture becomes part of the required evidence bundle.
- Split recommendation: No split recommended; compiled model, compiled query, and DbContext pooling still share one benchmark harness, one SQLite baseline, and one documentation boundary.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8782`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `5972623b3628437ebfa3025b5e1bbf37`
- completed-at-utc: `<redacted>-22T19:55:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/GicketBot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F492CN76GS3CKM8EFD0C20XM/runs/20260522T195551177Z-5972623b3628437ebfa3025b5e1bbf37.json`