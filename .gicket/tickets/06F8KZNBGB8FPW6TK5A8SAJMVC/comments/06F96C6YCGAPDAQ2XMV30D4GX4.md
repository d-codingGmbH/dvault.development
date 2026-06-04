[gicket-bot] Run report (outcome: po-critic-non-blocking-apply)

Summary
- PO-critic review completed with a non-blocking assessment for ticket '06F8KZNBGB8FPW6TK5A8SAJMVC'.

Changes applied
- Lease label `bot/lease:hp-ai-<redacted>.1` was applied to ticket `06F8KZNBGB8FPW6TK5A8SAJMVC`.
- Optimistic claim succeeded (`expectedRevision=06F969PTH4CPK17KGQK2M8B5R8`, `currentRevision=06F969W6GDEHP4E7XM9Z6RJW7M`).
- Dispatch ownership was assigned to instance `hp-ai-<redacted>.1`.
- Prepared interactive po-critic scratch worktree for target branch 'ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua' from source '85c2fda4f67e932164d284960c012c109caebbb8'.
- Removed bot-owned orchestration details from the persisted PO-critic assessment.
- Cleared stale blocked label(s) during successful handoff: blocked/dev, blocked/test.
- Published one structured PO-critic review contract comment.
- Updated ticket fields (labels: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed]).
- Published runtime write-group comment template 'handover-dev'.
- Committed transactional ticket writeback for TP `TP2` on branch `ticket/06F8KZNBGB8FPW6TK5A8SAJMVC-story-strengthen-provider-specific-migration-gua` as `988b9d73160c`.

Open questions / Risiken
- Risky assumption: The contract leaves representative loadTimestampStorage variants to implementation judgment instead of enumerating an exact provider-by-token matrix.
- Risky assumption: The bounded EF migration-operation set will stay stable enough that provider package upgrades do not introduce new in-scope operation shapes without tests catching them.
- Split recommendation: No split is needed for the current narrowed secondary-index, primary-key, and timestamp-guardrail lane.
- Split recommendation: If later work needs DVault-owned unique-constraint modeling or provider-specific unique-constraint migration support, open a separate follow-up ticket instead of widening this story.

Next steps
- Continue with role `dev` according to policy.
- Keep non-blocking findings visible during implementation and test planning.

Prompt cache usage
- prompt-tokens: `<redacted>`
- cached-tokens: `<redacted>`
- effective-cache-ratio: `0.9042`
- cache-details: `reported`

Local cost telemetry
- observed-cost-usd: `0.0000`
- observed-cost-status: `not-reported`
- estimated-savings-usd: `0.0000`
- estimated-savings-status: `estimated`

Run metadata
- role: `po-critic`
- run-id: `064ebff81ac94daea3f62284395c8c63`
- completed-at-utc: `<redacted>-04T15:11:<redacted>+00:00`
- context-artifact: `C:/Users/DavidUllrich/AppData/Local/Gicket/Bot/state/repos/cd017d7d6b3075ce91d0edb4f5f38707cb0ad486495b01fb8837637f16d0213c/tickets/06F8KZNBGB8FPW6TK5A8SAJMVC/runs/20260604T151153948Z-064ebff81ac94daea3f62284395c8c63.json`