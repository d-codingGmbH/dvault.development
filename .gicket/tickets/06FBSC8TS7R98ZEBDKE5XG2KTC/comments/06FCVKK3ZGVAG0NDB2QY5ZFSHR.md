[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06FBSC8TS7R98ZEBDKE5XG2KTC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06FBSC8TS7R98ZEBDKE5XG2KTC`.
- Optimistic claim succeeded (`expectedRevision=06FCVHH399263RDHEWZYWHS3G0`, `currentRevision=06FCVHM09R87EDJYAP93M0XHVR`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06FBSC8TS7R98ZEBDKE5XG2KTC-story-define-provider-bulk-expansion-acceptance' from source '8b4354cb55529e319d33232b9650222c59df9947'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06FBSC8TS7R98ZEBDKE5XG2KTC-story-define-provider-bulk-expansion-acceptance` as `41588d98d9a5`.

Open questions / Risiken
- Risky assumption: Future provider implementation tickets will pin one exact provider/workload comparator and preserved artifact bundle before development starts; this story intentionally leaves that selection to later ticket creation.
- Risky assumption: Implementers will read `ordered explicit bulk batch or per-chunk ordered batch` consistently with the current `DefaultDataVaultSaveService` behavior and not as approval for a new provider-native chunk-execution surface.
- Split recommendation: No further split is needed for this acceptance-contract story.
- Split recommendation: Keep later implementation work per provider and split runtime save-strategy changes from design-time SQL artifact review when both are proposed.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9470`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `99490b645fc749e0905895c8cbac7381`
- completed-at-utc: `<redacted>-16T00:21:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06FBSC8TS7R98ZEBDKE5XG2KTC/runs/20260616T002155702Z-99490b645fc749e0905895c8cbac7381.json`