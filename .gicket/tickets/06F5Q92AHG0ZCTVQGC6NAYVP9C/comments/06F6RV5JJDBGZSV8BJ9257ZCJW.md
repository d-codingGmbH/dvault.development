[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F5Q92AHG0ZCTVQGC6NAYVP9C'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F5Q92AHG0ZCTVQGC6NAYVP9C`.
- Optimistic claim succeeded (`expectedRevision=06F6RKXWD4XW876BZG8RPZZJV0`, `currentRevision=06F6RSYDKPN8PV16W30X74GM48`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite' from source 'bec85c92a69b0aa1c5088aa31a2bbfb0c29e79e0'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F5Q92AHG0ZCTVQGC6NAYVP9C-story-generate-typed-latest-and-as-of-satellite` as `bb037938d1ae`.

Open questions / Risiken
- Risky assumption: The optional stable direct EF projection path stays inside the compiled-model and compiled-query compatibility boundary referenced by the contract and is not used to justify provider-specific SQL or runtime-shaped queries.
- Risky assumption: One authoritative metadata source per generated scope can be resolved consistently enough to preserve produced names, source fingerprint, parent reference data, ordinals, CLR types, and nullability across metadata-first, model-first, and code-first inputs.
- Split recommendation: No further split recommended; the persisted contract already isolates satellite generation from the PIT/bridge slice and the repository baseline supports that separation.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.8973`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `27838a3bad9442eebc723696c4c0a34b`
- completed-at-utc: `<redacted>-28T02:32:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F5Q92AHG0ZCTVQGC6NAYVP9C/runs/20260528T023203468Z-27838a3bad9442eebc723696c4c0a34b.json`